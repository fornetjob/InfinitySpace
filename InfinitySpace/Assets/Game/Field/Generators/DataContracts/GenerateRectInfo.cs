using Assets.Game.Access;
using Assets.Game.Field.Generators.DataContracts.Base;

using UnityEngine;

namespace Assets.Game.Field.Generators.DataContracts
{
    /// <summary>
    /// Задание на кусочную генерации в заданом прямоугольнике
    /// </summary>
    public class GenerateRectInfo
    {
        #region Ctor

        /// <summary>
        /// Конструктор задания
        /// </summary>
        /// <param name="rect">Прямоугольник для генерации</param>
        /// <param name="sliceWidth">Ширина куска</param>
        /// <param name="sliceHeight">Высота куска</param>
        /// <param name="textureIndex">Индекс текстуры (может быть нужен для быстрого выбора текстуры из списка)</param>
        /// <param name="currentSliceData">Текущее состояние задания</param>
        public GenerateRectInfo(RectInt rect, int sliceWidth, int sliceHeight, int textureIndex, ISliceData currentSliceData)
        {
            Rect = rect;
            SliceWidth = sliceWidth;
            SliceHeight = sliceHeight;
            SliceLenght = SliceWidth * SliceHeight;
            TextureIndex = textureIndex;
            SlicesSizeX = rect.width / SliceWidth;
            SlicesSizeY = rect.height / SliceHeight;
            CellSizeX = SliceWidth / SettingsAccess.CellPxSize;
            CellSizeY = SliceHeight / SettingsAccess.CellPxSize;
            CurrentSliceData = currentSliceData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Прямоугольник для генерации
        /// </summary>
        public readonly RectInt Rect;
        /// <summary>
        /// Ширина куска
        /// </summary>
        public readonly int SliceWidth;
        /// <summary>
        /// Высота куска
        /// </summary>
        public readonly int SliceHeight;
        /// <summary>
        /// Длина куска
        /// </summary>
        public readonly int SliceLenght;
        /// <summary>
        /// Индекс текстуры (может быть нужен для быстрого выбора текстуры из списка)
        /// </summary>
        public readonly int TextureIndex;
        /// <summary>
        /// Ширина в кусках
        /// </summary>
        public readonly int SlicesSizeX;
        /// <summary>
        /// Высота в кусках
        /// </summary>
        public readonly int SlicesSizeY;
        /// <summary>
        /// Ширина куска в ячейках
        /// </summary>
        public readonly int CellSizeX;
        /// <summary>
        /// Высота куска в ячейках
        /// </summary>
        public readonly int CellSizeY;
        /// <summary>
        /// Промежуточный результат выполнения
        /// </summary>
        public readonly ISliceData CurrentSliceData;

        #endregion
    }
}