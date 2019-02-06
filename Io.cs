using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers
{
    public class Io
    {
        const int CellSize = 50;    // размер стороны клетки в пикселах
        const int BorderWidth = 20; // ширина бордюра в пикселах
        Board _board;
        Size _topLeftSize;
        List<Cell> _hoverCells;

        public Io(Board board, Size topLeftSize)
        {
            _board = board;
            _topLeftSize = topLeftSize;
            _hoverCells = new List<Cell>();
        }

        /// <summary>
        /// Получение расчётного размера доски
        /// </summary>
        /// <returns>Ширина и высота доски вместе с бордюром</returns>
        public Size GetDrawBoardSize()
        {
            var boardSize = _board.SideSize;
            return new Size(boardSize * CellSize + BorderWidth * 2, boardSize * CellSize + BorderWidth * 2);
        }

        /// <summary>
        /// Получение адреса клетки под курсором
        /// </summary>
        /// <param name="mouse">Координаты курсора</param>
        /// <returns>Возвращается адрес клетки</returns>
        public Address GetCellAddress(Point mouse)
        {
            var side = _board.Player == Player.Black; // переворот доски
            var boardSize = _board.SideSize;
            for (var i = 0; i < boardSize; i++)
            {
                var ix = side ? boardSize - i - 1 : i;
                for (var j = 0; j < boardSize; j++)
                {
                    var jx = side ? boardSize - j - 1 : j;
                    var rect = new Rectangle(_topLeftSize.Width + BorderWidth + jx * CellSize, 
                        _topLeftSize.Height + BorderWidth + ix * CellSize, CellSize, CellSize);
                    if (rect.Contains(mouse))
                        return new Address(j, i);
                }
            }
            return new Address();
        }

        /// <summary>
        /// Получение клетки по координатам под курсором
        /// </summary>
        /// <param name="mouse">Координаты курсора</param>
        /// <param name="cell">Найденная клетка доски</param>
        /// <returns>true - клетка найдена под курсором</returns>
        public bool GetCell(Point mouse, out Cell cell)
        {
            cell = null;
            var address = GetCellAddress(mouse);
            if (address.IsEmpty()) return false;
            var map = _board.GetMap();
            cell = (Cell)map[address];
            return true;
        }

        /// <summary>
        /// Рисование доски
        /// </summary>
        /// <param name="graphics">Канва для рисования</param>
        public void DrawBoard(Graphics graphics)
        {
            var side = _board.Player == Player.Black; // переворот доски
            var fields = _board.GetFields();
            var map = _board.GetMap();
            var cellsCount = _board.SideSize;
            var boardSize = GetDrawBoardSize();
            var boardRect = new Rectangle(new Point(_topLeftSize.Width, _topLeftSize.Height), boardSize);
            using (var brush = new SolidBrush(Color.FromArgb(234, 206, 175)))
                graphics.FillRectangle(brush, boardRect);
            DrawCharBorder(graphics, boardRect, cellsCount, side);
            DrawNumberBorder(graphics, boardRect, cellsCount, side);
            // рисуем поля доски
            for (var i = 0; i < cellsCount; i++)
            {
                var ix = side ? cellsCount - i - 1 : i;
                for (var j = 0; j < cellsCount; j++)
                {
                    var jx = side ? cellsCount - j - 1 : j;
                    var rect = new Rectangle(_topLeftSize.Width + BorderWidth + jx * CellSize,
                        _topLeftSize.Height + BorderWidth + ix * CellSize, CellSize, CellSize);
                    var address = new Address(j, i);
                    var fieldState = (State)fields[address]; // цвет поля доски
                    var mapCell = (Cell)map[address];        // наличие и цвет фигур
                    using (var brush = new SolidBrush(fieldState == State.Black 
                                             ? Color.FromArgb(129, 112, 94) 
                                             : Color.FromArgb(233, 217, 200)))
                        graphics.FillRectangle(_hoverCells.Contains(mapCell) ? Brushes.DarkGray : brush, rect);
                    if (mapCell.State == State.White || mapCell.State == State.Black)
                    {
                        var sizeW = CellSize - (int)(CellSize * 0.91);
                        var sizeH = CellSize - (int)(CellSize * 0.91);
                        rect.Inflate(-sizeW, -sizeH);
                        using (var brush = new SolidBrush(mapCell.State == State.Black
                                             ? Color.FromArgb(10, 10, 10)
                                             : Color.FromArgb(250, 250, 250)))
                            graphics.FillEllipse(brush, rect);
                        rect.Inflate(-sizeW, -sizeH);
                        graphics.DrawEllipse(Pens.Gray, rect);
                        if (!mapCell.King)
                        {
                            rect.Inflate(-sizeW, -sizeH);
                            graphics.DrawEllipse(Pens.Gray, rect);
                        }
                    }
                }
            }
            // подсвечивание рамки с выбранным для хода полем
            if (_board.Selected != null)
            {
                var address = _board.Selected.Address;
                var point = address.Coordinates();
                // коррекция координат в зависимости от стороны игрока
                point.X = side ? cellsCount - point.X - 1 : point.X;
                point.Y = side ? cellsCount - point.Y - 1 : point.Y;
                var rect = new Rectangle(_topLeftSize.Width + BorderWidth + point.X * CellSize,
                    _topLeftSize.Height + BorderWidth + point.Y * CellSize, CellSize, CellSize);
                using (var pen = new Pen(Color.Aqua, 1))
                {
                    graphics.DrawRectangle(pen, rect);
                }
            }
        }

        /// <summary>
        /// Рисование букв столбцов по обеим сторонам доски
        /// </summary>
        /// <param name="graphics">Канва для рисования</param>
        /// <param name="boardRect">Размер доски вместе с рамкой</param>
        /// <param name="cellsCount">Количество клеток по стороне</param>
        /// <param name="side">Сторона игрока (нижняя): false - белые, true - чёрные</param>
        private void DrawCharBorder(Graphics graphics, Rectangle boardRect, int cellsCount, bool side)
        {
            var chars = side ? "HGFEDCBA" : "ABCDEFGH";
            for (var j = 0; j < cellsCount; j++)
            {
                var topRect = new Rectangle(BorderWidth + j * CellSize,  _topLeftSize.Height, CellSize, BorderWidth);
                var bottomRect = new Rectangle(BorderWidth + j * CellSize, 
                                           _topLeftSize.Height + boardRect.Height - BorderWidth, CellSize, BorderWidth);
                using (var sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    var ch = chars.ToCharArray()[j].ToString();
                    graphics.DrawString(ch, SystemFonts.CaptionFont, Brushes.Black, topRect, sf);
                    graphics.DrawString(ch, SystemFonts.CaptionFont, Brushes.Black, bottomRect, sf);
                }
            }
        }

        /// <summary>
        /// Рисование номеров строк по обеим сторонам доски
        /// </summary>
        /// <param name="graphics">Канва для рисования</param>
        /// <param name="boardRect">Размер доски вместе с рамкой</param>
        /// <param name="cellsCount">Количество клеток по стороне</param>
        /// <param name="side">Сторона игрока (нижняя): false - белые, true - чёрные</param>
        private void DrawNumberBorder(Graphics graphics, Rectangle boardRect, int cellsCount, bool side)
        {
            var chars = side ? "12345678" : "87654321";
            for (var i = 0; i < cellsCount; i++)
            {
                var topRect = new Rectangle(0, _topLeftSize.Height + BorderWidth + i * CellSize, BorderWidth, CellSize);
                var bottomRect = new Rectangle(boardRect.Width - BorderWidth, 
                                               _topLeftSize.Height + BorderWidth + i * CellSize, BorderWidth, CellSize);
                using (var sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    var ch = chars.ToCharArray()[i].ToString();
                    graphics.DrawString(ch, SystemFonts.CaptionFont, Brushes.Black, topRect, sf);
                    graphics.DrawString(ch, SystemFonts.CaptionFont, Brushes.Black, bottomRect, sf);
                }
            }
        }

        private int _movedCount;

        /// <summary>
        /// Пользователь выбрал курсором ячейку или фишку
        /// </summary>
        /// <param name="location"></param>
        /// <param name="modifierKeys"></param>
        public void MouseDown(Point location, Keys modifierKeys)
        {
            Cell cell;
            if (GetCell(location, out cell) && cell.State != State.Prohibited)
            {
                // если ячейка не пустая, но не может быть выбрана
                if (cell.State != State.Blank && !_board.CanCellEnter(cell)) return;
                // нет выбранной ячейки с фишкой
                if (_board.Selected == null)
                    // можем выбирать фишки только цвета игрока
                    SetSelectedCell(cell);
                else
                // ранее была выбрана фишка и выбрана пустая клетка
                if (cell.State == State.Blank)
                // пробуем делать ход
                {
                    var startPos = _board.Selected.Address;
                    var endPos = cell.Address;
                    var moveResult = _board.MakeMove(startPos, endPos);
                    _board.Selected = null;  // после хода сбрасываем текущую выбранную фишку
                    if (moveResult != MoveResult.Prohibited)
                    {
                        // подсчёт очков
                        if (moveResult == MoveResult.SuccessfullCombat)
                        {
                            if (_board.Direction)
                                _board.BlackScore++;
                            else
                                _board.WhiteScore++;
                        }

                        _movedCount++;
                        // определение дамки
                        if (!cell.King && 
                            (!_board.Direction && cell.Address.Row == _board.SideSize ||
                             _board.Direction && cell.Address.Row == 1))
                            cell.King = true;
                        var hasCombat = _board.HasCombat(endPos); // есть ли в этой позиции возможность боя
                        // запоминаем очередь хода перед возможной сменой
                        var lastDirection = _board.Direction;
                        // или был бой и далее нет возможности боя
                        if (moveResult == MoveResult.SuccessfullCombat && !hasCombat ||
                            moveResult == MoveResult.SuccessfullMove)
                        {
                            // сообщаем о перемещении фишки
                            OnCheckerMoved(lastDirection, startPos, endPos, moveResult, _movedCount);
                            // сбрасываем количество непрерывных ходов одной стороной
                            _movedCount = 0;
                            // передача очерёдности хода
                            _board.Direction = !_board.Direction;
                            OnActivePlayerChanged();
                            return;
                        }
                        else if (moveResult == MoveResult.SuccessfullCombat && hasCombat)
                            // выбрана фишка для продолжения боя
                            SetSelectedCell(cell);
                        // сообщаем о перемещении фишки
                        OnCheckerMoved(lastDirection, startPos, endPos, moveResult, _movedCount);
                    }
                }
                else // была выбрана другая фишка того же цвета
                    SetSelectedCell(cell);
            }
            else // было выбрано неигровое поле
                _board.Selected = null;
        }

        /// <summary>
        /// Выбираем только "игровые" клетки, по которым фишки двигаются
        /// </summary>
        /// <param name="cell">Выбранная ячейка</param>
        private void SetSelectedCell(Cell cell)
        {
            if (cell.State == State.Black || cell.State == State.White)
                _board.Selected = cell;
        }

        public event Action ActivePlayerChanged = delegate { };

        private void OnActivePlayerChanged()
        {
            ActivePlayerChanged();
        }

        public event Action<bool, Address, Address, MoveResult, int> CheckerMoved = delegate { };

        /// <summary>
        /// Обработка события перемещения фишки
        /// </summary>
        /// <param name="direction">Текущее направление игры: false - ходят белые</param>
        /// <param name="startPos">Начальное положение</param>
        /// <param name="endPos">Конечное положение</param>
        /// <param name="moveResult">Результат хода</param>
        private void OnCheckerMoved(bool direction, Address startPos, Address endPos, MoveResult moveResult, int stepCount)
        {
            CheckerMoved(direction, startPos, endPos, moveResult, stepCount);
        }  

        Cell _lastCell; // была выбрана в прошлый раз

        /// <summary>
        /// Перемещение указателя мыши
        /// </summary>
        /// <param name="location">Позиция курсора</param>
        /// <param name="modifierKeys">Модификаторные клавиши</param>
        public void MouseMove(Point location, Keys modifierKeys)
        {
            var address = GetCellAddress(location);
            Cell cell;
            // если под курсором найдена разрешённая ячейка
            if (GetCell(location, out cell) && cell.State != State.Prohibited)
            {
                // и эта ячейка другая 
                if (cell != _lastCell)
                {
                    // если уже была выбрана другая ячейка
                    if (_lastCell != null) LeaveCell(_lastCell);
                    _lastCell = cell;   // запоминаем выбранную ячейку
                    // пытаемся выбрать эту ячейку
                    EnterCell(cell);
                }
            }
            else if (_lastCell != null)
            {
                // покидаем ячейку
                LeaveCell(_lastCell);
                _lastCell = null;
            }
        }

        /// <summary>
        /// Попытка выбрать ячейку
        /// </summary>
        /// <param name="cell"></param>
        private void EnterCell(Cell cell)
        {
            _hoverCells.Clear();
            // добавляем, только если можно выбрать
            if (_board.CanCellEnter(cell))
                _hoverCells.Add(cell);
        }

        /// <summary>
        /// Действия при покидании ячейки
        /// </summary>
        /// <param name="cell"></param>
        private void LeaveCell(Cell cell)
        {
            _hoverCells.Clear();
        }
    }
}
