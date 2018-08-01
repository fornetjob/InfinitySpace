using Assets.Game.Access;
using TMPro;
using UnityEngine;

namespace Assets.Game.UI.Controls
{
    /// <summary>
    /// Счётчик фпс
    /// </summary>
    public class FpsBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Накопленный за интервал ФПС
        /// </summary>
        private float
            _fpsCount = 0;

        /// <summary>
        /// Количество фреймов за интервал
        /// </summary>
        private int
            _framesCount = 0;

        /// <summary>
        /// Время
        /// </summary>
        private float
            _timeLeft;

        #endregion

        #region Properties

        /// <summary>
        /// Интервал
        /// </summary>
        public float UpdateInterval = 0.5F;

        /// <summary>
        /// Надпись
        /// </summary>
        public TextMeshProUGUI FpsText;

        #endregion

        #region Game cicle

        void Start()
        {
            _timeLeft = UpdateInterval;

            if (SettingsAccess.Instance.IsFpsCounterVisible == false)
            {
                gameObject.SetActive(false);
            }
        }

        void Update()
        {
            _timeLeft -= Time.deltaTime;
            _fpsCount += Time.timeScale / Time.deltaTime;

            _framesCount++;

            if (_timeLeft <= 0.0)
            {
                RefreshFpsText();

                _timeLeft = UpdateInterval;
                _fpsCount = 0.0F;
                _framesCount = 0;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Обновить надпись
        /// </summary>
        private void RefreshFpsText()
        {
            float fps = _fpsCount / _framesCount;

            FpsText.text = string.Format("FPS: {0:0}", fps);

            if (fps < 30)
            {
                FpsText.color = Color.yellow;
            }
            else if (fps < 10)
            {
                FpsText.color = Color.red;
            }
            else
            {
                FpsText.color = Color.green;
            }
        }

        #endregion
    }
}