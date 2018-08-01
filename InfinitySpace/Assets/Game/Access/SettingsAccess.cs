using Assets.Game.Access.Base;

using UnityEngine;

namespace Assets.Game.Access
{
    /// <summary>
    /// Доступ к настройкам поля
    /// Для ускорения переменные выполнены в виде констант, можно перевести на доступ через инстанцирование
    /// </summary>
    [CreateAssetMenu(fileName = "SettingsAccess", menuName = "Access/SettingsAccess")]
    public class SettingsAccess: AccessBase<SettingsAccess>
    {
        #region Constants

        /// <summary>
        /// Максимальный рейтинг
        /// </summary>
        public const ushort MaxRating = 10002;

        /// <summary>
        /// Количество видимых игроку планет, в расширенном режиме
        /// </summary>
        public const int MaxAdvancedVisiblePlanet = 20;

        /// <summary>
        /// Размер ячейки в пикселях
        /// </summary>
        public const byte CellPxSize = 100;

        /// <summary>
        /// Половина ячейки в пикселях
        /// </summary>
        public const byte HalfCellPxSize = CellPxSize / 2;

        /// <summary>
        /// Размер поля в ячейках
        /// </summary>
        public const int FieldSize = 110;

        /// <summary>
        /// Общее количество ячеек на поле
        /// </summary>
        public const int FieldLength = FieldSize * FieldSize;

        /// <summary>
        /// Размер поля в пикселях
        /// </summary>
        public const int FieldSizePx = FieldSize * CellPxSize;

        /// <summary>
        /// Длина поля в пикселях
        /// </summary>
        public const int FieldLengthPx = FieldSizePx * FieldSizePx;
        
        /// <summary>
        /// Размер мира в пикселях (после которого он начинает повторяться)
        /// </summary>
        public const int WorldSize = 1000000;

        /// <summary>
        /// Половина размера мира
        /// </summary>
        public const int HalfWordSize = WorldSize / 2;

        /// <summary>
        /// Половина размера поля
        /// </summary>
        public const int HalfFieldSize = FieldSize / 2;

        /// <summary>
        /// Полное количество пикселей в ячейке
        /// </summary>
        public const ushort CellPxLenght = CellPxSize * CellPxSize;

        /// <summary>
        /// Размер одной ячейки в мировых координатах
        /// </summary>
        public const float CellWorldSize = 1f;

        /// <summary>
        /// Полностью сгенерированный радиус ячеек (размером <see cref="CellPxSize"/>) вокруг игрока
        /// </summary>
        public const int FullGeneratedCellsRadius = 1;

        /// <summary>
        /// Количество окружающих игрока ячеек, в которых мы сохраняем информацию о всех планетах
        /// </summary>
        public static readonly int FullGeneratedCellsRadiusLength = RadiusToLength(FullGeneratedCellsRadius);

        /// <summary>
        /// Длина полностью сгенерированного мира вокруг игрока
        /// </summary>
        public static readonly int FullGeneratedCellsRadiusSize = (int)Mathf.Sqrt(FullGeneratedCellsRadiusLength);

        #endregion

        #region Statics

        /// <summary>
        /// Возвращает прямоугольник поля по его центральной ячейке
        /// </summary>
        /// <returns>Прямоугольник</returns>
        public static RectInt GetFieldRectPx(Vector2Int centerCellPos)
        {
            return new RectInt(centerCellPos + new Vector2Int(HalfFieldSize * -1, HalfFieldSize * -1) * CellPxSize, Vector2Int.one * FieldSizePx);
        }

        /// <summary>
        /// Возвращает генерируемый прямоугольник вокруг игрока, который генерируется полностью
        /// </summary>
        /// <returns>Видимый прямоугольник</returns>
        public static RectInt GetFullGenerationRect(Vector2Int centerPos)
        {
            var beginPos = centerPos + new Vector2Int(Mathf.FloorToInt(FullGeneratedCellsRadiusSize / -2), Mathf.FloorToInt(FullGeneratedCellsRadiusSize / -2)) * CellPxSize;

            return new RectInt(beginPos, Vector2Int.one * FullGeneratedCellsRadiusSize * CellPxSize);
        }

