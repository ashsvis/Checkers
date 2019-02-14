using System;
using System.Text;

namespace Checkers.Net
{
    [Serializable]
    public struct P2PData
    {
        public P2PData(Player player, Guid id, string step, int blackScore, int whiteScore)
        {
            Player = player;
            PlayerId = id;
            Step = step;
            BlackScore = blackScore;
            WhiteScore = whiteScore;
            Map = "";
        }

        public Player Player { get; set; }
        public Guid PlayerId { get; set; }
        public string Step { get; set; }
        public int BlackScore { get; set; }
        public int WhiteScore { get; set; }
        public string Map { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Player=" + Player);
            sb.AppendLine("PlayerId=" + PlayerId);
            sb.AppendLine("Step=" + Step);
            sb.AppendLine("BlackScore=" + BlackScore);
            sb.AppendLine("WhiteScore=" + WhiteScore);
            sb.AppendLine("Map=" + Map);
            return sb.ToString();
        }
    }
}
