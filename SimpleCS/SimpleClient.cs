using System.Net.Sockets;
using SimpleCS.Helper;
using SimpleCS.Entity.Config;

namespace SimpleCS
{
    public class SimpleClient
    {
        public ClientConfig Config { get; private set; }

        public SimpleClient(ClientConfig Config)
        {
            this.Config = Config;
        }

        public virtual string SendMessage(string Request)
        {
            try
            {
                TcpClient Client = new TcpClient(this.Config.ServerAddress, this.Config.ServerPort);

                byte[] Data = StringHelper.StringToBytes(Request);

                NetworkStream Stream = Client.GetStream();

                Stream.Write(Data, 0, Data.Length);

                Data = new byte[this.Config.Package];

                int BytesRead = Stream.Read(Data, 0, Data.Length);
                string Response = StringHelper.BytesToString(Data);

                Stream.Close();
                Client.Close();

                return Response;
            }
            catch
            {
                return null;
            }
        }
    }
}
