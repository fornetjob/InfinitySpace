using Assets.Game.Field.Cells;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Game.Field.Base
{
    /// <summary>
    /// Интерфейс для добавления заданий для коллекции
    /// </summary>
    public interface IAsyncCollectionProcess<TResult>
    {
        /// <summary>
        /// Выполнение задания завершено
        /// </summary>
        event Action<TResult> OnProcessEnd;
        /// <summary>
        /// Добавить задание
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <param name="clearExists">Удалить существующие задания</param>
        void AppendRect(RectInt rect, bool clearExists = false);
        /// <summary>
        /// Добавить пакетное задание
        /// </summary>
        /// <param name="rects">Прямоугольник</param>
        /// <param name="clearExists">Удалить существующие задания</param>
        void AppendRects(List<RectInt> rects, bool clearExists = false);
        /// <summary>
        /// Получить текущий проегресс
        /// </summary>
        /// <returns>Прогресс от 0 до 1f</returns>
        float GetProgress();
    }
}
