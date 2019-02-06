using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Checkers
{
    public partial class CheckersForm : Form
    {
        private Io _io;
        private Board _board;
        private List<LogItem> _log;

        public CheckersForm()
        {
            InitializeComponent();
            mainStatus.SizingGrip = false;
            DoubleBuffered = true;
            _log = new List<LogItem>();
            _board = new Board();
            _board.ShowError += _board_ShowError;
            _io = new Io(_board, new Size(0, mainMenu.Height + mainTools.Height));
            _io.ActivePlayerChanged += _io_ActivePlayerChanged;
            _io.CheckerMoved += _io_CheckerMoved;
        }

        private void _board_ShowError(string text, string caption)
        {
            status.Text = string.Format(_board.Direction 
                ? "Ход чёрных ({0})..." : "Ход белых ({0})...", 
                text.ToLower().TrimEnd('!'));
            //MessageBox.Show(this, text, caption);
        }

        private void _io_CheckerMoved(bool direction, Address startPos, Address endPos, MoveResult moveResult, int stepCount)
        {
            UpdateLog(direction, startPos, endPos, moveResult, stepCount);
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
                    _log.Add(new LogItem() { Number = _log.Count + 1, White = result, Map = _board.GetMap().DeepClone() });
                    lvLog.VirtualListSize = _log.Count;
                    var item = lvLog.Items[lvLog.Items.Count - 1];
                    lvLog.FocusedItem = item;
                    item.EnsureVisible();
                    lvLog.Invalidate();
                }
                else
                {
                    var value = _log[_log.Count - 1];
                    value.White += ":" + endPos;
                    value.Map = _board.GetMap().DeepClone();
                    lvLog.Invalidate();
                }
            }
            else
            {
                // ходят "чёрные"
                var value = _log[_log.Count - 1];
                if (stepCount == 1) // первый ход (из, возможно, серии ходов)
                    value.Black = result;
                else
                    value.Black += ":" + endPos;
                value.Map = _board.GetMap().DeepClone();
                lvLog.Invalidate();
            }
        }

        private void _io_ActivePlayerChanged()
        {
            UpdateStatus();
        }

        private void CheckersForm_Load(object sender, EventArgs e)
        {
            _board.ResetMap();
            var size = _io.GetDrawBoardSize();
            size.Width += panelInfo.Width;
            size.Height += mainMenu.Height + mainTools.Height + mainStatus.Height;
            ClientSize = size;
            UpdateStatus();
            CenterToScreen();
        }

        private void UpdateStatus()
        {
            lbWhiteScore.Text = string.Format("Белые: {0}", _board.WhiteScore);
            lbBlackScore.Text = string.Format("Чёрные: {0}", _board.BlackScore);
            status.Text = _board.WhiteScore == 12 
                ? "Белые выиграли" 
                : _board.BlackScore == 12 
                       ? "Чёрные выиграли" 
                       : _board.Direction ? "Ход чёрных..." : "Ход белых...";
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
            _io.MouseDown(e.Location, ModifierKeys);
            Invalidate();
        }

        private void CheckersForm_MouseMove(object sender, MouseEventArgs e)
        {
            _io.MouseMove(e.Location, ModifierKeys);
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
                _board.Player = Player.White;
            else if (sender == tsmiBlackSide)
                _board.Player = Player.Black;
            Invalidate();
        }

        /// <summary>
        /// Отображение текущей стороны игрока
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiSelectSide_DropDownOpening(object sender, EventArgs e)
        {
            tsmiBlackSide.Checked = _board.Player == Player.Black;
            tsmiWhiteSide.Checked = _board.Player == Player.White;
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
            _log.Clear();
            _board.WhiteScore = 0;
            _board.BlackScore = 0;
            lvLog.VirtualListSize = _log.Count;
            lvLog.Invalidate();
            lbWhiteScore.Text = string.Format("Белые: {0}", _board.WhiteScore);
            lbBlackScore.Text = string.Format("Чёрные: {0}", _board.BlackScore);
        }

        private void lvLog_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = new ListViewItem();
            var item = _log[e.ItemIndex];
            e.Item.Text = item.Number.ToString();
            e.Item.SubItems.Add(item.White);
            e.Item.SubItems.Add(item.Black);
        }

        private void lvLog_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            e.Index = _log.Count > 0 ? _log.Count - 1 : -1;
            
        }

        private void lvLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            var n = lvLog.SelectedIndices.Count > 0 ? lvLog.SelectedIndices[0] : -1;
            if (n < 0) return;
            var map = _log[n].Map.DeepClone();
            _board.SetMap(map);
            Invalidate();
        }

        private void tsmiTunings_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
