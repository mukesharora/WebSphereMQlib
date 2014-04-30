using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WebSphereLib.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (e.Exception != null)
                {
                    MessageBox.Show(string.Format("An unhandled error occurred : {0}", e.Exception.Message), "Error");
                }
                else
                {
                    MessageBox.Show("An unhandled error occurred", "Error");
                }

            }), System.Windows.Threading.DispatcherPriority.Normal);
        }
    }
}
