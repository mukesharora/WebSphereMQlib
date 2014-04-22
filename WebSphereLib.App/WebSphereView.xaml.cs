﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using WebSphereLib.Messages;
using System.Collections.ObjectModel;
using WebSphereLib.App.Entity;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Configuration;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace WebSphereLib.App
{
    /// <summary>
    /// Interaction logic for WebSphereView.xaml
    /// </summary>
    public partial class WebSphereView : Window, INotifyPropertyChanged
    {
        #region Private Members

        private string _strQueueManagerName = string.Empty;
        private string _strChannelInfo = string.Empty;
        private string _getQueueName = string.Empty;
        private string _putQueueName = string.Empty;
        private string _putText = string.Empty;
        private ObservableCollection<QueueData> _queueDataList = null;
        private MQManager _manager = new MQManager();
        private long _uniqueId = 1;
        private bool _isMQConnected = false;

        #endregion

        #region Constructors

        public WebSphereView()
        {
            InitializeComponent();
            _strQueueManagerName = ConfigurationManager.AppSettings.Get("QueueManager");
            _strChannelInfo = string.Format("{0}/{1}/{2}", ConfigurationManager.AppSettings.Get("ChannelName"), ConfigurationManager.AppSettings.Get("ProtocolName"), ConfigurationManager.AppSettings.Get("ConnectionName"));
            _getQueueName = ConfigurationManager.AppSettings.Get("GetQueueName");
            _putQueueName = ConfigurationManager.AppSettings.Get("PutQueueName");
            this.DataContext = this;
            this._queueDataList = new ObservableCollection<QueueData>();
            //Checks for Network availability
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        #endregion

        #region Properties

        public ObservableCollection<QueueData> QueueDataList
        {
            get
            {
                return _queueDataList;
            }
            set
            {
                if (_queueDataList != value)
                {
                    _queueDataList = value;
                    this.OnPropertyChanged("QueueDataList");
                }
            }
        }


        #endregion

        #region Methods

        void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                MessageBox.Show("Network is available", "Information");
                this.Dispatcher.Invoke(new Action(() =>
                {
                    if (_isMQConnected)
                    {
                        this.btnSendMessage.IsEnabled = true;
                        this.btnRetrieveMessage.IsEnabled = true;
                    }

                }), DispatcherPriority.Normal);
            }
            else
            {
                MessageBox.Show("Network is unavailable", "Information");
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.btnSendMessage.IsEnabled = false;
                    this.btnRetrieveMessage.IsEnabled = false;

                }), DispatcherPriority.Normal);
            }
        }

        /// <summary>
        /// Serilizes the object to string
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="obj">Obj</param>
        /// <returns></returns>
        public string SerializeObject<T>(T obj)
        {
            XmlSerializer serializer = null;
            serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, obj);
                writer.Flush();
                return sb.ToString();
            }
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            string messageToSend = string.Empty;

            if (this.radioPartDelievered.IsChecked.HasValue && this.radioPartDelievered.IsChecked.Value)
            {
                PartDeliveredMessage partDeliveredMessage = new PartDeliveredMessage();
                partDeliveredMessage.MessageType = (MessageType)Enum.Parse(typeof(MessageType), Convert.ToString(this.comboMessageTypePartDelivered.Text), true);
                partDeliveredMessage.RouteColor = (RouteColor)Enum.Parse(typeof(RouteColor), Convert.ToString(this.comboColorPartDelivered.Text), true);
                partDeliveredMessage.PartNumber = this.txtPartNumberPartDelivered.Text.Trim();
                partDeliveredMessage.Timestamp = DateTime.Parse(this.txtTimestampPartDelivered.Text.Trim());
                partDeliveredMessage.UID = int.Parse(this.txtUniqueIdPartDelivered.Text.Trim());

                messageToSend = this.SerializeObject<PartDeliveredMessage>(partDeliveredMessage);
            }
            else
            {
                PartReorderMessage partReorderMessage = new PartReorderMessage();
                partReorderMessage.MessageType = (MessageType)Enum.Parse(typeof(MessageType), Convert.ToString(this.comboMessageTypePartReorder.Text), true);
                partReorderMessage.RouteColor = (RouteColor)Enum.Parse(typeof(RouteColor), Convert.ToString(this.comboColorPartReorder.Text), true);
                partReorderMessage.PartNumber = this.txtPartNumberPartReorder.Text.Trim();
                partReorderMessage.Timestamp = DateTime.Parse(this.txtTimestampPartReorder.Text.Trim());
                partReorderMessage.UID = int.Parse(this.txtUniqueIdPartReorder.Text.Trim());

                messageToSend = this.SerializeObject<PartReorderMessage>(partReorderMessage);
            }

            if (!string.IsNullOrWhiteSpace(messageToSend))
            {
                MQMessageStatus messageStatus = _manager.WriteLocalQMsg(messageToSend, this.txtPutQueue.Text.Trim());

                if (messageStatus.Status)
                {
                    MessageBox.Show(messageStatus.Message, "Information");
                    //Clear the existing data from the fields
                    this.ClearFields();
                    //Increment unique id on each message send
                    this._uniqueId = this._uniqueId + 1;
                    this.txtUniqueIdPartDelivered.Text = this._uniqueId.ToString();
                    this.txtUniqueIdPartReorder.Text = this._uniqueId.ToString();
                    //this.btnRetrieveMessage.IsEnabled = true;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(messageStatus.Message))
                    {
                        MessageBox.Show(messageStatus.Message, "Error");
                    }
                    else
                    {
                        MessageBox.Show("Some error occured while queuing the message", "Error");
                    }
                }
            }
            else
            {
                MessageBox.Show("No message available to queue", "Info");
            }
        }

        private void ClearFields()
        {
            this.txtPartNumberPartDelivered.Text = string.Empty;
            this.txtUniqueIdPartDelivered.Text = string.Empty;
            this.txtPartNumberPartReorder.Text = string.Empty;
            this.txtUniqueIdPartReorder.Text = string.Empty;
            this.comboMessageTypePartDelivered.SelectedIndex = 0;
            this.comboColorPartDelivered.SelectedIndex = 0;
            this.comboMessageTypePartReorder.SelectedIndex = 0;
            this.comboColorPartReorder.SelectedIndex = 0;
            this.txtTimestampPartDelivered.Text = DateTime.Now.ToString();
            this.txtTimestampPartReorder.Text = DateTime.Now.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtQueueManager.Text = _strQueueManagerName;
            this.txtChannelInfo.Text = _strChannelInfo;
            this.txtPutQueue.Text = _putQueueName;
            this.txtGetQueue.Text = _getQueueName;
            this.radioPartDelievered.IsChecked = true;
            this.txtTimestampPartDelivered.Text = DateTime.Now.ToString();
            this.txtTimestampPartReorder.Text = DateTime.Now.ToString();
            this.txtUniqueIdPartDelivered.Text = this._uniqueId.ToString();
            this.txtUniqueIdPartReorder.Text = this._uniqueId.ToString();
            this.btnSendMessage.IsEnabled = false;
            this.btnRetrieveMessage.IsEnabled = false;
        }

        private void radioPartDelievered_Checked(object sender, RoutedEventArgs e)
        {
            if (this.radioPartDelievered.IsChecked.HasValue && this.radioPartDelievered.IsChecked.Value)
            {
                if (this.partDeliveredGrid != null)
                {
                    this.partDeliveredGrid.Visibility = Visibility.Visible;
                }
                if (this.partReorderGrid != null)
                {
                    this.partReorderGrid.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (this.partDeliveredGrid != null)
                {
                    this.partDeliveredGrid.Visibility = Visibility.Collapsed;
                }
                if (this.partReorderGrid != null)
                {
                    this.partReorderGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void radioPartReorder_Checked(object sender, RoutedEventArgs e)
        {
            if (this.radioPartReorder.IsChecked.HasValue && this.radioPartReorder.IsChecked.Value)
            {
                if (this.partReorderGrid != null)
                {
                    this.partReorderGrid.Visibility = Visibility.Visible;
                }
                if (this.partDeliveredGrid != null)
                {
                    this.partDeliveredGrid.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (this.partReorderGrid != null)
                {
                    this.partReorderGrid.Visibility = Visibility.Collapsed;
                }
                if (this.partDeliveredGrid != null)
                {
                    this.partDeliveredGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            MQMessageStatus connectStatus = _manager.ConnectMQ(this.txtQueueManager.Text.Trim(), this.txtChannelInfo.Text.Trim());

            if (connectStatus.Status)
            {
                MessageBox.Show(connectStatus.Message, "Information");
                this.btnSendMessage.IsEnabled = true;
                this.btnRetrieveMessage.IsEnabled = true;
                this._isMQConnected = true;
            }
            else
            {
                this._isMQConnected = false;
                this.btnSendMessage.IsEnabled = false;
                this.btnRetrieveMessage.IsEnabled = false;

                if (!string.IsNullOrWhiteSpace(connectStatus.Message))
                {
                    MessageBox.Show(connectStatus.Message, "Error");
                }
                else
                {
                    MessageBox.Show("Some error occured while establishing the connection", "Error");
                }
            }
        }

        private void btnRetrieveMessage_Click(object sender, RoutedEventArgs e)
        {
            MQMessageStatus messageStatus = _manager.ReadLocalQMsg(this.txtGetQueue.Text.Trim());

            if (messageStatus.Status)
            {
                this.QueueDataList.Add(new QueueData { Data = messageStatus.Message });
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(messageStatus.Message))
                {
                    MessageBox.Show(messageStatus.Message, "Error");
                }
                else
                {
                    MessageBox.Show("Some error occured while getting the message from the queue", "Error");
                }
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.dgridData.SelectedItem != null)
            {
                QueueData data = this.dgridData.SelectedItem as QueueData;
                DataView view = new DataView(data.Data) { Owner = this, ShowInTaskbar = false, Topmost = true };

                this.Opacity = 0.5;
                this.Effect = new BlurEffect();

                view.ShowDialog();

                this.Opacity = 1;
                this.Effect = null;
            }
        }

        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}