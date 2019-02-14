using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Checkers.Net;

namespace Checkers
{
    public partial class CheckersForm : Form
    {
        private Io _io;
        private Board _board;
        private Game _game;
        private NetGame _net;

        public CheckersForm()
        {
            InitializeComponent();
            mainStatus.SizingGrip = false;
            
            DoubleBuffered = true;
            _game = new Game() { Mode = PlayMode.SelfGame };
            _board = new Board(_game);
            _board.UpdateStatus += () => UpdateStatus();
            _board.ShowError += _board_ShowError;
            _board.AskQuestion += _board_AskQuestion;
            _board.ActivePlayerChanged += _board_ActivePlayerChanged;
            _board.CheckerMoved += _board_CheckerMoved;
            _net = new NetGame(_game, _board);
            _net.DisplayPeerMessage += _net_DisplayPeerMessage;
            _net.CaptionChanged += _net_CaptionChanged;
           _io = new Io(_game, _board, new Size(0, mainMenu.Height + mainTools.Height));
        }

        private void _net_CaptionChanged()
        {
            UpdateCaptionText(_net.Caption);
        }

        private bool _board_AskQuestion(string text, string caption)
        {
            return MessageBox.Show(this, text, caption, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        private void _board_ShowError(string text, string caption)
        {
            //var status = string.Format(_game.Direction
            //                            ? "Ход чёрных ({0})..." 
            //                            : "Ход белых ({0})...", text.ToLower().TrimEnd('!'));
            UpdateStatusText(_game.ToString());
            //MessageBox.Show(this, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void _board_CheckerMoved(bool direction, Address startPos, Address endPos, MoveResult moveResult, int stepCount)
        {
            UpdateLog(direction, startPos, endPos, moveResult, stepCount);
            tsmiSaveGame.Enabled = tsbSaveGame.Enabled = true;
        }

        private void UpdateLog(bool direction, Address startPos, Address endPos, MoveResult moveResult, int stepCount)
        {
            var result = string.Format("{0}{1}{2}",
                    startPos, moveResult == MoveResult.SuccessfullCombat ? ":" : "-", endPos);
            if (!direction)
            {
                // ходят "белые"
                if (stepCount == 1) // первый ход (из, возможно, серии ходов)
                {
                    var item = new LogItem() { Number = _game.Log.Count + 1, White = result };
                    item.AddToMap(_board.GetMap().DeepClone());
                    _game.Log.Add(item);
                    lvLog.VirtualListSize = _game.Log.Count;
                    var lvi = lvLog.Items[lvLog.Items.Count - 1];
                    lvLog.FocusedItem = lvi;
                    lvi.EnsureVisible();
                    lvLog.Invalidate();
                }
                else
                {
                    var item = _game.Log[_game.Log.Count - 1];
                    item.White += ":" + endPos;
                    item.AddToMap(_board.GetMap().DeepClone());
                    lvLog.Invalidate();
                }
            }
            else
            {
                // ходят "чёрные"
                var item = _game.Log[_game.Log.Count - 1];
                if (stepCount == 1) // первый ход (из, возможно, серии ходов)
                    item.Black = result;
                else
                    item.Black += ":" + endPos;
                item.AddToMap(_board.GetMap().DeepClone());
                lvLog.Invalidate();
            }
        }

        private void _board_ActivePlayerChanged()
        {
            UpdateStatus();
            _net.SendNetGameStatus();
        }

        private void _net_DisplayPeerMessage(P2PData message, string from)
        {
            // Показать полученное сообщение (вызывается из службы WCF)
            _board.SetMap(message.Map);
            var method = new MethodInvoker(() =>
            {
                Invalidate();
                _game.Direction = message.Player == Player.Black;
                _game.BlackScore = message.BlackScore;
                _game.WhiteScore = message.WhiteScore;
                UpdateStatusText(message.Step);
                UpdateStatus();
                if (message.Player == Player.Black)
                {
                    var item = new LogItem() { Number = _game.Log.Count + 1, White = message.Step };
                    item.AddToMap(_board.GetMap().DeepClone());
                    _game.Log.Add(item);
                    lvLog.VirtualListSize = _game.Log.Count;
                    var lvi = lvLog.Items[lvLog.Items.Count - 1];
                    lvLog.FocusedItem = lvi;
                    lvi.EnsureVisible();
                    lvLog.Invalidate();
                }
                else
                {
                    var item = _game.Log[_game.Log.Count - 1];
                    item.Black = message.Step;
                    item.AddToMap(_board.GetMap().DeepClone());
                    lvLog.Invalidate();
                }
            });
            if (InvokeRequired)
                BeginInvoke(method);
            else
                method();

        }

        private void CheckersForm_Load(object sender, EventArgs e)
        {
            // Конфигурирование игры
            _board.ResetMap();
            var size = _io.GetDrawBoardSize();
            size.Width += panelLog.Width;
            size.Height += mainMenu.Height + mainTools.Height + mainStatus.Height;
            ClientSize = size;
            UpdateStatus();
            CenterToScreen();
        }

        private void UpdateStatus()
        {
            lbWhiteScore.Text = string.Format("Белые: {0}", _game.WhiteScore);
            lbBlackScore.Text = string.Format("Чёрные: {0}", _game.BlackScore);
            UpdateCaptionText(_net.Caption);
            UpdateStatusText(_game.ToString());
        }

        private void UpdateStatusText(string text)
        {
            var method = new MethodInvoker(() =>
            {
                status.Text = text;
                mainStatus.Refresh();
            });
            if (InvokeRequired)
                BeginInvoke(method);
            else
                method();
        }

        private void UpdateCaptionText(string text)
        {
            var method = new MethodInvoker(() =>
            {
                Text = text;
            });
            if (InvokeRequired)
                BeginInvoke(method);
            else
                method();
        }

        private void CheckersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void CheckersForm_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            // рисуем доску с шашками
            _io.DrawBoard(graphics);
        }

        private void CheckersForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var n = lvLog.SelectedIndices.Count > 0 ? lvLog.SelectedIndices[0] : -1;
                if (n < 0 || n == _game.Log.Count - 1)
                {
                    _io.MouseDown(e.Location);
                    Invalidate();
                }
                else
                    if (_board_AskQuestion("Продолжить игру?", "Шашки"))
                {
                    var item = lvLog.Items[_game.Log.Count - 1];
                    item.Selected = true;
                }
            }
        }

