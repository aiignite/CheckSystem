using System.Net;

namespace CommonUtility
{
    /// <summary>
    /// Socket基类(抽象类)
    /// </summary>
    public abstract class SocketObject
    {
        public abstract void InitSocket(IPAddress ipaddress, int port);
        public abstract void InitSocket(string ipaddress, int port);
        public abstract void Start();
        public abstract void Stop();
    }
}
