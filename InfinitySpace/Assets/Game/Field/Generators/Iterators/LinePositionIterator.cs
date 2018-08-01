using Assets.Game.Field.Generators.DataContracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.Field.Generators.Iterators
{
    /// <summary>
    /// Линейный итератор ячеек поля
    /// </summary>
    public class LinePositionIterator : IEnumerator<Vector2Int>
    {
        #region Fields

        /// <summary>
        /// Текущая позиция
        /// </summary>
        private Vector2Int
            _currentPos;

        /// <summary>
        /// Начальная позиция
        /// </summary>
        private Vector2Int
            _beginPos;

        /// <summary>
        /// Текущее задание
        /// </summary>
        private GenerateRectInfo
            _info;

        /// <summary>
        /// Текущий индекс
        /// </summary>
        private int 
            _currentIndex = 0;

        /// <summary>
        /// Количество
        /// </summary>
        public readonly int 
            _count;

        #endregion

        #region ctor

        public LinePositionIterator(GenerateRectInfo info)
        {
            _info = info;

            _beginPos = _info.Rect.position;

            _count = _info.SlicesSizeX * _info.SlicesSizeY;

            Reset();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получить текущий проегресс
        /// </summary>
        /// <returns>Прогресс от 0 до 1f</returns>
        public float GetProgress()
        {
            return _currentIndex / (float)(_count + 1);
        }

        #endregion

        #region IEnumerator implementation

        /// <summary>
        /// Текущая позиция
        /// </summary>
        public Vector2Int Current
        {
            get
            {
                return _currentPos;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return _currentPos;
            }
        }

        public void Dispose()
        {
            _info = null;
        }

        /// <summary>
        /// Следующий кусок
        /// </summary>
        /// <returns>Результат выполнения</returns>
        public bool MoveNext()
        {
            if (_currentIndex < _count)
            {
                int xPos = (_currentIndex * _info.SliceWidth) % _info.Rect.width;

                int yPos = ((_currentIndex * _info.SliceWidth) / _info.Rect.width) * _info.SliceHeight;

                _currentPos = _beginPos + new Vector2Int(xPos, yPos);

                _info.CurrentSliceData.ReloadPos(_currentPos);

                _currentIndex++;

                return true;
            }

            return false;
        }

        public void Reset()
        {
            _currentIndex = 0;
        }

        #endregion
    }
}
