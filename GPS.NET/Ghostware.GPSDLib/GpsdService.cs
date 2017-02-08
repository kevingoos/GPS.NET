using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using Ghostware.GPSDLib.Exceptions;
using Ghostware.GPSDLib.Models;

namespace Ghostware.GPSDLib
{
    public class GpsdService : IDisposable
    {
        #region Private Properties

        private TcpClient _client;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;

        private GpsdDataParser _gpsdDataParser;

        private readonly string _serverAddress;
        private readonly int _serverPort;

        private bool _proxyEnabled;
        private string _proxyAddress;
        private int _proxyPort;
        private bool _proxyAuthenticationEnabled;
        private string _proxyUsername;
        private SecureString _proxyPassword;

        private GpsLocation _previousGpsLocation;
        
        private int _retryReadCount;

        #endregion

        #region Properties

        public bool IsRunning { get; set; }
        public int ReadFrequenty { get; set; } = 1000;
        public int RetryRead { get; set; } = 3;

        public GpsdOptions GpsOptions { get; set; }

        
        #endregion

        #region Events

        public delegate void VersionEventHandler(object source, GpsdVersion e);
        public delegate void LocationEventHandler(object source, GpsLocation e);
        public delegate void RawLocationEventHandler(object source, string rawLocation);
        public event VersionEventHandler OnGpsdVersionChanged;
        public event LocationEventHandler OnLocationChanged;
        public event RawLocationEventHandler OnRawLocationChanged;

        #endregion

        #region Constructors

        public GpsdService(string serverAddress, int serverPort)
        {
            _serverAddress = serverAddress;
            _serverPort = serverPort;
            GpsOptions = GpsdConstants.DefaultGpsdOptions;
            IsRunning = true;
        }

        public GpsdService(string serverAddress, int serverPort, GpsdOptions gpsOptions = null) : this(serverAddress, serverPort)
        {
            GpsOptions = gpsOptions ?? GpsdConstants.DefaultGpsdOptions;
        }

        #endregion

        #region Connection Functionality

        public bool Connect()
        {
            _client = _proxyEnabled ? ConnectViaHttpProxy() : new TcpClient(_serverAddress, _serverPort);
            _streamReader = new StreamReader(_client.GetStream());
            _streamWriter = new StreamWriter(_client.GetStream());

            _gpsdDataParser = new GpsdDataParser();

            var gpsData = _streamReader.ReadLine();
            var message = _gpsdDataParser.GetGpsData(gpsData);
            var version = message as GpsdVersion;
            if (version == null) return false;
            OnGpsdVersionChanged?.Invoke(this, version);
            ExecuteGpsdCommand(GpsOptions.GetCommand());
            return true;
        }

        public bool Disconnect()
        {
            StopGpsReading();
            Dispose();
            return true;
        }

        #endregion

        #region Gps Reading Functionality

        /// <summary>
        /// This task reads the gps. This task will run in a loop, so keep in mind to run it in a thread.
        /// </summary>
        /// <exception cref="NotConnectedException">Exception when client is not connected or streamreader is null. Plz call connect!</exception>
        /// <exception cref="ConnectionLostException">Exception when the connection is lost.</exception>
        public void StartGpsReading()
        {
            if (_streamReader == null || !_client.Connected) throw new NotConnectedException();

            _retryReadCount = RetryRead;
            IsRunning = true;
            while (IsRunning)
            {
                if (!_client.Connected)
                {
                    throw new ConnectionLostException();
                }

                try
                {
                    var gpsData = _streamReader.ReadLine();
                    OnRawLocationChanged?.Invoke(this, gpsData);
                    if (gpsData == null)
                    {
                        if (_retryReadCount == 0)
                        {
                            Disconnect();
                            throw new ConnectionLostException();
                        }
                        _retryReadCount--;
                        continue;
                    }

                    var message = _gpsdDataParser.GetGpsData(gpsData);
                    var gpsLocation = message as GpsLocation;
                    if (gpsLocation == null ||
                        (_previousGpsLocation != null &&
                         gpsLocation.Time.Subtract(new TimeSpan(0, 0, 0, 0, ReadFrequenty)) <= _previousGpsLocation.Time))
                        continue;
                    OnLocationChanged?.Invoke(this, gpsLocation);
                    _previousGpsLocation = gpsLocation;
                }
                catch (IOException)
                {
                    Disconnect();
                    throw;
                }
            }
        }

        public Task StartGpsReadingAsync()
        {
            return new Task(StartGpsReading);
        }


        public void StopGpsReading()
        {
            if (!IsRunning) return;
            IsRunning = false;
            ExecuteGpsdCommand(GpsdConstants.DisableCommand);
        }

        #endregion

        #region Helper Functions

        private async void ExecuteGpsdCommand(string command)
        {
            if (_streamWriter == null) return;
            await _streamWriter.WriteLineAsync(command);
            await _streamWriter.FlushAsync();
        }

        #endregion

        #region Proxies

        public void SetProxy(string proxyAddress, int proxyPort)
        {
            _proxyEnabled = true;
            _proxyAddress = proxyAddress;
            _proxyPort = proxyPort;
        }

        public void SetProxyAuthentication(string username, string password)
        {
            _proxyAuthenticationEnabled = true;
            _proxyUsername = username;

            var securePass = new SecureString();
            foreach (var passwordChar in password)
            {
                securePass.AppendChar(passwordChar);
            }
            _proxyPassword = securePass;
        }

        public void SetProxyAuthentication(string username, SecureString password)
        {
            _proxyAuthenticationEnabled = true;
            _proxyUsername = username;
            _proxyPassword = password;
        }

        public void DisableProxy()
        {
            _proxyEnabled = false;
        }

        private TcpClient ConnectViaHttpProxy()
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttp,
                Host = _proxyAddress,
                Port = _proxyPort
            };

            var proxyUri = uriBuilder.Uri;
            var request = WebRequest.Create("http://" + _serverAddress + ":" + _serverPort);
            var webProxy = new WebProxy(proxyUri);

            request.Proxy = webProxy;
            request.Method = "CONNECT";

            if (_proxyAuthenticationEnabled)
            {
                webProxy.Credentials = new NetworkCredential(_proxyUsername, _proxyPassword);
            }
            else
            {
                webProxy.UseDefaultCredentials = true;
            }

            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();
            Debug.Assert(responseStream != null);

            const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

            var rsType = responseStream.GetType();
            var connectionProperty = rsType.GetProperty("Connection", flags);

            var connection = connectionProperty.GetValue(responseStream, null);
            var connectionType = connection.GetType();
            var networkStreamProperty = connectionType.GetProperty("NetworkStream", flags);

            var networkStream = networkStreamProperty.GetValue(connection, null);
            var nsType = networkStream.GetType();
            var socketProperty = nsType.GetProperty("Socket", flags);
            var socket = (Socket)socketProperty.GetValue(networkStream, null);

            return new TcpClient { Client = socket };
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            _streamReader?.Close();
            _streamWriter?.Close();
            _client?.Close();
        }

        #endregion
    }
}
