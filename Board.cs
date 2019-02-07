using System;
using System.Collections;
using System.Collections.Generic;

namespace Checkers
{
    /// <summary>
    /// Результат хода
    /// </summary>
    public enum MoveResult
    {
        Prohibited,                 // запрещено
        SuccessfullMove,            // разрешённое пермещение
        SuccessfullCombat           // разрешённое взятие шашки противника
    }

    public enum Player
    {
        White,
        Black
    }

    public enum PlayMode
    {
        Game,       // игра
        Collocation // расстановка фишек
    }

    [Serializable]
    public class Board
    {
        const int _boardSize = 8;
        private Hashtable _fields = new Hashtable();
        private Hashtable _cells = new Hashtable();
        private Cell _selected;
        private PlayMode _mode;
        private object _lastGameMap = null;
        private object _lastCollocationMap = null;

        private int _movedCount = 0;
        List<Cell> _goalCells = new List<Cell>();

        public int SideSize { get { return _boardSize; } }

        public PlayMode Mode
        {
            get { return _mode; }
            set
            {
                if (_mode != value)
                {
                    // сохраняем текущую расстановку
                    if (_mode == PlayMode.Game)
                    {
                        _lastGameMap = _cells.DeepClone();
                        Console.WriteLine("Сохранили игру");
                    }
                    else if (_mode == PlayMode.Collocation)
                    {
                        _lastCollocationMap = _cells.DeepClone();
                        Console.WriteLine("Сохранили расстановку");
                    }
                    _mode = value;
                    // восстанавливаем расстановка
                    if (_mode == PlayMode.Game && _lastGameMap != null)
                    {
                        var map = _lastGameMap.DeepClone();
                        SetMap(map);
                        Console.WriteLine("Восстановили игру");
                    }
                    else if (_mode == PlayMode.Collocation && _lastCollocationMap != null)
                    {
                        var map = _lastCollocationMap.DeepClone();
                        SetMap(map);
                        Console.WriteLine("Восстановили расстановку");
                    }
                }
            }
        }

        public void ResetMap()
        {
            var whiteCheckers = Mode == PlayMode.Game ? "a1,c1,e1,g1,b2,d2,f2,h2,a3,c3,e3,g3" : "";
            var blackCheckers = Mode == PlayMode.Game ? "b6,d6,f6,h6,a7,c7,e7,g7,b8,d8,f8,h8" : "";
            _cells.Clear();
            Direction = false;
            Selected = null;
            var black = false;  // признак чередования цветов полей
            for (var i = 0; i < _boardSize; i++)
            {
                State state;
                for (var j = 0; j < _boardSize; j++)
                {
                    var address = new Address(j, i);
                    if (whiteCheckers.Contains(address.ToString()))
                        state = State.White;
                    else if (blackCheckers.Contains(address.ToString()))
                        state = State.Black;
                    else
                        state = black ? State.Blank : State.Prohibited;
                    var cell = new Cell() { Address = address, State = state };
                    _cells[address] = cell;
                    _fields[address] = black ? State.Black : State.White;
                    black = !black; // чередуем цвет в столбце
                }
                black = !black;     // чередуем цвет в строке
            }
        }

        public int WhiteScore { get; set; }
        public int BlackScore { get; set; }

        public Hashtable GetMap()
        {
            return _cells;
        }

        public void SetMap(object value)
        {
            _cells = (Hashtable)value; 
        }

        public Hashtable GetFields()
        {
            return _fields;
        }

        public bool Direction { get; set; }

        public Cell Selected
        {
            get { return _selected; }
            set
            {
                _goalCells.Clear();
                _selected = value;
                if (_selected != null)
                    FillGoalCells(_selected);
            }
        }

        public Player Player { get; set; }

        /// <summary>
        /// При текущем направлении игры проверяем, если у стороны "бьющие" фишки
        /// </summary>
        /// <returns></returns>
        public bool HasAnyCombat()
        {
            var result = false;
            foreach (var item in _cells)
            {
                var cell = (Cell)((DictionaryEntry)item).Value;
                var cellState = cell.State;
                var cellAddress = cell.Address;
                if (cellState == State.Blank || cellState == State.Prohibited) continue;
                if ((Direction && cellState == State.Black ||
                    !Direction && cellState == State.White) && HasCombat(cellAddress))
                    return true;
            }
            return result;
        }

        /// <summary>
        /// Фишка на этой позиции может произвести "бой"
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool HasCombat(Address pos)
        {
            var result = false;
            if (pos.IsEmpty() || pos.IsEmpty()) return result;
            var cellState = ((Cell)_cells[pos]).State;
            // запрет хода для фишек не в свою очередь
            if (Direction && cellState != State.Black ||
                !Direction && cellState != State.White) return result;
            var list = new List<Address>
            {
                // "бой"
                new Address(pos.Coords.X - 2, pos.Coords.Y - 2),
                new Address(pos.Coords.X + 2, pos.Coords.Y - 2),
                new Address(pos.Coords.X - 2, pos.Coords.Y + 2),
                new Address(pos.Coords.X + 2, pos.Coords.Y + 2)
            };
            foreach (var addr in list)
                if (CheckMove(pos, addr) == MoveResult.SuccessfullCombat)
                    return true;
            return result;
        }

