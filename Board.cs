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
        Prohibited,         // запрещено
        SuccessfullMove,    // разрешённое пермещение
        SuccessfullCombat   // разрешённое взятие шашки противника
    }

    public enum Player
    {
        White,
        Black
    }

    [Serializable]
    public class Board
    {
        const int mBoardSize = 8;
        private Hashtable mFields = new Hashtable();
        private Hashtable mCells = new Hashtable();

        public int SideSize { get { return mBoardSize; } }

        public void ResetMap()
        {
            const string whiteCheckers = "a1,c1,e1,g1,b2,d2,f2,h2,a3,c3,e3,g3";
            const string blackCheckers = "b6,d6,f6,h6,a7,c7,e7,g7,b8,d8,f8,h8";
            mCells.Clear();
            Direction = false;
            Selected = null;
            var black = false;  // признак чередования цветов полей
            for (var i = 0; i < mBoardSize; i++)
            {
                State state;
                for (var j = 0; j < mBoardSize; j++)
                {
                    var address = new Address(j, i);
                    if (whiteCheckers.Contains(address.ToString()))
                        state = State.White;
                    else if (blackCheckers.Contains(address.ToString()))
                        state = State.Black;
                    else
                        state = black ? State.Blank : State.Prohibited;
                    var cell = new Cell() { Address = address, State = state };
                    mCells[address] = cell;
                    mFields[address] = black ? State.Black : State.White;
                    black = !black; // чередуем цвет в столбце
                }
                black = !black;     // чередуем цвет в строке
            }
        }

        public int WhiteScore { get; set; }
        public int BlackScore { get; set; }

        public Hashtable GetMap()
        {
            return mCells;
        }

        public void SetMap(object value)
        {
            mCells = (Hashtable)value; 
        }

        public Hashtable GetFields()
        {
            return mFields;
        }

        public bool Direction { get; set; }

        public Cell Selected { get; set; }

        public Player Player { get; set; }

        /// <summary>
        /// Проверяется возможность сделать "ход" или "бой" из этой ячейки с адресом
        /// </summary>
        /// <param name="pos">Адрес ячейки, из которой нужно сделать ход</param>
        /// <returns></returns>
        public MoveResult HasMove(Address pos)
        {
            var result = MoveResult.Prohibited;
            if (pos.IsEmpty() || pos.IsEmpty()) return result;
            var cellState = ((Cell)mCells[pos]).State;
            // запрет хода для фишек не в свою очередь
            if (Direction && cellState != State.Black ||
                !Direction && cellState != State.White) return result;
            var list = new List<Address>();
            // добавляем адреса вокруг фишки для проверки
            list.AddRange(new Address[]{
                    // "ход"
                    new Address(pos.Coordinates().X - 1, pos.Coordinates().Y - 1),
                    new Address(pos.Coordinates().X + 1, pos.Coordinates().Y - 1),
                    new Address(pos.Coordinates().X - 1, pos.Coordinates().Y + 1),
                    new Address(pos.Coordinates().X + 1, pos.Coordinates().Y + 1),
                    // "бой"
                    new Address(pos.Coordinates().X - 2, pos.Coordinates().Y - 2),
                    new Address(pos.Coordinates().X + 2, pos.Coordinates().Y - 2),
                    new Address(pos.Coordinates().X - 2, pos.Coordinates().Y + 2),
                    new Address(pos.Coordinates().X + 2, pos.Coordinates().Y + 2)
                });
            foreach (var addr in list)
            {
                var check = CheckMove(pos, addr);
                if (check != MoveResult.Prohibited)
                    return check;
            }
            return result;
        }

        /// <summary>
        /// При текущем направлении игры проверяем, если у стороны "бьющие" фишки
        /// </summary>
        /// <returns></returns>
        public bool HasAnyCombat()
        {
            var result = false;
            foreach (var item in mCells)
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
            var cellState = ((Cell)mCells[pos]).State;
            // запрет хода для фишек не в свою очередь
            if (Direction && cellState != State.Black ||
                !Direction && cellState != State.White) return result;
            var list = new List<Address>
            {
                // "бой"
                new Address(pos.Coordinates().X - 2, pos.Coordinates().Y - 2),
                new Address(pos.Coordinates().X + 2, pos.Coordinates().Y - 2),
                new Address(pos.Coordinates().X - 2, pos.Coordinates().Y + 2),
                new Address(pos.Coordinates().X + 2, pos.Coordinates().Y + 2)
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

            var startCellState = ((Cell)mCells[startPos]).State;
            // запрет хода для фишек не в свою очередь
            if (Direction && startCellState != State.Black ||
                !Direction && startCellState != State.White) return result;
            
            var dX = endPos.Coordinates().X - startPos.Coordinates().X;
            var dY = endPos.Coordinates().Y - startPos.Coordinates().Y;

            var startCellIsKing = ((Cell)mCells[startPos]).King;
            var targetCellState = ((Cell)mCells[endPos]).State;
            if (targetCellState == State.Blank)
            {
                // проверка "боя"
                if (Math.Abs(dX) == 2 && Math.Abs(dY) == 2)
                {
                    // поиск "промежуточной" ячейки
                    var victimPos = new Address((startPos.Coordinates().X + endPos.Coordinates().X) / 2,
                                                (startPos.Coordinates().Y + endPos.Coordinates().Y) / 2);
                    var victimCellState = ((Cell)mCells[victimPos]).State;
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
                {
                    OnShowError("Нужно брать шашку противника!", "Правило хода");
                    return MoveResult.Prohibited;
                }
            }
            else if (moveResult == MoveResult.SuccessfullCombat)
            {
                MoveChecker(startPos, endPos);
                // снятие фишки противника
                var address = new Address((startPos.Coordinates().X + endPos.Coordinates().X) / 2,
                                          (startPos.Coordinates().Y + endPos.Coordinates().Y) / 2);
                RemoveChecker(address);
            }
            return moveResult;
        }

        public event Action<string, string> ShowError = delegate { };

        private void OnShowError(string message, string caption)
        {
            ShowError(message, caption);
        }

        /// <summary>
        /// Переносим фишку на доске (вспомогательный метод)
        /// </summary>
        /// <param name="startPos">Начальная позиция</param>
        /// <param name="endPos">Конечная позиция</param>
        private void MoveChecker(Address startPos, Address endPos)
        {
            var startCell = (Cell)mCells[startPos];
            var endCell = (Cell)mCells[endPos];
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
            var cell = (Cell)mCells[pos];
            cell.State = State.Blank;
            cell.King = false;
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
            // если нет возможности сделать "ход" или "бой"
            if (HasMove(cell.Address) == MoveResult.Prohibited) return false;
            // если по очереди некоторые фишки могут "ударить", но эта фишка "ударить" не может
            if (HasAnyCombat() && !HasCombat(cell.Address)) return false;
            return true;
        }
    }
}
