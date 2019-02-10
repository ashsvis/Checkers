using System;
using System.Collections.Generic;

namespace Checkers
{
    public enum Player
    {
        White,
        Black
    }

    public enum WinPlayer
    {
        None,
        White,
        Black,
        Draw
    }

    [Serializable]
    public class Game
    {
        public List<LogItem> Log { get; set; }

        public int WhiteScore { get; set; }
        public int BlackScore { get; set; }

        public bool Direction { get; set; }

        public Player Player { get; set; }

        public WinPlayer WinPlayer { get; set; }

        public Game()
        {
            Log = new List<LogItem>();
        }

        public void CheckWin()
        {
            WinPlayer = WhiteScore == 12
                ? WinPlayer.White
                : BlackScore == 12 ? WinPlayer.Black : WinPlayer.None;
        }
    }
}