        /// <summary>
        /// Проверка возможности "хода" фишки
        /// </summary>
        /// <param name="startPos">Начальная позиция</param>
        /// <param name="endPos">Конечная позиция</param>
        /// <param name="direction">Направление проверки: false - фишки идут вверх, true - фишки идут вниз</param>
        /// <returns>Результат хода</returns>
        public MoveResult CheckMove(Address startPos, Address endPos)
        {
            var result = MoveResult.Prohibited;
            if (startPos.IsEmpty() || endPos.IsEmpty()) return result;

            var startCellState = ((Cell)_cells[startPos]).State;
            // запрет хода для фишек не в свою очередь
            if (Direction && startCellState != State.Black ||
                !Direction && startCellState != State.White) return result;
            
            var dX = endPos.Coords.X - startPos.Coords.X;
            var dY = endPos.Coords.Y - startPos.Coords.Y;

            var startCellIsKing = ((Cell)_cells[startPos]).King;
            var targetCellState = ((Cell)_cells[endPos]).State;
            if (targetCellState == State.Blank)
            {
                // проверка "боя"
                if (Math.Abs(dX) == 2 && Math.Abs(dY) == 2)
                {
                    // поиск "промежуточной" ячейки
                    var victimPos = new Address((startPos.Coords.X + endPos.Coords.X) / 2,
                                                (startPos.Coords.Y + endPos.Coords.Y) / 2);
                    var victimCellState = ((Cell)_cells[victimPos]).State;
                    // снимаем только фишку противника
                    result = targetCellState != victimCellState && startCellState != victimCellState
                                ? MoveResult.SuccessfullCombat : result;
                }
                else // проверка "хода"
                if (Math.Abs(dX) == 1 && dY == 1 && Direction || Math.Abs(dX) == 1 && dY == -1 && !Direction ||
                    // дамка "ходит" во все стороны
                    Math.Abs(dX) == 1 && Math.Abs(dY) == 1 && startCellIsKing)
                    result = MoveResult.SuccessfullMove;
            }
            return result;
        }

        /// <summary>
        /// Делаем "ход"
        /// </summary>
        /// <param name="startPos">Начальная позиция</param>
        /// <param name="endPos">Конечная позиция</param>
        /// <returns>результат хода</returns>
        public MoveResult MakeMove(Address startPos, Address endPos)
        {
            var moveResult = CheckMove(startPos, endPos);
            if (moveResult == MoveResult.SuccessfullMove)
            {
                if (!HasCombat(startPos))
                    MoveChecker(startPos, endPos);
                else
                    return MoveResult.Prohibited; // обязан брать шашку противника
            }
            else if (moveResult == MoveResult.SuccessfullCombat)
            {
                MoveChecker(startPos, endPos);
                // снятие фишки противника
                var address = new Address((startPos.Coords.X + endPos.Coords.X) / 2,
                                          (startPos.Coords.Y + endPos.Coords.Y) / 2);
                RemoveChecker(address);
            }
            return moveResult;
        }

        public event Action<string, string> ShowError = delegate { };

        private void OnShowError(string message, string caption)
        {
            ShowError(message, caption);
        }

        public event Func<string, string, bool> AskQuestion = delegate { return false; };

        private bool OnAskQuestion(string message, string caption)
        {
            return AskQuestion(message, caption);
        }

        /// <summary>
        /// Переносим фишку на доске (вспомогательный метод)
        /// </summary>
        /// <param name="startPos">Начальная позиция</param>
        /// <param name="endPos">Конечная позиция</param>
        private void MoveChecker(Address startPos, Address endPos)
        {
            var startCell = (Cell)_cells[startPos];
            var endCell = (Cell)_cells[endPos];
            endCell.State = startCell.State;
            endCell.King = startCell.King;
            RemoveChecker(startPos);
        }

        /// <summary>
        /// Убираем фишку с доски (вспомогательный метод)
        /// </summary>
        /// <param name="pos">Позиция фишки</param>
        private void RemoveChecker(Address pos)
        {
            var cell = (Cell)_cells[pos];
            cell.State = State.Blank;
            cell.King = false;
        }

        /// <summary>
        /// Получение ячейки по адресу, с защитой от пустого адреса
        /// </summary>
        /// <param name="address"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool GetCell(Address address, out Cell cell)
        {
            cell = null;
            if (address.IsEmpty()) return false;
            cell = (Cell)_cells[address];
            return true;
        }

