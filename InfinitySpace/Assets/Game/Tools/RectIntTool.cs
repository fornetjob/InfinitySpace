using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Game.Tools
{
    /// <summary>
    /// Для работы с <see cref="RectInt"/>
    /// </summary>
    public static class RectIntTool
    {
        #region Public methods

        /// <summary>
        /// Проверка сторон
        /// </summary>
        /// <param name="size">Сторона для проверки</param>
        /// <param name="sliceSize">Размер куска</param>
        /// <returns>Сторона делится нацело на куски</returns>
        public static bool IsDevided(int size, int sliceSize)
        {
            var devide = size / (float)sliceSize;

            return devide == Mathf.FloorToInt(devide);
        }

        /// <summary>
        /// Проверка прямоугольника
        /// </summary>
        /// <param name="rect">Прямоугольник для проверки</param>
        /// <param name="sliceSize">Размер куска</param>
        /// <returns>Стороны прямоугольника делятся нацело на куски</returns>
        public static bool IsDevided(RectInt rect, int sliceSize)
        {
            return IsDevided(rect.width, sliceSize)
                && IsDevided(rect.height, sliceSize);
        }

        /// <summary>
        /// Проверка прямоугольника
        /// </summary>
        /// <param name="rect">Прямоугольник для проверки</param>
        /// <param name="sliceWidth">Ширина куска</param>
        /// <param name="sliceHeight">Высота куска</param>
        /// <returns>Стороны прямоугольника делятся нацело на куски</returns>
        public static bool IsDevided(RectInt rect, int sliceWidth, int sliceHeight)
        {
            return IsDevided(rect.width, sliceWidth)
                && IsDevided(rect.height, sliceHeight);
        }

        /// <summary>
        /// Вычитание прямоугольников
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <param name="subtract">Вычитаемый прямоугольник</param>
        /// <returns>До 4-х прямоугольников, описывающие область первого прямоугольника, которая не входит во второй</returns>
        public static List<RectInt> Subtract(RectInt rect, RectInt subtract)
        {
            List<RectInt> results = new List<RectInt>();

            if (IsIntersect(rect, subtract) == false)
            {
                return results;
            }

            var top = new RectInt(rect.xMin, rect.yMin, rect.width, subtract.yMin - rect.yMin);

            if (top.IsEmpty() == false)
            {
                results.Add(top);
            }

            int topHeight = Math.Max(top.height, 0);

            var left = new RectInt(rect.xMin, rect.yMin + topHeight, subtract.xMin - rect.xMin, rect.height - topHeight);

            if (left.IsEmpty() == false)
            {
                results.Add(left);
            }

            var leftWidth = Math.Max(left.width, 0);

            var rightWidth = rect.xMax - subtract.xMax;

            var right = new RectInt(rect.xMax - rightWidth, rect.yMin + topHeight, rightWidth, rect.height - topHeight);

            rightWidth = Math.Max(rightWidth, 0);

            if (right.IsEmpty() == false)
            {
                results.Add(right);
            }

            var bottomHeight = rect.yMax - subtract.yMax;

            var bottom = new RectInt(rect.xMin + leftWidth, rect.yMax - bottomHeight, rect.width - leftWidth - rightWidth, bottomHeight);

            if (bottom.IsEmpty() == false)
            {
                results.Add(bottom);
            }

            return results;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Прямоугольники пересекаются
        /// </summary>
        /// <param name="rect1">Первый прямоугольник</param>
        /// <param name="rect2">Второй прямоугольник</param>
        /// <returns>Результат проверки</returns>
        private static bool IsIntersect(RectInt rect1, RectInt rect2)
        {
            return (rect1.x >= rect2.xMax || rect1.xMax <= rect2.x || rect1.y >= rect2.yMax || rect1.yMax <= rect2.y) == false;
        }

        /// <summary>
        /// Некорректный или пустой прямоугольник
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <returns>Результат проверки</returns>
        private static bool IsEmpty(this RectInt rect)
        {
            return rect.height < 1 || rect.width < 1;
        }

        #endregion
    }
}
