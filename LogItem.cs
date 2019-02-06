using System;

namespace Checkers
{
    [Serializable]
    public class LogItem
    {
        /// <summary>
        /// Номер по порядку
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Ход белых
        /// </summary>
        public string White { get; set; }

        /// <summary>
        /// Ход чёрных
        /// </summary>
        public string Black { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Map { get; set; }

        /// <summary>
        /// Текстовое представления
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}. {1}\t{2}", Number, White, Black);
        }
    }
}
