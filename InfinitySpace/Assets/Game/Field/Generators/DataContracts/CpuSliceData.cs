using Assets.Game.Access;
using Assets.Game.Field.Generators.DataContracts.Base;
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
        private int
            _seed;

        #endregion

        #region Public methods

        /// <summary>
        /// Установить сид
        /// </summary>
        /// <param name="seed">Сид</param>
        public void SetSeed(int seed)
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
            const int PrimeX = 1619;
            const int PrimeY = 31337;
            const int HashMod = 30000;

            int x = SettingsAccess.WorldSize + _pos.x + id % SettingsAccess.CellPxSize;
            int y = SettingsAccess.WorldSize + _pos.y + id / SettingsAccess.CellPxSize;

            int hash = _seed;

            hash += x * PrimeX;
            hash += y * PrimeY;
            hash = (hash * hash * 60493) % HashMod + 1;

            if (hash >= SettingsAccess.MaxRating)
            {
                hash = 0;
            }

            return (ushort)hash;
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
