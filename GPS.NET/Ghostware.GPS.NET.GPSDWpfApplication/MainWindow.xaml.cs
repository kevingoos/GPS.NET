using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ghostware.GPS.NET.Enums;
using Ghostware.GPS.NET.Models.ConnectionData;
using Ghostware.GPS.NET.Models.Events;
using Ghostware.GPS.NET.Models.GpsdModels;

namespace Ghostware.GPS.NET.GPSDWpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static StreamWriter _writer;
        private static GpsService _gpsdService;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_gpsdService != null && _gpsdService.IsRunning) return;

            //_gpsdService = new GpsdService("***.***.***.***", 80);
            _gpsdService = new GpsService(GpsType.Gpsd);

            //_gpsdService.SetProxy("proxy", 80);
            //_gpsdService.SetProxyAuthentication("*****", "*****");

            _writer = new StreamWriter("testFile1.nmea");

            _gpsdService.RegisterRawDataEvent(GpsdServiceOnRawLocationChanged);
            _gpsdService.RegisterDataEvent(GpsdServiceOnLocationChanged);

            var task = Task.Run(() =>
            {
                Retry.Do(ConnectToGpsd, TimeSpan.FromSeconds(1));
            });
            task.ContinueWith(t =>
            {
                MessageBox.Show(t.Exception?.ToString(), "Exception", MessageBoxButton.OK);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public bool ConnectToGpsd()
        {
            var info = new GpsdInfo()
            {
                Address = "***.* **.* **.***",
                //Default
                //Port = 2947,
                //IsProxyEnabled = true,
                //ProxyAddress = "proxy",
                //ProxyPort = 80,
                //IsProxyAuthManual = true,
                //ProxyUsername = "*****",
                //ProxyPassword = "*****"
            };

            return _gpsdService.Connect(info);
        }

        private void GpsdServiceOnLocationChanged(object source, GpsDataEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (GpsTextBox == null) return;
                GpsTextBox.Text = GpsTextBox.Text + e + "\n";
            });

            Console.WriteLine(e.ToString());
        }

        private void GpsdServiceOnRawLocationChanged(object source, string rawData)
        {
            _writer.WriteLine(rawData);
        }


        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _gpsdService?.Disconnect();
        }
    }
}
