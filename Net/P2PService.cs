using System.ServiceModel;

namespace Checkers.Net
{
    public interface IDisplayMessage
    {
        void DisplayMessage(string message, string from);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class P2PService : IP2PService
    {
        private IDisplayMessage hostReference;
        private string username;

        public P2PService(IDisplayMessage hostReference, string username)
        {
            this.hostReference = hostReference;
            this.username = username;
        }

        public string GetName()
        {
            return username;
        }

        public void SendMessage(string message, string from)
        {
            hostReference.DisplayMessage(message, from);
        }
    }
}
