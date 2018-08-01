using System;
using System.Collections.Generic;

using Assets.Game.Access;
using Assets.Game.Field.Base;
using Assets.Game.Field.Generators.Iterators;
using Assets.Game.Tools;

using UnityEngine;

namespace Assets.Game.Field.Cells
{
    /// <summary>
    /// Находит первые <see cref="SettingsAccess.MaxAdvancedVisiblePlanet"/> отсортированных ячеек в области зума
    /// </summary>
    public class SortedCellsVisitor: MonoBehaviour, IAsyncCollectionProcess<SortedCellItem[]>
    {
        #region Constants

        /// <summary>
        /// Максимальный размер куска
        /// </summary>
        private const int MaxSliceSize = 1000;

        /// <summary>
        /// Минимальный размер куска
        /// </summary>
        private const int MinSliceSize = 100;

        #endregion

        #region Fields

        /// <summary>
        /// Коллекция
        /// </summary>
        private CellCollection
            _cells;

        /// <summary>
        /// Выбранный размер куска
        /// </summary>
        private int
            _sliceLenght;

        /// <summary>
        /// Текущая ячейка
        /// </summary>
        private CellInfo
            _cell;

        /// <summary>
        /// Размер области 1 или <see cref="SettingsAccess.CellPxSize"/>, если мы используемые кешированные в ячейках результаты первых <see cref="SettingsAccess.MaxAdvancedVisiblePlanet"/> результатов
        /// </summary>
        private int
            _size;

        /// <summary>
        /// Рейтинг игрока
        /// </summary>
        private ushort
            _playerRating;

        /// <summary>
        /// Сортируемые позиции
        /// </summary>
        private SortedCellItems
            _sortedCells = new SortedCellItems();

        /// <summary>
        /// Итератор позиций по спирали
        /// </summary>
        private SpiralPositionsIterator
            _iterator;

        /// <summary>
        /// Процесс завершён
        /// </summary>
        private bool
            _isVisitEnded;

        private bool
            _isWaitCellCreation;

        #endregion

        #region Game

        void Update()
        {
            if (_isVisitEnded == false)
            {
                VisitNext();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Инициализация визитора
        /// </summary>
        /// <param name="playerRating">Рейтинг игрока</param>
        /// <returns></returns>
        public SortedCellsVisitor Init(CellCollection cells, ushort playerRating)
        {
            _cells = cells;
            _iterator = new SpiralPositionsIterator();

            _playerRating = playerRating;

            return this;
        }

        #endregion

        #region IAsyncCollectionProcess implementation

        /// <summary>
        /// Выполнение задания завершено
        /// </summary>
        public event Action<SortedCellItem[]> OnProcessEnd;

        /// <summary>
        /// Установить связь с коллекцией
        /// </summary>
        /// <param name="cells">Коллекция с ячейками</param>
        public void BindCollection(CellCollection cells)
        {
            _cells = cells;
        }

        /// <summary>
        /// Добавить задание
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <param name="clearExists">Удалить существующие задания</param>
        public void AppendRect(RectInt rect, bool clearExists = false)
        {
            if (RectIntTool.IsDevided(rect, SettingsAccess.CellPxSize)
                && rect.width != SettingsAccess.CellPxSize)
            {
                _size = SettingsAccess.CellPxSize;
            }
            else
            {
                _size = 1;
            }

            var centerPos = new Vector2Int(Mathf.FloorToInt(rect.center.x), Mathf.FloorToInt(rect.center.y));

            _iterator.Reset(rect, centerPos, _size);

            var length = _iterator.Lenght;

            _sliceLenght = length;

            _sortedCells.Clear();

            _isVisitEnded = false;
        }

        void IAsyncCollectionProcess<SortedCellItem[]>.AppendRects(List<RectInt> rects, bool clearExists = false)
        {
            throw new NotImplementedException();
        }

        float IAsyncCollectionProcess<SortedCellItem[]>.GetProgress()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Сортирует следущие <see cref="_sliceLenght"/> позиций по координатам из <see cref="_iterator"/>
        /// </summary>
        private void VisitNext()
        {
            if (_isVisitEnded)
            {
                return;
            }

            for (int i = 0; i < _sliceLenght; i++)
            {
                if (_isWaitCellCreation == false
                    && _iterator.MoveNext() == false)
                {
                    _isVisitEnded = true;

                    if (OnProcessEnd != null)
                    {
                        OnProcessEnd(_sortedCells.GetValues());
                    }

                    return;
                }

                if (_size == SettingsAccess.CellPxSize)
                {
                    _cell = _cells.GetCell(_iterator.Current);

                    if (_cell == null)
                    {
                        _isWaitCellCreation = true;

                        Debug.Log(_iterator.Current);

                        return;
                    }

                    _sortedCells.Append(_cell.SortedCellItems);
                }
                else
                {
                    var cellPos = SettingsAccess.GetCellBeginPosition(_iterator.Current);

                    if (_cell == null
                        || _cell.Pos != cellPos)
                    {
                        _cell = _cells.GetCell(cellPos);
                    }

                    if (_cell == null)
                    {
                        _isWaitCellCreation = true;

                        return;
                    }

                    var cellItemPos = _cell.GetCellItemPosition(_iterator.Current);

                    ushort index = _cell.GetCellItemIndex(cellItemPos);

                    var rating = _cell.GetRating(index);

                    if (rating > 0)
                    {
                        var ratingDistance = (ushort)Mathf.Abs(rating - _playerRating);

                        if (_sortedCells.IsBelowThanMaxRatingDistance(ratingDistance))
                        {
                            _sortedCells.AppendCellItemFast(new SortedCellItem(cellPos, index, rating, ratingDistance));
                        }
                    }
                }
            }
        }

        #endregion
    }
}
