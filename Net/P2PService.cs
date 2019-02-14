using System;
using System.ServiceModel;

namespace Checkers.Net
{
    public interface IDisplayMessage
    {
        void DisplayMessage(P2PData message, string from);
        void DisplayConnect(Guid id, string from);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class P2PService : IP2PService
    {
        private IDisplayMessage hostReference;
        private string username;
        private Player player;
        private Guid id;

        public P2PService(IDisplayMessage hostReference, string username, Player player, Guid id)
        {
            this.hostReference = hostReference;
            this.username = username;
            this.player = player;
            this.id = id;
        }

        public string GetName()
        {
            return username;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public Guid GetPlayerId()
        {
            return id;
        }

        public void SendConnect(Guid id, string from)
        {
            hostReference.DisplayConnect(id, from);
        }

        public void SendMessage(P2PData message, string from)
        {
            hostReference.DisplayMessage(message, from);
        }
    }
}
