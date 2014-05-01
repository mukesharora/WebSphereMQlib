//-----------------------------------------------------------------------
//
// <copyright file="DataView.xaml.cs" company="Omni-ID, Ltd.">
//
// Copyright (c) 2013, 2014 - Omni-ID, Ltd. All rights reserved.
//
// <author>Omni-ID</author>
//
// </copyright>
//
//-----------------------------------------------------------------------

using System.Windows;

namespace WebSphereLib.App
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : Window
    {
        public DataView()
        {
            InitializeComponent();
        }

        public DataView(string data)
        {
            InitializeComponent();
            this.txtData.Text = data;
            this.DataContext = this;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
