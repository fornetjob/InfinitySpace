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
        public float FramesPerSecond = 10;

        #endregion

        #region Game

        void Update()
        {
            _img.sprite = Sprites[(int)(Time.time * FramesPerSecond) % Sprites.Length];
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Проигрывать анимацию
        /// </summary>
        public void Play()
        {
            _img.enabled = true;

            Update();

            gameObject.SetActive(true);
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
