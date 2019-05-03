using SimpleCS.Entity;
using SimpleCS.Entity.Config;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleCS
{
    public class SimpleServer
    {
        public string Name { get; private set; }

        public ServerStatus Status { get; private set; }

        public ServerHandler Handler { get; private set; }
        public ServerConfig Config { get; private set; }

        public SimpleServer(ServerHandler Handler, ServerConfig Config = null)
        {
            if (this.Status == ServerStatus.Launched)
                throw new Exception("This server is already running!");

            this.Name = "default";
            this.Handler = Handler;
            this.Status = ServerStatus.Stopped;
            if (Config != null)
                this.Configure(Config);
        }

        public SimpleServer(string Name, ServerHandler Handler, ServerConfig Config = null)
        {
            if (this.Status == ServerStatus.Launched)
                throw new Exception("This server is already running!");

            this.Name = Name;
            this.Handler = Handler;
            this.Status = ServerStatus.Stopped;
            if (Config != null)
                this.Configure(Config);
        }

        public void Configure(ServerConfig Config)
        {
            if (this.Status == ServerStatus.Launched)
                throw new Exception("This server is already running!");

            this.Config = Config;
            this.Status = ServerStatus.Configured;
        }

        public async void Start()
        {
            if (this.Status == ServerStatus.Launched)
                throw new Exception("This server is already running!");
            if (this.Status != ServerStatus.Configured)
                throw new Exception("This server is not configured!");

            this.Status = ServerStatus.Launched;

            TcpListener Listener = TcpListener.Create(this.Config.Port);
            Listener.Start();
            while (this.Status == ServerStatus.Launched)
            {
                TcpClient TcpConnection = await Listener.AcceptTcpClientAsync();
                _ = this.ProccessTcpConnection(TcpConnection);
            }
        }

        public void Stop()
        {
            if (this.Status != ServerStatus.Launched)
                throw new Exception("This server is not running!");

            this.Status = ServerStatus.Stopped;
        }

        private async Task ProccessTcpConnection(TcpClient TcpConnection)
        {
            using (ServerConnection CurrentServerConnection = new ServerConnection(TcpConnection, this))
                await CurrentServerConnection.ProcessAsync();
        }
    }

    public enum ServerStatus
    {
        Stopped,
        Configured,
        Launched
    }
}
