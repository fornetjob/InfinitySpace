using Assets.Game.Access;
using Assets.Game.Field.Base;
using Assets.Game.Field.Generators.DataContracts;

using UnityEngine;

namespace Assets.Game.Field.Cells
{
    /// <summary>
    /// Колекция ячеек поля
    /// </summary>
    public class CellCollection
    {
        #region Fields
        /// <summary>
        /// Реиспользуемый массив для полной генерации мира вокруг игрока
        /// </summary>
        private ushort[][]
            _fullGenerationRatingsArray = new ushort[SettingsAccess.FullGeneratedCellsRadiusLength][];

        /// <summary>
        /// Ячейки
        /// </summary>
        public CellInfo[]
            _cells = new CellInfo[SettingsAccess.FieldLength];

        /// <summary>
        /// Текущий рейтинг игрока
        /// </summary>
        private ushort
            _playerRating;

        #endregion

        #region Ctor

        /// <summary>
        /// Создаёт коллекцию ячеек
        /// </summary>
        /// <param name="field">Поле</param>
        public CellCollection(ushort playerRating)
        {
            _playerRating = playerRating;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Заполнить ячейки сгенерированными данными
        /// </summary>
        /// <param name="info">Данные</param>
        public void FillRect(Vector2Int centerPos, GenerateRectInfo info)
        {
            CellInfo cell = null;

            var data = info.CurrentSliceData;

            var pos = data.GetPos();
            var count = data.GetCount();

            int index = 0;

            while (index < count)
            {
                ushort cellItemIndex = (ushort)(index % SettingsAccess.CellPxLenght);

                int cellIndex = index / SettingsAccess.CellPxLenght;

                int cellX = cellIndex % info.CellSizeX;
                int cellY = cellIndex / info.CellSizeY;

                cell = GetOrCreateCell(centerPos, pos + new Vector2Int(cellX, cellY) * SettingsAccess.CellPxSize);

                index = cell.FillRatings(data, index, _playerRating);
            }
        }

        /// <summary>
        /// Ячейка с такой позицией существует на поле
        /// </summary>
        /// <param name="cellPos">Позиция</param>
        /// <returns>Результат проверки</returns>
        public bool IsCellExist(Vector2Int cellPos)
        {
            var index = SettingsAccess.GetFieldIndex(cellPos);

            return _cells[index] != null;
        }
        
        /// <summary>
        /// Возвращает ячейку по её индексу
        /// </summary>
        /// <param name="cellPos"></param>
        /// <returns></returns>
        public CellInfo GetCell(Vector2Int cellPos)
        {
            var cell = _cells[SettingsAccess.GetFieldIndex(cellPos)];

            if (cell.Pos != cellPos)
            {
                return null;
            }

            return cell;
        }

        /// <summary>
        /// Ячейка генерируется полностью
        /// За размер генерирумой области вокруг игрока отвечает параметр <see cref="SettingsAccess.FullGeneratedCellsRadius"/>
        /// </summary>
        /// <param name="cellPos">Координаты ячейки</param>
        /// <returns>Результат проверки</returns>
        public bool IsFullGeneratedCell(Vector2Int centerPos, Vector2Int cellPos)
        {
            return GetFullGenerationIndex(centerPos, cellPos) != -1;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Создать или добавить ячейку на поле
        /// </summary>
        /// <param name="centerCellPos">Координаты центральной ячейки, для расчёта координат полностью генерируемых ячеек</param>
        /// <param name="cellPos">Координаты ячейки</param>
        /// <returns>Ячейка для заданной позиции</returns>
        private CellInfo GetOrCreateCell(Vector2Int centerCellPos, Vector2Int cellPos)
        {
            var index = SettingsAccess.GetFieldIndex(cellPos);

            CellInfo cell = _cells[index];

            if (cell == null)
            {
                cell = new CellInfo();

                _cells[index] = cell;
            }

            ushort[] ratings = null;

            // Если эта ячейка должна быть сгенерирована полностью, найдём для неё индекс реиспользуемого массива
            int visibleIndex = GetFullGenerationIndex(centerCellPos, cellPos);

            if (visibleIndex != -1)
            {
                ratings = _fullGenerationRatingsArray[visibleIndex];

                if (ratings == null)
                {
                    ratings = new ushort[SettingsAccess.CellPxLenght];

                    _fullGenerationRatingsArray[visibleIndex] = ratings;
                }
            }

            cell.InitCell(cellPos, ratings);

            return cell;
        }

        /// <summary>
        /// Возвращает индекс для полной генерации мира вокруг игрока <see cref="_fullGenerationRatingsArray"/>
        /// </summary>
        /// <returns>Индекс массива</returns>
        private int GetFullGenerationIndex(Vector2Int centerCellPosition, Vector2Int cellPos)
        {
            if (SettingsAccess.GetRadius(centerCellPosition, cellPos) <= SettingsAccess.FullGeneratedCellsRadius)
            {
                return Mathf.Abs(cellPos.x % SettingsAccess.FullGeneratedCellsRadiusSize + cellPos.y % SettingsAccess.FullGeneratedCellsRadiusSize * SettingsAccess.FullGeneratedCellsRadiusSize);
            }

            return -1;
        }

        #endregion
    }
}