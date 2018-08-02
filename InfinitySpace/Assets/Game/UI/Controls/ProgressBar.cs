using Assets.Game.Core;
using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.UI.Controls
{
    /// <summary>
    /// Прогресс
    /// </summary>
    public class ProgressBar : MonoBehaviour
    {
        #region Mappings

        /// <summary>
        /// Маппинг на анимацию прогресса
        /// </summary>
        [Mapping(AbsolutePath= "Canvas/WaitAnimation")]
        [SerializeField]
        public WaitAnimation
            _animation;

        /// <summary>
        /// Маппинг на значение прогресса
        /// </summary>
        [Mapping]
        [SerializeField]
        public Image
            _value;

        /// <summary>
        /// Маппинг на текст прогресса
        /// </summary>
        [Mapping]
        [SerializeField]
        public TextMeshProUGUI
            _text;

        #endregion

        #region Fields

        /// <summary>
        /// Текущее значение
        /// </summary>
        private float
            _currentValue;

        /// <summary>
        /// Следующее значение
        /// </summary>
        private float
            _moveToValue;

        /// <summary>
        /// Текст прогресса
        /// </summary>
        private string
            _progressText;

        /// <summary>
        /// Количество точек в тексте справа и слева
        /// </summary>
        private int
            _dotCount;

        /// <summary>
        /// Ссылка на функцию для получения текущего состояния
        /// </summary>
        private Func<float>
            _getValueFunc;

        /// <summary>
        /// Возврат по окончании прогресса
        /// </summary>
        private Action
            _callback;

        /// <summary>
        /// Задержка между увеличением количества точек в тесте
        /// </summary>
        private CheckDelay
            _dotWait = new CheckDelay(1000);

        #endregion

        #region Game

        void Update()
        {
            if (_getValueFunc == null)
            {
                Stop();

                return;
            }

            _moveToValue = Math.Min(1, _getValueFunc());

            if (_moveToValue > _currentValue)
            {
                _currentValue += Time.deltaTime * 2;

                _currentValue = Mathf.Min(_currentValue, _moveToValue);

                RefreshValueImage();

                if (_currentValue == 1)
                {
                    Stop();

                    return;
                }
            }

            if (_dotWait.Check())
            {
                _dotCount++;

                if (_dotCount > 3)
                {
                    _dotCount = 0;
                }

                RefreshText();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Приступить
        /// </summary>
        /// <param name="getValueFunc">Ссылка на функцию для получения текущего состояния</param>
        /// <param name="progressText">Текст прогресса</param>
        /// <param name="callback">Возврат по окончании прогресса</param>
        public void Begin(Func<float> getValueFunc, string progressText, Action callback = null)
        {
            _getValueFunc = getValueFunc;
            _callback = callback;

            gameObject.SetActive(true);

            _dotCount = 0;
            _progressText = progressText;

            RefreshText();

            _currentValue = getValueFunc();
            _moveToValue = _currentValue;

            RefreshValueImage();
        }

        #endregion

        #region Private methods

        private void Stop()
        {
            if (_callback != null)
            {
                _callback();
            }

            _animation.Stop();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Обновить состояние прогресса
        /// </summary>
        private void RefreshValueImage()
        {
            _value.fillAmount = _currentValue;

            _animation.OnProgress(_currentValue);
        }

        /// <summary>
        /// Обновить текст прогресса
        /// </summary>
        private void RefreshText()
        {
            string dots = new string('.', _dotCount);

            _text.text = string.Format("{0}{1}{0}", dots, _progressText);
        }

        #endregion
    }
}