        /// <summary>
        /// Возвращает случайную ячейку на поле
        /// </summary>
        /// <returns>Случайная ячейка</returns>
        public static Vector2Int GetRandomFieldPosition()
        {
            int x = Random.Range(HalfFieldSize * -1, HalfFieldSize);
            int y = Random.Range(HalfFieldSize * -1, HalfFieldSize);

            return new Vector2Int(x, y) * CellPxSize;
        }

        /// <summary>
        /// Возвращает случайную позицию в ячейке
        /// </summary>
        /// <param name="cellPos">Ячейка</param>
        /// <returns>Случайная позиция</returns>
        public static Vector2Int GetRandomCellPosition(Vector2Int cellPos)
        {
            byte x = (byte)Random.Range(0, CellPxSize);
            int y = (byte)Random.Range(0, CellPxSize);

            return new Vector2Int(cellPos.x + x, cellPos.y + y);
        }

        /// <summary>
        /// Возвращает радиус между двумя точками
        /// </summary>
        /// <param name="posFrom">Первая точка</param>
        /// <param name="posTo">Вторая точка</param>
        /// <returns>Радиус</returns>
        public static int GetRadius(Vector2Int posFrom, Vector2Int posTo)
        {
            var pos = (posFrom - posTo);

            return Mathf.Max(Mathf.Abs(pos.x / CellPxSize), Mathf.Abs(pos.y / CellPxSize));
        }

        /// <summary>
        /// Возвращает длину спирали для указанного радиуса
        /// </summary>
        /// <param name="radius">Радиус</param>
        /// <returns>Длина спирали</returns>
        public static int RadiusToLength(int radius)
        {
            return (int)Mathf.Pow(3 + 2 * (radius - 1), 2);
        }

        /// <summary>
        /// Возвращает ячейку, содержащую данную позицию
        /// </summary>
        /// <param name="cellItemPosition">Позиция на поле</param>
        /// <returns>Ячейка</returns>
        public static Vector2Int GetCellBeginPosition(Vector2Int cellItemPosition)
        {
            cellItemPosition[0] = Mathf.CeilToInt((cellItemPosition[0] - CellPxSize + 1) / (float)CellPxSize) * CellPxSize;
            cellItemPosition[1] = Mathf.CeilToInt((cellItemPosition[1] - CellPxSize + 1) / (float)CellPxSize) * CellPxSize;

            return cellItemPosition;
        }

        /// <summary>
        /// Получить мировые координаты для позиции
        /// </summary>
        /// <param name="pos">Позиция</param>
        /// <param name="zoomPrecision">Зум</param>
        /// <returns>Мировые координаты</returns>
        public static Vector3 ConvertToWorldPosition(Vector2Int pos, float zoomPrecision = 1)
        {
            return new Vector3((pos.x + 0.5f) * CellWorldSize * zoomPrecision, (pos.y + 0.5f) * CellWorldSize * zoomPrecision);
        }

        /// <summary>
        /// Возвращает индекс ячейки
        /// </summary>
        /// <param name="cellPos">Позиция ячейки</param>
        /// <returns>Индекс</returns>
        public static uint GetFieldIndex(Vector2Int cellPos)
        {
            cellPos = new Vector2Int(cellPos.x / CellPxSize, cellPos.y / CellPxSize);

            cellPos += new Vector2Int(HalfFieldSize, HalfFieldSize);

            var index = (cellPos.x + cellPos.y * FieldSize) % FieldLength;

            if (index < 0)
            {
                index += FieldLength;
            }

            return (uint)index;
        }

        /// <summary>
        /// Возвращает уникальный идентификатор позиции ячеки
        /// </summary>
        /// <param name="cellPos">Позиция ячейки</param>
        /// <returns>Уникальный идентификатор</returns>
        public static int GetId(Vector2Int cellPos)
        {
            return cellPos.x + cellPos.y * WorldSize + 1;
        }

        #endregion

        #region Properties

        public bool IsFpsCounterVisible;

        #endregion
    }
}
