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
            _net = new NetGame();
            _net.ResolveProgressChanged += _net_ResolveProgressChanged;
            _net.ResolveCompleted += _net_ResolveCompleted;
            _net.DisplayPeerMessage += _net_DisplayPeerMessage;
            _game = new Game();
            _board = new Board(_game);
            _board.UpdateStatus += () => UpdateStatus();
            _board.ShowError += _board_ShowError;
            _board.AskQuestion += _board_AskQuestion;
            _board.ActivePlayerChanged += _io_ActivePlayerChanged;
            _board.CheckerMoved += _io_CheckerMoved;
            _io = new Io(_game, _board, new Size(0, mainMenu.Height + mainTools.Height));
        }

        private void _net_DisplayPeerMessage(string message, string from)
        {
            // Показать полученное сообщение (вызывается из службы WCF)
            //MessageBox.Show(this, message, string.Format("Сообщение от {0}", from),
            //    MessageBoxButtons.OK, MessageBoxIcon.Information);
            status.Text = message;
        }

        private void _net_ResolveCompleted()
        {
            UpdatePeerList();
            // Повторно включаем кнопку "обновить"
            btnRefreshPeers.Enabled = _net.CanRefreshPeers;
        }

        private void _net_ResolveProgressChanged()
        {
            UpdatePeerList();
        }

        private void UpdatePeerList()
        {
            lbPeerList.Items.Clear();
            foreach (var item in _net.PeerList)
            {
                if (item.ButtonsEnabled)
                    lbPeerList.Items.Add(item);
            }
        }

        private bool _board_AskQuestion(string text, string caption)
        {
            return MessageBox.Show(this, text, caption, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        private void _board_ShowError(string text, string caption)
        {
            status.Text = string.Format(_game.Direction 
                ? "Ход чёрных ({0})..." : "Ход белых ({0})...", 
                text.ToLower().TrimEnd('!'));
            //MessageBox.Show(this, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void _io_CheckerMoved(bool direction, Address startPos, Address endPos, MoveResult moveResult, int stepCount)
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
                    var value = _game.Log[_game.Log.Count - 1];
                    value.White += ":" + endPos;
                    value.AddToMap(_board.GetMap().DeepClone());
                    lvLog.Invalidate();
                }
            }
            else
            {
                // ходят "чёрные"
                var value = _game.Log[_game.Log.Count - 1];
                if (stepCount == 1) // первый ход (из, возможно, серии ходов)
                    value.Black = result;
                else
                    value.Black += ":" + endPos;
                value.AddToMap(_board.GetMap().DeepClone());
                lvLog.Invalidate();
            }
        }

        private void _io_ActivePlayerChanged()
        {
            UpdateStatus();
        }

        private void CheckersForm_Load(object sender, EventArgs e)
        {
            btnRefreshPeers.Enabled = _net.Start(Properties.Settings.Default.P2PPort, 
                       Properties.Settings.Default.P2PUserName,
                       Environment.MachineName);
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
            status.Text = _game.WinPlayer == WinPlayer.White
                ? "Белые выиграли" 
                : _game.WinPlayer == WinPlayer.Black 
                       ? "Чёрные выиграли" 
                       : _game.WinPlayer == WinPlayer.Draw 
                                ? "Ничья" 
                                : _game.Direction ? "Ход чёрных..." : "Ход белых...";
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

        /// <summary>
        /// Выбор стороны игрока (за белых или за чёрных)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPlayerWhite_Click(object sender, EventArgs e)
        {
            if (sender == tsmiWhiteSide)
                _game.Player = Player.White;
            else if (sender == tsmiBlackSide)
                _game.Player = Player.Black;
            Invalidate();
        }

        /// <summary>
        /// Отображение текущей стороны игрока
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiSelectSide_DropDownOpening(object sender, EventArgs e)
        {
            tsmiBlackSide.Checked = _game.Player == Player.Black;
            tsmiWhiteSide.Checked = _game.Player == Player.White;
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
            _board.ResetMap();
            Invalidate();
            _game.Log.Clear();
            _game.WhiteScore = 0;
            _game.BlackScore = 0;
            lvLog.VirtualListSize = _game.Log.Count;
            lvLog.Invalidate();
            lbWhiteScore.Text = string.Format("Белые: {0}", _game.WhiteScore);
            lbBlackScore.Text = string.Format("Чёрные: {0}", _game.BlackScore);
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

            //var map = _game.Log[n].GetLastMap().DeepClone();
            //_board.SetMap(map);
            //Invalidate();

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
                status.Text = string.Format("Положение фигур после {0}-го хода.", n + 1);
        }

        private void tsmiTunings_Click(object sender, EventArgs e)
        {
            //
        }

        private void tsmiApplicationMode_DropDownOpening(object sender, EventArgs e)
        {
            tsmiGameMode.Checked = _board.Mode == PlayMode.Game;
            tsmiCollocationMode.Checked = _board.Mode == PlayMode.Collocation;
        }

        private void tsmiGameMode_Click(object sender, EventArgs e)
        {
            tsmiSelectSide.Enabled = true;
            _board.Mode = PlayMode.Game; // игра
            panelLog.Visible = true;
            UpdateStatus();
            Invalidate();
        }

        private void tsmiCollocationMode_Click(object sender, EventArgs e)
        {
            tsmiSelectSide.Enabled = false;
            _board.Mode = PlayMode.Collocation; // расстановка
            panelLog.Visible = false;
            status.Text = "Режим расстановки шашек";
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
            _net.Stop();
        }

        private void btnRefreshPeers_Click(object sender, EventArgs e)
        {
            if (_net.Started)
            {
                lbPeerList.Items.Clear();
                _net.RefreshPeers();
                btnRefreshPeers.Enabled = false;
            }
        }

        private void lbPeerList_DoubleClick(object sender, EventArgs e)
        {
            if (lbPeerList.SelectedIndex >= 0)
            {
                // Получение пира и прокси, для отправки сообщения
                var peerEntry = lbPeerList.SelectedItem as PeerEntry;
                if (_net.Started) _net.SendMessasge(peerEntry);
            }
        }
    }
}
