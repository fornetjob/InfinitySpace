using UnityEngine;

namespace Assets.Game.Field.Generators.DataContracts.Base
{
    /// <summary>
    /// Текущее состояние куска задания
    /// </summary>
    public interface ISliceData
    {
        /// <summary>
        /// Обновить позицию
        /// </summary>
        /// <param name="pos">Позиция куска</param>
        void ReloadPos(Vector2Int pos);
        /// <summary>
        /// Получить текущую позицию куска
        /// </summary>
        /// <returns>Текущая позиция</returns>
        Vector2Int GetPos();

        /// <summary>
        /// Получить количество
        /// </summary>
        /// <returns>Количество рейтингов в куске</returns>
        int GetCount();

        /// <summary>
        /// Получить рейтинг
        /// </summary>
        /// <param name="index">Индекс рейтинга в коллекции</param>
        /// <returns>Рейтинг планеты от 1 до 10001 (0 если она отсутствует)</returns>
        ushort GetRating(int index);
    }
}
