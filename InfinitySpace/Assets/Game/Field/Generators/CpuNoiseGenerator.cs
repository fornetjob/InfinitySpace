using System;
using Assets.Game.Field.Generators.Base;
using Assets.Game.Field.Generators.DataContracts;
using Assets.Game.Access;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.Game.Field.Generators
{
    /// <summary>
    /// Генерация рейтингов планет на цпу. 
    /// Платформонезависимая генерация
    /// </summary>
    public class CpuNoiseGenerator : NoiseGeneratorBase
    {
        #region Constants

        /// <summary>
        /// Максимальное количество итераций генерации за раз
        /// </summary>
        private const int SliceCount = 50;

        #endregion

        #region Fields

        /// <summary>
        /// Кешировали итератор заданий на генерацию прямоугольников
        /// </summary>
        private IEnumerator<GenerateRectInfo>
            _iterator;

        /// <summary>
        /// Кешировали текущее состояние задания на генерацию прямоугольников
        /// </summary>
        private CpuSliceData
            _data = new CpuSliceData();

        #endregion

        #region Game

        void Awake()
        {
            _iterator = GetIterator();

            _data.SetSeed(GetNewSeed());
        }

        void Update()
        {
            for (int i = 0; i < SliceCount; i++)
            {
                if (_iterator.MoveNext())
                {
                    OnSliceComplete();
                }
                else
                {
                    break;
                }
            }
        }

        #endregion

        #region Overriden methods

        /// <summary>
        /// Создадим задание для прямоугольника
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <returns>Задание</returns>
        protected override GenerateRectInfo CreateInfo(RectInt rect)
        {
            return new GenerateRectInfo(rect, SettingsAccess.CellPxSize, SettingsAccess.CellPxSize, -1, _data);
        }

        #endregion
    }
}