using System;

namespace Checkers
{
    /// <summary>
    /// Empty, Black, White, Prohibited
    /// </summary>
    public enum State
    {
        Prohibited = 3,
        Empty = 0,
        White = 2,
        Black = 1
    }

    /// <summary>
    /// Клетка доски
    /// </summary>
    [Serializable]
    public class Cell
    {
        /// <summary>
        /// Адрес ячейки
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Состояние ячейки: Empty, Black, White, Prohibited
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Признак "дамки"
        /// </summary>
        public bool King { get; set; }
    }
}
