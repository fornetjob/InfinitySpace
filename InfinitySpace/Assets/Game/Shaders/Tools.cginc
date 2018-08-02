uint CalcRating(uint seed, uint id, int posX, int posY)
{
    const uint WorldSize = 1000000;
    const uint PrimeX = 1619;
    const uint PrimeY = 31337;
    const uint HashMod = 32768;
    const uint MaxRating = 10001;
    const uint CellSize = 100;
    const int CellLenght = 100 * 100;

    uint cellPos = (id / CellLenght);

    uint cellX = (cellPos % 10) * CellSize;
    uint cellY = (cellPos / 10) * CellSize;

    uint sId = id % CellLenght;

    uint x = WorldSize + posX + cellX + sId % CellSize;
    uint y = WorldSize + posY + cellY + (sId / CellSize) % CellSize;

    uint hash = seed;

    hash += x * PrimeX;
    hash += y * PrimeY;
    hash = (hash * hash * 60493) % HashMod;

    hash *= step(hash, MaxRating);

    return hash;
}

int2 Spiral(int index)
{
    if (index > 0)
    {
        index--;

        int radius = floor((sqrt(index + 1) - 1) / 2) + 1;

        int p = (8 * radius * (radius - 1)) / 2;

        int en = radius * 2;

        int a = (1 + index - p) % (radius * 8);

        // Стороны: 0 top, 1 right, 2, bottom, 3 left
        int f = floor(a / (radius * 2));

        if (f == 0)
        {
            return int2(a - radius, -radius);
        }
        else if (f == 1)
        {
            return int2(radius, (a % en) - radius);
        }
        else if (f == 2)
        {
            return int2(radius - (a % en), radius);
        }
        else if (f == 3)
        {
            return int2(-radius, radius - (a % en));
        }
    }

    return int2(0, 0);
}