        public List<Cell> GetGoals()
        {
            return _goalCells;
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

        public event Action ActivePlayerChanged = delegate { };

        private void OnActivePlayerChanged()
        {
            ActivePlayerChanged();
        }

        public void SelectCell(Address location)
        {
            Cell cell;
            if (GetCell(location, out cell) && cell.State != State.Prohibited)
            {
                // если ячейка не пустая, но не может быть выбрана
                if (cell.State != State.Blank && !CanCellEnter(cell)) return;
                // нет выбранной ячейки с фишкой
                if (Selected == null)
                    // можем выбирать фишки только цвета игрока
                    SetSelectedCell(cell);
                else
                // ранее была выбрана фишка и выбрана пустая клетка
                if (cell.State == State.Blank)
                // пробуем делать ход
                {
                    var startPos = Selected.Address;
                    var endPos = cell.Address;
                    var moveResult = MakeMove(startPos, endPos);
                    Selected = null;  // после хода сбрасываем текущую выбранную фишку
                    if (moveResult != MoveResult.Prohibited)
                    {
                        // подсчёт очков
                        if (moveResult == MoveResult.SuccessfullCombat)
                        {
                            if (Direction)
                                BlackScore++;
                            else
                                WhiteScore++;
                        }

                        _movedCount++;
                        // определение дамки
                        if (!cell.King &&
                            (!Direction && cell.Address.Row == SideSize ||
                             Direction && cell.Address.Row == 1))
                            cell.King = true;
                        var hasCombat = HasCombat(endPos); // есть ли в этой позиции возможность боя
                        // запоминаем очередь хода перед возможной сменой
                        var lastDirection = Direction;
                        // или был бой и далее нет возможности боя
                        if (moveResult == MoveResult.SuccessfullCombat && !hasCombat ||
                            moveResult == MoveResult.SuccessfullMove)
                        {
                            // сообщаем о перемещении фишки
                            OnCheckerMoved(lastDirection, startPos, endPos, moveResult, _movedCount);
                            // сбрасываем количество непрерывных ходов одной стороной
                            _movedCount = 0;
                            // передача очерёдности хода
                            Direction = !Direction;
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
                Selected = null;
        }

        /// <summary>
        /// Выбираем только "игровые" клетки, по которым фишки двигаются
        /// </summary>
        /// <param name="cell">Выбранная ячейка</param>
        private void SetSelectedCell(Cell cell)
        {
            if (cell.State == State.Black || cell.State == State.White)
            {
                Selected = cell;
            }
        }

        /// <summary>
        /// Проверка возможности выбрать указанную ячейку для начала "хода"
        /// </summary>
        /// <param name="cell">Целевая ячейка</param>
        /// <returns>true - выбор возможен</returns>
        public bool CanCellEnter(Cell cell)
        {
            // пустую ячейку сделать текущей не можем
            if (cell.State == State.Blank) return false;
            // "ходят" чёрные и пытаемся выбрать белую фишку
            if (Direction && cell.State == State.White) return false;
            // "ходят" белые и пытаемся выбрать чёрную фишку
            if (!Direction && cell.State == State.Black) return false;
            // у фишки нет ходов
            if (!HasGoals(cell)) return false;
            // если по очереди некоторые фишки могут "ударить", но эта фишка "ударить" не может
            if (HasAnyCombat() && !HasCombat(cell.Address)) return false;
            return true;
        }

        /// <summary>
        /// Есть ли у фишки вообще ходы?
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool HasGoals(Cell cell)
        {
            var goals = new List<Cell>();
            FillGoalCells(cell, goals);
            return goals.Count > 0;
        }

        /// <summary>
        /// Заполнение списка ячеек, куда возможно перемещение фишки, с учетом правил
        /// </summary>
        /// <param name="selectedCell">Текущая ячейка с фишкой</param>
        public void FillGoalCells(Cell selectedCell, List<Cell> goals = null)
        {
            var goalList = goals ?? _goalCells; 
            var pos = selectedCell.Address;
            var king = selectedCell.King;
            //
            AddGoal(goalList, pos, -2, -2, king);
            AddGoal(goalList, pos, +2, -2, king);
            AddGoal(goalList, pos, -2, +2, king);
            AddGoal(goalList, pos, +2, +2, king);
            if (_goalCells.Count > 0) return;
            AddGoal(goalList, pos, -1, -1, king);
            AddGoal(goalList, pos, +1, -1, king);
            AddGoal(goalList, pos, -1, +1, king);
            AddGoal(goalList, pos, +1, +1, king);
        }

        /// <summary>
        /// Добавление целевого поля для шашки
        /// </summary>
        /// <param name="goalList">Список целей, накопительный</param>
        /// <param name="pos">адрес ячейки, вокруг которой ищется цель</param>
        /// <param name="dx">шаг поиска по горизонтали</param>
        /// <param name="dy">шаг поиска по вертикали</param>
        private bool AddGoal(List<Cell> goalList, Address pos, int dx, int dy, bool king)
        {
            var result = false;
            var addr = new Address(pos.Coords.X + dx, pos.Coords.Y + dy);
            Console.WriteLine(addr + (king ? " king" : ""));
            var check = CheckMove(pos, addr);
            if (check != MoveResult.Prohibited)
            {
                goalList.Add((Cell)_cells[addr]);
                if (king)
                    result = result || AddGoal(goalList, addr, dx, dy, king);
                else
                    result = true;
            }
            return result;
        }
    }
}
