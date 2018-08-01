uint CalculateRating(int width, int cellSize, int posX, int posY, int id)
{
    int x = posX + id / cellSize % width;

    int yShift = id / cellSize / width;
    int y = posY + id % cellSize + yShift * cellSize;

    float hash = 10000 * sin(17.0 * x + y * 0.1 * (0.1f + abs(sin(x * 13.0f + y))));
    hash = hash - floor(hash) - 0.6;

    hash = max(0, hash) * 2.5;

    return hash * 10001;
}

uint CalcRating(int x, int y)
{
    float hash = 10000 * sin(17.0 * x + y * 0.1 * (0.1f + abs(sin(x * 13.0f + y))));
    hash = hash - floor(hash) - 0.6;

    hash = max(0, hash) * 2.5;

    return hash * 10001;
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