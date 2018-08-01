using Assets.Game.Field.Generators.DataContracts.Base;

using System;

using UnityEngine;

namespace Assets.Game.Field.Generators.DataContracts
{
    /// <summary>
    /// Текущее состояние куска задания, получаемое из пикселей
    /// </summary>
    public class PixelsSliceData : ISliceData, IDisposable
    {
        #region Fields

        /// <summary>
        /// Текущая позиция куска
        /// </summary>
        private Vector2Int
            _pos;

        /// <summary>
        /// Пиксели в которых находится рейтинги планет для текущего куска
        /// </summary>
        private Color32[]
            _pixels;

        #endregion

        #region Public methods

        /// <summary>
        /// Обновить состояние
        /// </summary>
        /// <param name="pixels">Пиксели, в которых находятся рейтинги планет для текущего куска</param>
        public void ReloadValues(Color32[] pixels)
        {
            _pixels = pixels;
        }

        /// <summary>
        /// Обновить позицию
        /// </summary>
        /// <param name="pos">Позиция куска</param>
        public void ReloadPos(Vector2Int pos)
        {
            _pos = pos;
        }

        /// <summary>
        /// Получить текущую позицию куска
        /// </summary>
        /// <returns>Текущая позиция</returns>
        public Vector2Int GetPos()
        {
            return _pos;
        }

        /// <summary>
        /// Получить количество
        /// </summary>
        /// <returns>Количество рейтингов в куске</returns>
        public int GetCount()
        {
            return _pixels.Length;
        }

        /// <summary>
        /// Получить рейтинг
        /// </summary>
        /// <param name="index">Индекс рейтинга в коллекции</param>
        /// <returns>Рейтинг планеты от 1 до 10001 (0 если она отсутствует)</returns>
        public ushort GetRating(int index)
        {
            var pix = _pixels[index];

            return (ushort)(pix.r + pix.g * 256);
        }

        public void Dispose()
        {
            _pixels = null;
        }

        #endregion
    }
}
