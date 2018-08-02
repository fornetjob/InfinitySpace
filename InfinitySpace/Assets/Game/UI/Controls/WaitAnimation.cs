using Assets.Game.Core;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.UI.Controls
{
    /// <summary>
    /// Анимация ожидания
    /// </summary>
    public class WaitAnimation:MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Маппинг на картинку
        /// </summary>
        [Mapping(".")]
        [SerializeField]
        private Image
            _img;

        /// <summary>
        /// Массив картинок к анимации
        /// </summary>
        public Sprite[] Sprites;

        /// <summary>
        /// Кадров в секунду
        /// </summary>
        public float FramesPerValue = 180;

        #endregion

        #region Public methods

        public void OnProgress(float progress)
        {
            if (_img.enabled == false)
            {
                _img.enabled = true;

                gameObject.SetActive(true);
            }

            int spriteIndex = (int)(progress * FramesPerValue) % Sprites.Length;

            _img.sprite = Sprites[spriteIndex];
        }

        /// <summary>
        /// Завершить проигрывание анимации
        /// </summary>
        public void Stop()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}
