using Assets.Game.Access;
using Assets.Game.Field.Generators.Base;
using Assets.Game.Field.Generators.DataContracts;
using Assets.Game.Tools;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.Game.Field.Generators
{
    /// <summary>
    /// Генерация рейтингов планет с помощью ComputedShader
    /// </summary>
    public class ComputedShaderNoiseGenerator : NoiseGeneratorBase
    {
        #region Constants

        /// <summary>
        /// Для Computed Shader практически не имеет значения размер создаваемого прямоугольника, поэтому ограничиваем его размер, исходя из здравого смысла 
        /// </summary>
        private const int MaxSliceSize = SettingsAccess.CellPxSize * 10;

        /// <summary>
        /// Количество потоков
        /// </summary>
        private const int ShaderThreadCount = 1;

        #endregion

        #region Fields

        /// <summary>
        /// Кешировали итератор заданий на генерацию прямоугольников
        /// </summary>
        private IEnumerator<GenerateRectInfo>
            _iterator;

        /// <summary>
        /// Устанавливаем размер буфера максимальным размером куска
        /// </summary>
        private static uint[]
            _data = new uint[MaxSliceSize * MaxSliceSize];

        /// <summary>
        /// Кешировали текущее состояние задания на генерацию прямоугольников
        /// </summary>
        private ArraySliceData
            _sliceData = new ArraySliceData(_data);

        /// <summary>
        /// Шейдер для генерации рейтингов
        /// </summary>
        private ComputeShader
            _shader;

        /// <summary>
        /// Буфер с рейтингами
        /// </summary>
        private ComputeBuffer
            _shaderBuffer;
        
        /// <summary>
        /// Ссылка на точку входа в шейдер
        /// </summary>
        private int
            _shaderKernel;

        #endregion

        #region Cicles

        void Awake()
        {
            _iterator = GetIterator();

            _shader = ResourcesAccess.Instance.CalculateShader;
            _shaderBuffer = new ComputeBuffer(_data.Length, 4);
            _shaderKernel = _shader.FindKernel("Calculate");

            _shader.SetInt("CellSize", SettingsAccess.CellPxSize);

            var renderTextures = ResourcesAccess.Instance.RenderTextures;

            for (int i = 0; i < renderTextures.Length; i++)
            {
                var tex = renderTextures[i];

                tex.initializationMode = CustomRenderTextureUpdateMode.OnDemand;
                tex.updateMode = CustomRenderTextureUpdateMode.OnDemand;

                tex.Release();
            }
        }

        void LateUpdate()
        {
            if (_iterator.MoveNext())
            {
                // Зададим условия для генерации рейтингов
                var current = _iterator.Current;
                var pos = current.CurrentSliceData.GetPos();

                _shader.SetInt("Width", current.SliceWidth);

                _shader.SetInt("PosX", pos.x);
                _shader.SetInt("PosY", pos.y);

                _shader.SetBuffer(_shaderKernel, "CellBuffer", _shaderBuffer);
                // Укажем реальный размер буфера к заполнению (SliceLenght)
                _shader.Dispatch(_shaderKernel, current.SliceLenght / ShaderThreadCount, 1, 1);

                // Получим рассчитаные рейтинги
                _shaderBuffer.GetData(_data);

                // Укажем реальный размер массива с рейтингами
                _sliceData.SetCount(current.SliceLenght);

                // Завершили текущий кусок прямоугольника
                OnSliceComplete();
            }
        }

        #endregion

        #region Overriden methods

        /// <summary>
        /// Создадим задание для прямоугольника
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <returns>Задание</returns>
        protected override GenerateRectInfo CreateInfo(RectInt rect)
        {
            // Прямоугольник должен делиться нацело на CellPxSize
            if (RectIntTool.IsDevided(rect, SettingsAccess.CellPxSize) == false)
            {
                throw new System.ArgumentOutOfRangeException("Невозможно обработать данный прямоугольник " + rect);
            }

            int sliceWidth;
            int sliceHeight;

            // Если прямоугольник делится нацело на максимальный размер - установим его
            if (RectIntTool.IsDevided(rect, MaxSliceSize))
            {
                sliceWidth = MaxSliceSize;
                sliceHeight = MaxSliceSize;
            }
            else
            {
                sliceWidth = SettingsAccess.CellPxSize;
                sliceHeight = SettingsAccess.CellPxSize;
            }

            return new GenerateRectInfo(rect, sliceWidth, sliceHeight, -1, _sliceData);
        }

        #endregion
    }
}
