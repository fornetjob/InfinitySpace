using Assets.Game.Access;
using UnityEngine;

namespace Assets.Game.Tools
{
    public static class NoiseTool
    {
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

        public static ushort GetRandomPlanetRating(uint seed, Vector2Int pos, int id)
        {
            const int PrimeX = 1619;
            const int PrimeY = 31337;
            const int HashMod = 32768;

            uint x = (uint)(SettingsAccess.WorldSize + pos.x + id % SettingsAccess.CellPxSize);
            uint y = (uint)(SettingsAccess.WorldSize + pos.y + id / SettingsAccess.CellPxSize);

            uint hash = seed;

            hash += x * PrimeX;
            hash += y * PrimeY;
            hash = (hash * hash * 60493) % HashMod + 1;

            if (hash >= SettingsAccess.MaxRating)
            {
                hash = 0;
            }

            return (ushort)hash;
        }
    }
}
