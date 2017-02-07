using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Ghostware.GPSDLib;
using Ghostware.GPSDLib.Models;

namespace Ghostware.GPSDTestApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static StreamWriter _writer;
        private static GpsdService _gpsdService;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_gpsdService != null && _gpsdService.IsRunning) return;

            //_gpsdService = new GpsdService("***.***.***.***", 80);
            _gpsdService = new GpsdService("127.0.0.1", 80);

            //_gpsdService.SetProxy("proxy", 80);
            //_gpsdService.SetProxyAuthentication("*****", "*****");

            _writer = new StreamWriter("testFile1.nmea");

            _gpsdService.OnRawLocationChanged += GpsdServiceOnRawLocationChanged;
            _gpsdService.OnLocationChanged += GpsdServiceOnLocationChanged;

            var task = Task.Run(() =>
            {
                Retry.Do(_gpsdService.Connect, TimeSpan.FromSeconds(1));
            });
            task.ContinueWith(t =>
            {
                if (t.Exception != null) return;
                _gpsdService.StartGpsReading();
            });
            task.ContinueWith(t =>
            {
                MessageBox.Show(t.Exception?.ToString(), "Test", MessageBoxButton.OK);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void GpsdServiceOnLocationChanged(object source, GpsLocation e)
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
