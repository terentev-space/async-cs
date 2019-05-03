namespace SimpleCS.Entity.Config
{
    public class ClientConfig
    {
        public string ServerAddress = "localhost";
        public ushort ServerPort = 54154;
        public uint Timeout = 10000;//ms
        public ushort Package = 4096;
    }
}
