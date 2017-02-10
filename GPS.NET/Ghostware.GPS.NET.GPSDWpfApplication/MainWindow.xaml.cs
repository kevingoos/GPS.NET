using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Ghostware.GPS.NET.Models.ConnectionInfo;
using Ghostware.GPS.NET.Models.Events;

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
            _gpsdService = new GpsService(info);
            _writer = new StreamWriter("testFile1.nmea");

            _gpsdService.RegisterRawDataEvent(GpsdServiceOnRawLocationChanged);
            _gpsdService.RegisterDataEvent(GpsdServiceOnLocationChanged);

            var task = Task.Run(() =>
            {
                Retry.Do(_gpsdService.Connect, TimeSpan.FromSeconds(1));
            });
            task.ContinueWith(t =>
            {
                MessageBox.Show(t.Exception?.ToString(), "Exception", MessageBoxButton.OK);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
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