        private void CheckersForm_MouseMove(object sender, MouseEventArgs e)
        {
            _io.MouseMove(e.Location);
            Invalidate();
        }

        private void CheckersForm_MouseUp(object sender, MouseEventArgs e)
        {
            _io.MouseUp(e.Location);
            Invalidate();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Новая игра (сброс доски)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiNewGame_Click(object sender, EventArgs e)
        {
            _game.WinPlayer = WinPlayer.None;
            _board.ResetMap();
            Invalidate();
            _game.Log.Clear();
            _game.WhiteScore = 0;
            _game.BlackScore = 0;
            lvLog.VirtualListSize = _game.Log.Count;
            lvLog.Invalidate();
            lbWhiteScore.Text = string.Format("Белые: {0}", _game.WhiteScore);
            lbBlackScore.Text = string.Format("Чёрные: {0}", _game.BlackScore);
            UpdateStatusText(_game.ToString());
        }

        private void lvLog_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = new ListViewItem();
            var item = _game.Log[e.ItemIndex];
            e.Item.Text = item.Number.ToString();
            e.Item.SubItems.Add(item.White);
            e.Item.SubItems.Add(item.Black);
        }

        private void lvLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvLog.SelectedIndices.Count == 0) return;
            var n = lvLog.SelectedIndices[0];
            _board.Selected = null;

            var semiSteps = _game.Log[n].GetMapSemiSteps();
            foreach (var item in semiSteps)
            {
                var map = item.DeepClone();
                _board.SetMap(map);
                Refresh();
                Thread.Sleep(200);
            }

