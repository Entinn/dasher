using Mirror;

namespace Dasher
{
    internal class DasherNetManager : NetworkManager
    {
        public static bool InGameNow => NetworkServer.active || NetworkClient.active;

        public void SetHostName(string hostname)
        {
            networkAddress = hostname;
        }
    }
}