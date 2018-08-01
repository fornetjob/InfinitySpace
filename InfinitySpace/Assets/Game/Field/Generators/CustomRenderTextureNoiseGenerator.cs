using Assets.Game.Access;
using Assets.Game.Access.Enums;
using Assets.Game.Field.Generators.Base;
using Assets.Game.Field.Generators.DataContracts;
using Assets.Game.Tools;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.Game.Field.Generators
{
    /// <summary>
    /// Генерация рейтингов планет с помощью фрагментного шейдера. 
    /// По идее, должен поддерживаться большинством платформ (если это не так, нужно добавить для недостающих платформ свои реализации NoiseGeneratorBase)
    /// </summary>
    public class CustomRenderTextureNoiseGenerator : NoiseGeneratorBase
    {
        #region Fields

        /// <summary>
        /// Текстуры для генерации
        /// </summary>
        private CustomRenderTexture[]
            _renderTextures;

        /// <summary>
        /// Текстуры, для получения результатов
        /// </summary>
        private Texture2D[]
            _textures;

        /// <summary>
        /// Для подсчёта итераций рендеринга
        /// </summary>
        private int
            _renderCount = 0;

        /// <summary>
        /// Кешировали итератор заданий на генерацию прямоугольников
        /// </summary>
        private IEnumerator<GenerateRectInfo>
            _iterator;

        /// <summary>
        /// Кешировали текущее состояние задания на генерацию прямоугольников
        /// </summary>
        private PixelsSliceData
            _sliceData = new PixelsSliceData();

        #endregion

        #region Game cicle

        void Awake()
        {
            _iterator = GetIterator();
            
            // На текущий момент используются две текстуры - одна на начальную инициализацию всего поля, а вторая для небольших фрагментов при изменении позиции игрока
            _renderTextures = new CustomRenderTexture[]
            {
                ResourcesAccess.Instance.GetRenderTexture(RenderTextureType.RenderTexture1000x1000),
                ResourcesAccess.Instance.GetRenderTexture(RenderTextureType.RenderTexture100x100)
            };

            for (int i = 0; i < _renderTextures.Length; i++)
            {
                var tex = _renderTextures[i];

                tex.initializationMode = CustomRenderTextureUpdateMode.Realtime;
                tex.updateMode = CustomRenderTextureUpdateMode.Realtime;
            }

            _textures = new Texture2D[_renderTextures.Length];

            for (int i = 0; i < _renderTextures.Length; i++)
            {
                var rendTex = _renderTextures[i];

                _textures[i] = new Texture2D(rendTex.width, rendTex.height, TextureFormat.ARGB32, false);
            }
        }

        void OnPreRender()
        {
            if (_renderCount > 0)
            {
                return;
            }

            if (_iterator.MoveNext() == false)
            {
                _renderCount = -1;

                return;
            }

            _renderCount = 0;

            var current = _iterator.Current;

            var pos = current.CurrentSliceData.GetPos();
            var rendText = _renderTextures[current.TextureIndex];
            var mat = rendText.material;

            // Изменяем координаты и добавляем прочие параметры, необходимые для генерации текущего куска прямоугольника
            mat.SetInt("CellSize", SettingsAccess.CellPxSize);
            mat.SetInt("Width", rendText.width);
            mat.SetInt("PosX", pos.x);
            mat.SetInt("PosY", pos.y);
        }

        void OnPostRender()
        {
            if (_renderCount == -1)
            {
                return;
            }

            // CustomRenderTexture иногда может не обновиться с первого раза
            // Как вариант - реализовать собственный плагин, для работы непосредственно с opengl  https://answers.unity.com/questions/465409/reading-from-a-rendertexture-is-slow-how-to-improv.html
            if (_renderCount < 1)
            {
                _renderCount++;

                return;
            }

            _renderCount = 0;

            // Получим сгенерированные для текущего куска пиксели, в которых находятся рейтинги планет, для текущего куска прямоугольника
            RenderTexture.active = _renderTextures[_iterator.Current.TextureIndex];

            var tex = _textures[_iterator.Current.TextureIndex];

            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            tex.Apply();

            var pixels = tex.GetPixels32();

            RenderTexture.active = null;

            // Добавим считанные пиксели, в которых находятся рейтинги планет, в текущее состояние задания
            _sliceData.ReloadValues(pixels);

            // Завершили текущий кусок прямоугольника
            OnSliceComplete();
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
            for (int textureIndex = 0; textureIndex < _renderTextures.Length; textureIndex++)
            {
                var rendTex = _renderTextures[textureIndex];

                if (RectIntTool.IsDevided(rect, rendTex.width, rendTex.height))
                {
                    return new GenerateRectInfo(rect, rendTex.width, rendTex.height, textureIndex, _sliceData);
                }
            }

            throw new System.ArgumentOutOfRangeException(string.Format("Невозможнно обработать данный прямоугольник {0}", rect));
        }

        #endregion
    }
}
