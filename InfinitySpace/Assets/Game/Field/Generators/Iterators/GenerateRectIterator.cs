using Assets.Game.Field.Generators.DataContracts;

using System;
using System.Collections.Generic;

using System.Collections;

namespace Assets.Game.Field.Generators.Iterators
{
    /// <summary>
    /// Перечисление добавленных на генерацию прямоугольников
    /// </summary>
    public class GenerateRectIterator: IEnumerator<GenerateRectInfo>
    {
        #region Fields

        /// <summary>
        /// Список прямоугольников на генерации
        /// </summary>
        private Queue<GenerateRectInfo>
            _rects = new Queue<GenerateRectInfo>();

        /// <summary>
        /// Линтейный итератор ячеек поля
        /// </summary>
        private LinePositionIterator
            _line = null;

        /// <summary>
        /// Текущее задание
        /// </summary>
        private GenerateRectInfo
            _info;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавить задания на генерацию
        /// </summary>
        /// <param name="infos">Задания</param>
        /// <param name="clearExists">Удалить существующие</param>
        public void AppendToGenerate(GenerateRectInfo[] infos, bool clearExists)
        {
            if (clearExists)
            {
                _rects.Clear();
            }

            for (int i = 0; i < infos.Length; i++)
            {
                _rects.Enqueue(infos[i]);
            }
        }

        /// <summary>
        /// Получить текущий проегресс
        /// </summary>
        /// <returns>Прогресс от 0 до 1f</returns>
        public float GetProgress()
        {
            if (_line == null)
            {
                if (_rects.Count > 0)
                {
                    return 0f;
                }

                return 1f;
            }

            return _line.GetProgress();
        }

        #endregion

        #region IEnumerator implementation

        /// <summary>
        /// Текущее задание
        /// </summary>
        public GenerateRectInfo Current
        {
            get
            {
                return _info;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return _info;
            }
        }

        /// <summary>
        /// Следущая кусок в задании, либо новое задание
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_line != null
                || PrepareNextRect())
            {
                if (_line.MoveNext())
                {
                    return true;
                }

                Reset();
            }

            return false;
        }

        /// <summary>
        /// Сбросить текущее состояние
        /// </summary>
        public void Reset()
        {
            if (_line != null)
            {
                _line.Dispose();
            }

            _line = null;
            _info = null;
        }

        public void Dispose()
        {
            Reset();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Подготовить следущее задание
        /// </summary>
        /// <returns>Результат подготовки</returns>
        private bool PrepareNextRect()
        {
            if (_rects.Count == 0)
            {
                return false;
            }

            _info = _rects.Dequeue();

            _line = new LinePositionIterator(_info);

            return true;
        }

        #endregion
    }
}
