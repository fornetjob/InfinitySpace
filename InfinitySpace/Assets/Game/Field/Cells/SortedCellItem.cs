using UnityEngine;

namespace Assets.Game.Field.Cells
{
    /// <summary>
    /// Хранит данные сортировки в ячейке
    /// </summary>
    public struct SortedCellItem
    {
        #region Statics

        /// <summary>
        /// Пустая позиция ячейки
        /// </summary>
        public static readonly SortedCellItem Empty = new SortedCellItem(Vector2Int.zero, 0, 0, ushort.MaxValue);

        #endregion

        #region ctor

        public SortedCellItem(Vector2Int cellPos, ushort index, ushort rating, ushort ratingDistance)
        {
            CellPos = cellPos;
            Index = index;
            Rating = rating;
            RatingDistance = ratingDistance;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Координаты ячейки
        /// </summary>
        public readonly Vector2Int CellPos;
        /// <summary>
        /// Индекс позиции в ячейке
        /// </summary>
        public readonly ushort Index;
        /// <summary>
        /// Рейтинг
        /// </summary>
        public readonly ushort Rating;
        /// <summary>
        /// Дистанция между рейтингом позиции в ячейки и рейтингом игрока
        /// </summary>
        public readonly ushort RatingDistance;
        /// <summary>
        /// Ячейка пустая
        /// </summary>
        /// <returns>Результат проверки</returns>
        public bool IsEmpty()
        {
            return RatingDistance == ushort.MaxValue;
        }

        #endregion
    }
}
