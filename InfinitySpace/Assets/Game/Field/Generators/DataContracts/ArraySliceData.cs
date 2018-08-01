using Assets.Game.Field.Generators.DataContracts.Base;

using System;

using UnityEngine;

namespace Assets.Game.Field.Generators.DataContracts
{
    /// <summary>
    /// Текущее состояние куска задания, получаемое из массива
    /// </summary>
    public class ArraySliceData : ISliceData, IDisposable
    {
        #region Fields

        /// <summary>
        /// Текущая позиция куска
        /// </summary>
        private Vector2Int
            _pos;

        /// <summary>
        /// Рейтинги планет для текущего куска
        /// </summary>
        private uint[]
              _values;

        /// <summary>
        /// Количество анализируемых в буффере результатов
        /// </summary>
        private int
            _count;

        #endregion

        #region ctor

        public ArraySliceData(uint[] values)
        {
            _values = values;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Установить количество анализируемых в буффере результатов
        /// </summary>
        /// <param name="count">Количество</param>
        public void SetCount(int count)
        {
            _count = count;
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
            return _count;
        }

        /// <summary>
        /// Получить рейтинг
        /// </summary>
        /// <param name="index">Индекс рейтинга в коллекции</param>
        /// <returns>Рейтинг планеты от 1 до 10001 (0 если она отсутствует)</returns>
        public ushort GetRating(int index)
        {
            return (ushort)_values[index];
        }

        public void Dispose()
        {
            _values = null;
        }

        #endregion
    }
}
