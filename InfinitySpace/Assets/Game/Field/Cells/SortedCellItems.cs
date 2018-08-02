using Assets.Game.Access;

using UnityEngine;

namespace Assets.Game.Field.Cells
{
    /// <summary>
    /// Сортированные позиции в ячейке, близкие к рейтингу игрока
    /// </summary>
    public class SortedCellItems
    {
        #region Fields

        /// <summary>
        /// Позиции
        /// </summary>
        private SortedCellItem[]
            _items;

        /// <summary>
        /// Максимальная дистанция рейтинга в списке
        /// </summary>
        private ushort
            _maxRatingDistance;

        /// <summary>
        /// Индекс максимальной дистанции рейтинга
        /// </summary>
        private int
            _maxRatingDistanceIndex;

        #endregion

        #region ctor

        public SortedCellItems()
        {
            _items = new SortedCellItem[SettingsAccess.MaxAdvancedVisiblePlanet];

            Clear();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Очистить
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                _items[i] = SortedCellItem.Empty;
            }

            _maxRatingDistance = ushort.MaxValue;
            _maxRatingDistanceIndex = 0;
        }

        /// <summary>
        /// Возвращает первые <see cref="SettingsAccess.MaxAdvancedVisiblePlanet"/> отсортированных ячеек
        /// </summary>
        /// <returns></returns>
        public SortedCellItem[] GetValues()
        {
            return _items;
        }

        public void Append(SortedCellItems cells)
        {
            for (int i = 0; i < cells._items.Length; i++)
            {
                var cellItem = cells._items[i];

                if (IsBelowThanMaxRatingDistance(cellItem.RatingDistance))
                {
                    AppendCellItemFast(cellItem);
                }
            }
        }

        /// <summary>
        /// Дистанция меньше существующей максимальной дистанции.
        /// </summary>
        /// <param name="ratingDistance">Дистанция проверяемым рейтингом и рейтингом игрока</param>
        /// <returns>Рейтинг меньше, чем максимальный рейтинг в коллекции</returns>
        public bool IsBelowThanMaxRatingDistance(ushort ratingDistance)
        {
            return ratingDistance < _maxRatingDistance;
        }

        /// <summary>
        /// Добавить позицию, если она проходит проверку в <see cref="IsBelowThanMaxRatingDistance"/>. Рекомендуется использовать <see cref="AppendCellItemRecommended"/>.
        /// </summary>
        /// <example> 
        /// Используется для ускорения наполнения ячейки данными, использование <see cref="AppendCellItemFast"/> осуществляется следующим способом:
        /// <code>
        /// if (IsBelowThanMaxRatingDistance(ratingDistance))
        /// {
        ///     AppendCellItem(new SortedCellItem(Pos, i, rating, ratingDistance));
        /// }
        /// </code>
        /// </example>
        /// <param name="item">Дистанция проверяемым рейтингом и рейтингом игрока</param>
        /// <returns>Рейтинг меньше, чем максимальный рейтинг в коллекции</returns>
        public void AppendCellItemFast(SortedCellItem item)
        {
            _items[_maxRatingDistanceIndex] = item;

            _maxRatingDistance = _items[0].RatingDistance;
            _maxRatingDistanceIndex = 0;

            for (int i = 1; i < _items.Length; i++)
            {
                var cell = _items[i];

                if (cell.RatingDistance > _maxRatingDistance)
                {
                    _maxRatingDistance = cell.RatingDistance;
                    _maxRatingDistanceIndex = i;
                }
            }
        }

        /// <summary>
        /// Если дистанция между рейтингом позиции и рейтингом игрока меньше существующей, добавить её в коллекцию
        /// </summary>
        /// <param name="pos">Координаты ячейки</param>
        /// <param name="index">Индекс позиции в ячейке</param>
        /// <param name="rating">Рейтинг</param>
        /// <param name="playerRating">Рейтинг игрока</param>
        public void AppendCellItemRecommended(Vector2Int pos, ushort index, ushort rating, ushort playerRating)
        {
            // Если дистанция меньше самого большого из показателей - заменим её на текущую
            // Если дистанция равна самой большой, значит это конец списка и её мы не добавляем (так как самая большая дистанция у нас равна ushort.MaxValue и не существует)
            // Если дистанция больше самой большой, значит она нам не интересна

            var ratingDistance = (ushort)Mathf.Abs(rating - playerRating);

            if (IsBelowThanMaxRatingDistance(ratingDistance))
            {
                AppendCellItemFast(new SortedCellItem(pos, index, rating, ratingDistance));
            }
        }

        #endregion
    }
}
