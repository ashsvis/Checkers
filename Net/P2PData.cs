using System;

namespace Checkers.Net
{
    [Serializable]
    public struct P2PData
    {
        public P2PData(Player player, string step, int blackScore, int whiteScore)
        {
            Player = player;
            Step = step;
            BlackScore = blackScore;
            WhiteScore = whiteScore;
            Map = "";
        }

        public Player Player { get; set; }
        public string Step { get; set; }
        public int BlackScore { get; set; }
        public int WhiteScore { get; set; }
        public string Map { get; set; }
    }
}
