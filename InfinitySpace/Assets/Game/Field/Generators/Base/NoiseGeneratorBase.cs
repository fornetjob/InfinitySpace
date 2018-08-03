using Assets.Game.Field.Base;
using Assets.Game.Field.Generators.DataContracts;
using Assets.Game.Field.Generators.Iterators;
using Assets.GameDebug;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Game.Field.Generators.Base
{
    /// <summary>
    /// Базовый генератор
    /// </summary>
    public abstract class NoiseGeneratorBase : MonoBehaviour, IAsyncCollectionProcess<GenerateRectInfo>
    {
        #region Fields

        /// <summary>
        /// Итератор заданий
        /// </summary>
        private GenerateRectIterator
            _rectIterator = new GenerateRectIterator();

        /// <summary>
        /// Отладка
        /// </summary>
        private IGenerationDebug
            _debug;

        #endregion

        #region Events

        /// <summary>
        /// Выполнение задания завершено
        /// </summary>
        public event Action<GenerateRectInfo> OnProcessEnd;

        #endregion

        #region Abstract methods

        /// <summary>
        /// Создадим задание для прямоугольника
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <returns>Задание</returns>
        protected abstract GenerateRectInfo CreateInfo(RectInt rect);

        #endregion

        #region Public methods

        /// <summary>
        /// Добавить отладчик
        /// </summary>
        /// <param name="debug">Отладчик</param>
        /// <returns></returns>
        public NoiseGeneratorBase SetDebug(IGenerationDebug debug)
        {
            _rectIterator.SetDebug(debug);

            return this;
        }

        /// <summary>
        /// Добавить задание на генерацию поля
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <param name="clearExists">Удалить существующие задания</param>
        public void AppendRect(RectInt rect, bool clearExists = false)
        {
            var list = new List<RectInt>();
            list.Add(rect);

            AppendRects(list, clearExists);
        }

        /// <summary>
        /// Добавить задания на генерацию поля
        /// </summary>
        /// <param name="rects">Прямоугольники</param>
        /// <param name="clearExists">Удалить существующие задания</param>
        public void AppendRects(List<RectInt> rects, bool clearExists = false)
        {
            var infos = new GenerateRectInfo[rects.Count];

            for (int i = 0; i < rects.Count; i++)
            {
                infos[i] = CreateInfo(rects[i]);
            }

            _rectIterator.AppendToGenerate(infos, clearExists);
        }

        /// <summary>
        /// Получить текущий прогресс генерации
        /// </summary>
        /// <returns>Прогресс от 0 до 1f</returns>
        public float GetProgress()
        {
            return _rectIterator.GetProgress();
        }

        #endregion

        #region Protected methods

        protected uint GetNewSeed()
        {
            return (uint)UnityEngine.Random.Range(0, int.MaxValue);
        }

        /// <summary>
        /// Генерация куска завершена
        /// </summary>
        protected void OnSliceComplete()
        {
            if (OnProcessEnd != null)
            {
                OnProcessEnd(_rectIterator.Current);
            }
        }

        /// <summary>
        /// Получить итератор заданий
        /// </summary>
        /// <returns>Итератор активных заданий</returns>
        protected IEnumerator<GenerateRectInfo> GetIterator()
        {
            return _rectIterator;
        }

        #endregion
    }
}
