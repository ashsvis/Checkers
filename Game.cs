using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Checkers
{
    public enum Player
    {
        None,
        Black,
        White
    }

    public class Game: IDisposable
    {
        private BackgroundWorker _worker;
        private Board _board;
        private Io _io;
        private bool _isSurrender;
        private Player _lastPlayer;
        private int _whiteScore;
        private int _blackScore;

        public Game(Board board, Io io)
        {
            _board = board;
            _io = io;
            //_lastPlayer = board.Player;
            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
        }

        public void Stop()
        {
            _worker.CancelAsync();
        }

        public void Start()
        {
            _worker.RunWorkerAsync();
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            lock (_board)
            {
                _board.ResetMap();
            }
            // Check is game over
            while (GetWinner() == Player.None && !worker.CancellationPending)
            {
                var moveResult = MoveResult.Prohibited;
                while (moveResult == MoveResult.Prohibited && !worker.CancellationPending)
                {
                    // IO stuff
                    lock (_board)
                    {
                        var startPos = _board.Selected != null ? _board.Selected.Address : new Address();
                        var endPos = _board.Goal != null ? _board.Goal.Address : new Address();
                        if (!startPos.IsEmpty() && !endPos.IsEmpty())
                        {
                            _board.Direction = GetDirection();
                            Console.WriteLine(_board.Direction);
                            moveResult = _board.MakeMove(startPos, endPos);
                            _board.Selected = null;
                            _board.Goal = null;
                        }
                    }
                }
                // Parse Move Result
                if (moveResult == MoveResult.SuccessfullCombat)
                    // update score
                    UpdateScore();
                // update last player
                SwitchPlayer();
            }
        }

        private void SwitchPlayer()
        {
            if (_lastPlayer == Player.White)
                _lastPlayer = Player.Black;
            else
                _lastPlayer = Player.White;
        }

        private void UpdateScore()
        {
            if (_lastPlayer == Player.White)
                _blackScore++;
            else
                _whiteScore++;
        }

        private bool GetDirection()
        {
            bool direction;
            switch (_lastPlayer)
            {
                case Player.White:
                    direction = false;
                    break;
                default:
                    direction = true;
                    break;
            }
            return direction;
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _io.EndGame(CastPlayer(GetWinner()));
        }

        private string CastPlayer(Player player)
        {
            string playerStr;
            switch (player)
            {
                case Player.White:
                    playerStr = "White player";
                    break;
                default:
                    playerStr = "Black player";
                    break;
            }
            return playerStr;
        }

        private Player GetWinner()
        {
            var winner = Player.None;

            if (_isSurrender)
            {
                if (_lastPlayer == Player.White)
                    winner = Player.Black;
                else if (_lastPlayer == Player.Black)
                    winner = Player.White;
            }
            else
            {
                if (_whiteScore == 12)
                    winner = Player.White;
                else if (_blackScore == 12)
                    winner = Player.Black;
            }
            return winner;
        }

        ~Game()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_worker != null) _worker.Dispose();
        }
    }
}
