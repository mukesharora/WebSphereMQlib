//-----------------------------------------------------------------------
//
// <copyright file="QueueData.cs" company="Omni-ID, Ltd.">
//
// Copyright (c) 2013, 2014 - Omni-ID, Ltd. All rights reserved.
//
// <author>Omni-ID</author>
//
// </copyright>
//
//-----------------------------------------------------------------------

using System.ComponentModel;

namespace WebSphereLib.App.Entity
{
    public class QueueData : INotifyPropertyChanged
    {
        #region Private Members

        private string _data = string.Empty;

        #endregion

        #region Properties

        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    this.OnPropertyChanged("Data");
                }
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
