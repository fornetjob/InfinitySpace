using Assets.Game.Access;
using Assets.Game.Core;
using Assets.Game.Field.Generators.DataContracts.Base;

using UnityEngine;

namespace Assets.Game.Field.Cells
{
    /// <summary>
    /// Ячейка на поле
    /// </summary>
    public class CellInfo
    {
        #region Fields

        /// <summary>
        /// Рейтинги в видимой игроку ячейке
        /// </summary>
        private ushort[]
            _ratings;

        #endregion

        #region Properties

        /// <summary>
        /// Позиция
        /// </summary>
        public Vector2Int Pos;
        /// <summary>
        /// Первых 20 рейтингов в ячейке, близкие к рейтингу игрока
        /// </summary>
        public SortedCellItems SortedCellItems = new SortedCellItems();

        #endregion

        #region Public methods

        /// <summary>
        /// Возвращает мировые координаты
        /// </summary>
        /// <param name="index">Индекс в ячейке</param>
        /// <returns>Мировые координаты</returns>
        public Vector2Int GetWordPosition(ushort index)
        {
            var cellItemPos = GetCellItemPosition(index);

            return new Vector2Int(Pos.x + cellItemPos.x, Pos.y + cellItemPos.y);
        }

        /// <summary>
        /// Возвращает локальные координаты позиции в ячейке
        /// </summary>
        /// <param name="worldPos">Мировые координаты</param>
        /// <returns>Локальные координаты позиции</returns>
        public Vector2Byte GetCellItemPosition(Vector2Int worldPos)
        {
            if (Mathf.Max(Mathf.Abs(worldPos.x - Pos.x), Mathf.Abs(worldPos.y - Pos.y)) > SettingsAccess.CellPxSize)
            {
                throw new System.ArgumentOutOfRangeException(string.Format("Позиция {0} не принадлежит ячейке {1}", worldPos, Pos));
            }

            byte posX = (byte)(Mathf.Abs(worldPos.x - Pos.x));
            byte posY = (byte)(Mathf.Abs(worldPos.y - Pos.y));

            return new Vector2Byte(posX, posY);
        }

        /// <summary>
        /// Возвращает индекс по локальным координатам позиции в ячейке
        /// </summary>
        /// <param name="pos">Локальные координаты позиции</param>
        /// <returns>Индекс позиции в ячейке</returns>
        public ushort GetCellItemIndex(Vector2Byte pos)
        {
            return (ushort)(pos.x + pos.y * SettingsAccess.CellPxSize);
        }

        /// <summary>
        /// Возвращает локальные координаты позиции в ячейке
        /// </summary>
        /// <param name="index">Индекс позиции в ячейке</param>
        /// <returns>Локальные координаты</returns>
        public Vector2Byte GetCellItemPosition(ushort index)
        {
            return new Vector2Byte((byte)(index % SettingsAccess.CellPxSize), (byte)(Mathf.FloorToInt(index / SettingsAccess.CellPxSize)));
        }

        /// <summary>
        /// Возвращает прямоугольник ячейки
        /// </summary>
        /// <param name="precision">Зум</param>
        /// <returns>Прямоугольник ячейки</returns>
        public Rect GetRect(float precision)
        {
            Vector2 pos = new Vector2(Pos.x, Pos.y) * SettingsAccess.CellWorldSize * precision;
            var size = new Vector2(SettingsAccess.CellPxSize, SettingsAccess.CellPxSize) * SettingsAccess.CellWorldSize * precision;

            return new Rect(pos, size);
        }

       
        /// <summary>
        /// Инициализировать ячейку
        /// </summary>
        /// <param name="id">Уникальный идентификатор ячейки</param>
        /// <param name="pos">Позиция ячейки</param>
        public void InitCell(Vector2Int pos, ushort[] ratings)
        {
            Pos = pos;

            SortedCellItems.Clear();

            _ratings = ratings;
        }

        /// <summary>
        /// Возвращает рейтинг по локальным координатам
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public ushort GetRating(Vector2Byte pos)
        {
            return GetRating(GetCellItemIndex(pos));
        }

        /// <summary>
        /// Возвращает рейтинг по индексу
        /// </summary>
        /// <param name="index">Индекс позиции в ячейке</param>
        /// <returns>Рейтинг</returns>
        public ushort GetRating(int index)
        {
            if (_ratings == null)
            {
                return 0;
            }

            return _ratings[index];
        }

        /// <summary>
        /// Заполняет рейтинги данными из задания на генерацию
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="index">Текущий индекс</param>
        /// <param name="playerRating">Рейтинг игрока</param>
        /// <returns>Индекс, увеличенный на величину прочитанных данных</returns>
        public int FillRatings(ISliceData data, int index, ushort playerRating)
        {
            for (ushort i = 0; i < SettingsAccess.CellPxLenght; i++)
            {
                var rating = data.GetRating(i + index);

                if (_ratings != null)
                {
                    _ratings[i] = rating;
                }

                if (rating > 0)
                {
                    var ratingDistance = (ushort)Mathf.Abs(rating - playerRating);

                    if (SortedCellItems.IsBelowThanMaxRatingDistance(ratingDistance))
                    {
                        SortedCellItems.AppendCellItemFast(new SortedCellItem(Pos, i, rating, ratingDistance));
                    }
                }
            }

            return index + SettingsAccess.CellPxLenght;
        }

        #endregion
    }
}