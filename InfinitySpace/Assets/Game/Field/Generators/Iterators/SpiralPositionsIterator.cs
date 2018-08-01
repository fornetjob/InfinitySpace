using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Game.Field.Generators.Iterators
{
    /// <summary>
    /// Спиральный итератор ячеек поля
    /// </summary>
    public class SpiralPositionsIterator : IEnumerator<Vector2Int>
    {
        #region Statics

        /// <summary>
        /// Возвращает координаты по индексу в спирали
        /// </summary>
        /// <param name="index">Индекс в спирали</param>
        /// <returns>Координаты</returns>
        public static Vector2Int Spiral(int index)
        {
            if (index == 0)
            {
                return new Vector2Int(0, 0);
            }

            index--;

            var radius = Mathf.FloorToInt((Mathf.Sqrt(index + 1) - 1) / 2) + 1;

            var p = (8 * radius * (radius - 1)) / 2;

            var en = radius * 2;

            var a = (1 + index - p) % (radius * 8);

            Vector2Int pos = Vector2Int.zero;

            // Стороны: 0 top, 1 right, 2, bottom, 3 left
            switch (Mathf.FloorToInt(a / (radius * 2)))
            {
                case 0:
                    pos = new Vector2Int(a - radius, -radius);
                    break;
                case 1:
                    pos = new Vector2Int(radius, (a % en) - radius);
                    break;
                case 2:
                    pos = new Vector2Int(radius - (a % en), radius);
                    break;
                case 3:
                    pos = new Vector2Int(-radius, radius - (a % en));
                    break;
            }

            return pos;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Центр позиции
        /// </summary>
        private Vector2Int
            _centerPos;

        /// <summary>
        /// Текущий индекс
        /// </summary>
        private int
            _currentIndex;

        /// <summary>
        /// Размер позиции
        /// </summary>
        private int
            _size;

        /// <summary>
        /// Текущая позиция
        /// </summary>
        private Vector2Int
            _currentPos;

        #endregion

        #region Properties

        /// <summary>
        /// Длина
        /// </summary>
        public int Lenght;

        #endregion

        #region IEnumerator implementation

        /// <summary>
        /// Текущие координаты
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

        /// <summary>
        /// Получить текущий проегресс
        /// </summary>
        /// <returns>Прогресс от 0 до 1f</returns>
        public float GetProgress()
        {
            return _currentIndex / (float)(Lenght + 1);
        }

        /// <summary>
        /// Следущая позиция
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_currentIndex < Lenght)
            {
                _currentPos = (_centerPos + Spiral(_currentIndex)) * _size;

                _currentIndex++;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Сбросить значения и инициализировать итератор
        /// </summary>
        /// <param name="rect">Текущий прямоугольник</param>
        /// <param name="centerPos">Центр прямоугольника</param>
        /// <param name="size">Размер позиции</param>
        public void Reset(RectInt rect, Vector2Int centerPos, int size)
        {
            _size = size;

            var dividedRect = new RectInt(rect.x / _size, rect.y / _size, rect.width / _size, rect.height / _size);

            _centerPos = new Vector2Int(centerPos.x / _size, centerPos.y / _size);
            Lenght = dividedRect.width * dividedRect.height;

            _currentIndex = 0;
        }

        void IEnumerator.Reset()
        {
            
        }

        void IDisposable.Dispose()
        {
        }

        #endregion
    }
}