            if (n == _game.Log.Count - 1)
            {
                lvLog.SelectedIndices.Clear();
                UpdateStatus();
            }
            else
                UpdateStatusText(string.Format("Положение фигур после {0}-го хода.", n + 1));
        }

        private void tsmiTunings_Click(object sender, EventArgs e)
        {
            //
        }

        private void tsmiGameMode_Click(object sender, EventArgs e)
        {
            _game.Mode = PlayMode.Game; // игра
            panelLog.Visible = true;
            UpdateStatus();
            Invalidate();
        }

        private void tsmiCollocationMode_Click(object sender, EventArgs e)
        {
            _game.Mode = PlayMode.Collocation; // расстановка
            panelLog.Visible = false;
            UpdateStatusText("Режим расстановки шашек");
            Invalidate();
        }

        private void tsmiSaveGame_Click(object sender, EventArgs e)
        {
            SaveGame();
        }

        private void SaveGame()
        {
            if (string.IsNullOrWhiteSpace(saveGameDialog.FileName))
            {
                if (saveGameDialog.ShowDialog(this) == DialogResult.OK)
                    SaverLoader.SaveToFile(saveGameDialog.FileName, _game);
            }
            else
                SaverLoader.SaveToFile(saveGameDialog.FileName, _game);
            tsmiSaveGame.Enabled = tsbSaveGame.Enabled = false;
        }

        private void tsmiOpenGame_Click(object sender, EventArgs e)
        {
            if (openGameDialog.ShowDialog(this) == DialogResult.OK)
            {
                saveGameDialog.FileName = openGameDialog.FileName;
                _game = SaverLoader.LoadFromFile(openGameDialog.FileName);
                _board.SetGame(_game);
                _io.SetGame(_game);
                _net.SetGame(_game);
                Invalidate();
                UpdateStatus();
                lvLog.VirtualListSize = _game.Log.Count;
                lvLog.Invalidate();
            }
        }

        private void CheckersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tsmiSaveGame.Enabled)
            {
                if (MessageBox.Show(this, "Желаете сохранить игру перед выходом?", "Выход", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    SaveGame();
                }
            }
            NetStop();
        }

        private async void NetStop()
        {
            await _net.StopAsync();
        }

        private async void NetRestart()
        {
            await _net.RestartAsync(Properties.Settings.Default.P2PPort,
                       Properties.Settings.Default.P2PUserName, _game.Player,
                       Environment.MachineName);
        }

        private void tsmiTools_DropDownOpening(object sender, EventArgs e)
        {
            tsmiSelfGame.Checked = _game.WinPlayer == WinPlayer.Game && _game.Mode == PlayMode.SelfGame;
            tsmiAutoGame.Checked = _game.WinPlayer == WinPlayer.Game && _game.Mode == PlayMode.Game;
            tsmiNetGame.Checked = _game.WinPlayer == WinPlayer.Game && _game.Mode == PlayMode.NetGame;
            tsmiSelfGame.Enabled = tsmiAutoGame.Enabled = tsmiNetGame.Enabled = _game.WinPlayer != WinPlayer.Game;
        }

        private void tsmiSelfGame_Click(object sender, EventArgs e)
        {
            var last = _game.Mode;
            _game.Mode = PlayMode.SelfGame;
            if (last == PlayMode.NetGame)
                NetStop();
            _game.WinPlayer = WinPlayer.Game;
            UpdateStatus();
        }

        private void tsmiAutoGame_Click(object sender, EventArgs e)
        {
            var last = _game.Mode;
            _game.Mode = PlayMode.Game;
            if (last == PlayMode.NetGame)
                NetStop();
            _game.WinPlayer = WinPlayer.Game;
            UpdateStatus();
        }

        private void tsmiNetGame_Click(object sender, EventArgs e)
        {
            _game.WinPlayer = WinPlayer.None;
            _game.Mode = PlayMode.NetGame;
            GameTuning();
            UpdateStatus();
        }

        private void GameTuning()
        {
            var frm = new SelectNetEnemy(this, _net, _game);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                if (_net.Started && frm.Selected != null)
                {
                    // Получение пира и прокси, для отправки сообщения
                    if (frm.Selected.State == PeerState.User)
                    {
                        _net.Enemy = frm.Selected;
                        _net.SendConnect(_net.Enemy);
                        _game.Player = frm.Selected.Player == Player.White 
                            ? Player.Black 
                            : frm.Selected.Player == Player.Black 
                                 ? Player.White 
                                 : Player.Black;
                        _game.Mode = PlayMode.NetGame;
                        _game.WinPlayer = WinPlayer.Game;
                    }
                    UpdateCaptionText(_net.Caption);
                }
            }
            else
                NetStop();
        }

        private void tsmiCreateSelfNetGame_Click(object sender, EventArgs e)
        {
            var frm = new CreateNetGame(this, _net, _game);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                _game.WinPlayer = WinPlayer.None;
                _game.Player = frm.GetPlayer();
                _game.Mode = PlayMode.NetGame;
                Invalidate();
                UpdateCaptionText(_net.Caption);
                NetRestart();
            }
            else
                NetStop();
        }

        private void tsmiGame_DropDownOpening(object sender, EventArgs e)
        {
            tsmiOpenGame.Enabled = _game.WinPlayer != WinPlayer.Game;
        }
    }
}
