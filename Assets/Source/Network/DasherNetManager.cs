using Mirror;

namespace Dasher
{
    internal class DasherNetManager : NetworkManager
    {
        public void SetHostName(string hostname)
        {
            networkAddress = hostname;
        }
    }
}