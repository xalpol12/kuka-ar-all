using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Project.Scripts.EventSystem.Enums;

namespace Project.Scripts.Connectivity.Http.Requests
{
    public class PingChosenIpRequest : IHttpRequest<bool>
    {
        private readonly WebDataStorage storage = WebDataStorage.Instance;
        private readonly string ip;

        public PingChosenIpRequest(string ip)
        {
            this.ip = ip;
        }

        public async Task<bool> Execute(HttpClient client)
        {
            using var tcpClient = new TcpClient();
            var connectTask = tcpClient.ConnectAsync(ip, 8080);
            var timeoutTask = Task.Delay(storage.ConnectionTimeOut);
                
            var completedTask = await Task.WhenAny(connectTask, timeoutTask);
            try
            {
                await completedTask;
            }
            catch (Exception)
            {
                return false;
            }

            if (timeoutTask.IsCompleted)
            {
                return false;
            }

            return connectTask.Status == TaskStatus.RanToCompletion && tcpClient.Connected;
            // var ping = new Ping(Ip);
            // var time = 0;
            // while (!ping.isDone)
            // {
            //     if (time > WebDataStorage.ConnectionTimeOutSel)
            //     {
            //         break;
            //     }
            //
            //     time -= ping.time;
            //     storage.RobotConnectionStatus = ConnectionStatus.Connecting;
            // }
        
            // storage.RobotConnectionStatus = time > WebDataStorage.ConnectionTimeOut ?
                // ConnectionStatus.Disconnected : ConnectionStatus.Connected;
            // return storage.RobotConnectionStatus;
        }

        private void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                storage.RobotConnectionStatus = ConnectionStatus.Disconnected;
            }
            else
            {
                storage.RobotConnectionStatus = ConnectionStatus.Connected;
            }
            
            ((AutoResetEvent)e.UserState).Set();
        }

        private void PingCompleted()
        {
                
        }
    }
}