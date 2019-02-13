using System.ServiceModel;

namespace Checkers.Net
{
    public interface IDisplayMessage
    {
        void DisplayMessage(P2PData message, string from);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class P2PService : IP2PService
    {
        private IDisplayMessage hostReference;
        private string username;
        private Player player;

        public P2PService(IDisplayMessage hostReference, string username, Player player)
        {
            this.hostReference = hostReference;
            this.username = username;
            this.player = player;
        }

        public string GetName()
        {
            return username;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public void SendMessage(P2PData message, string from)
        {
            hostReference.DisplayMessage(message, from);
        }
    }
}
