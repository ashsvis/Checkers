using System;

namespace Checkers
{
    /// <summary>
    /// Prohibited, Blank, White, Black
    /// </summary>
    public enum State
    {
        Prohibited = 3,
        Blank = 0,
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
        /// Назначение ячейки: Prohibited, Blank, White, Black
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Признак "дамки"
        /// </summary>
        public bool King { get; set; }
    }
}
