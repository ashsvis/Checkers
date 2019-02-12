using System.Net.PeerToPeer;

namespace Checkers.Net
{
    public enum PeerState
    {
        Unknown,
        NotFound,
        User
    }

    public class PeerEntry
    {
        public PeerName PeerName { get; set; }
        public IP2PService ServiceProxy { get; set; }
        public string DisplayString { get; set; }
        public bool ButtonsEnabled { get; set; }
        public PeerState State { get; set; }

        public override string ToString()
        {
            return DisplayString;
        }
    }
}
