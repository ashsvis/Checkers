using System;
using System.Collections;
using System.Collections.Generic;

namespace Checkers
{
    public enum Player
    {
        White,
        Black
    }

    [Serializable]
    public class Game
    {
        public List<LogItem> Log { get; set; }

        public int WhiteScore { get; set; }
        public int BlackScore { get; set; }

        public bool Direction { get; set; }

        public Player Player { get; set; }

        public Game()
        {
            Log = new List<LogItem>();
        }
    }
}
