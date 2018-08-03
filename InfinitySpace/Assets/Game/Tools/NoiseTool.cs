using Assets.Game.Access;

using UnityEngine;

namespace Assets.Game.Tools
{
    /// <summary>
    /// Генератор шума
    /// </summary>
    public static class NoiseTool
    {
        /// <summary>
        /// Возвращает случайный рейтинг игрока
        /// </summary>
        /// <returns></returns>
        public static ushort GetRandomPlayerRating()
        {
            uint seed = (uint)Random.Range(0, int.MaxValue);

            int index = 0;

            while (index < 100)
            {
                ushort rating = GetRandomPlanetRating(seed, Vector2Int.zero, index);

                if (rating > 0)
                {
                    return rating;
                }

                index++;
            }

            return (ushort)Random.Range(1, SettingsAccess.MaxRating);
        }

        /// <summary>
        /// Возвращает рейтинг планеты
        /// </summary>
        /// <param name="seed">Сид</param>
        /// <param name="pos">Ячейка</param>
        /// <param name="id">Позиция</param>
        /// <returns></returns>
        public static ushort GetRandomPlanetRating(uint seed, Vector2Int pos, int id)
        {
            const int PrimeX = 1619;
            const int PrimeY = 31337;
            const int HashMod = 30000;

            uint x = (uint)(SettingsAccess.WorldSize + pos.x + id % SettingsAccess.CellPxSize);
            uint y = (uint)(SettingsAccess.WorldSize + pos.y + id / SettingsAccess.CellPxSize);

            uint hash = seed;

            hash += x * PrimeX;
            hash += y * PrimeY;
            hash = (hash * hash * hash * 60493) % HashMod + 1;

            if (hash >= SettingsAccess.MaxRating)
            {
                hash = 0;
            }

            return (ushort)hash;
        }
    }
}
