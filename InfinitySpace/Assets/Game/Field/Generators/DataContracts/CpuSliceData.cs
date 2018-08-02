using Assets.Game.Access;
using Assets.Game.Field.Generators.DataContracts.Base;
using Assets.Game.Tools;
using System;
using UnityEngine;

namespace Assets.Game.Field.Generators.DataContracts
{
    /// <summary>
    /// Текущее состояние куска задания, генерируемое на лету
    /// </summary>
    public class CpuSliceData : ISliceData
    {
        #region Fields

        /// <summary>
        /// Текущая позиция
        /// </summary>
        private Vector2Int
            _pos;

        /// <summary>
        /// Сид
        /// </summary>
        private uint
            _seed;

        #endregion

        #region Public methods

        /// <summary>
        /// Установить сид
        /// </summary>
        /// <param name="seed">Сид</param>
        public void SetSeed(uint seed)
        {
            _seed = seed;
        }

        /// <summary>
        /// Получить количество
        /// </summary>
        /// <returns>Количество рейтингов в куске</returns>
        public int GetCount()
        {
            return SettingsAccess.CellPxLenght;
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
        /// Получить рейтинг
        /// </summary>
        /// <param name="index">Индекс рейтинга в коллекции</param>
        /// <returns>Рейтинг планеты от 1 до 10001 (0 если она отсутствует)</returns>
        public ushort GetRating(int id)
        {
            return NoiseTool.GetRandomPlanetRating(_seed, _pos, id);
        }

        /// <summary>
        /// Обновить позицию
        /// </summary>
        /// <param name="pos">Позиция куска</param>
        public void ReloadPos(Vector2Int pos)
        {
            _pos = pos;
        }

        #endregion
    }
}
