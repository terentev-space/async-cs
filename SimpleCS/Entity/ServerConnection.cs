using SimpleCS.Helper;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleCS.Entity
{
    public class ServerConnection : IDisposable
    {
        private NetworkStream ConnectionNetworkStream;
        private SimpleServer Server;

        public ServerConnection(TcpClient TcpConnection, SimpleServer Server)
        {
            this.ConnectionNetworkStream = TcpConnection.GetStream();
            this.Server = Server;
        }

        public void Dispose()
        {
            this.ConnectionNetworkStream.Dispose();
        }

        public async Task ProcessAsync()
        {
            string Data = await this.ReadWithTimeoutAsync();
            Data = this.Server.Handler.ProcessRequest(Data);
            if (Data != null)
                await this.SendWithTimeoutAsync(Data);
        }

        private async Task<string> ReadWithTimeoutAsync()
        {
            using (var TokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(this.Server.Config.Timeout)))
            {
                CancellationToken Token = TokenSource.Token;
                try
                {
                    using (Token.Register(this.ConnectionNetworkStream.Close))
                    {
                        byte[] Bytes = await this.DynamicReadFromStreamAsync(TokenSource.Token);
                        return StringHelper.BytesToString(Bytes);
                    }
                }
                catch (ObjectDisposedException) when (Token.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
            }
        }

        private async Task<byte[]> DynamicReadFromStreamAsync(CancellationToken Token)
        {
            byte[] Bytes = new byte[this.Server.Config.Package];
            int BytesRead = await this.ConnectionNetworkStream.ReadAsync(Bytes, 0, Bytes.Length, Token);
            return Bytes.Take(BytesRead).ToArray();
        }

        private async Task SendWithTimeoutAsync(string Data)
        {
            using (var TokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(this.Server.Config.Timeout)))
            {
                CancellationToken Token = TokenSource.Token;
                try
                {
                    using (Token.Register(this.ConnectionNetworkStream.Close))
                    {
                        byte[] Bytes = StringHelper.StringToBytes(Data);
                        await this.DynamicSendFromStreamAsync(Bytes, TokenSource.Token);
                    }
                }
                catch (ObjectDisposedException) when (Token.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
            }
        }

        private async Task DynamicSendFromStreamAsync(byte[] Data, CancellationToken Token)
        {
            await this.ConnectionNetworkStream.WriteAsync(Data, 0, Data.Length, Token);
        }
    }
}
