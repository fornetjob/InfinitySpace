using UnityEngine.EventSystems;
using UnityEngine.UI;

using System;

namespace Assets.Game.UI.Controls
{
    /// <summary>
    /// Кнопка, расширяющая стандартную функциональность кнопки
    /// </summary>
    public class ExtraButton : Button
    {
        #region Fields

        /// <summary>
        /// Кнопка нажата
        /// </summary>
        private bool
            _isDown;

        /// <summary>
        /// Кнопка зажата
        /// </summary>
        private bool
            _isPressed;

        #endregion

        #region Events

        /// <summary>
        /// Кнопка нажата
        /// </summary>
        public event Action OnButtonDown;
        /// <summary>
        /// Кнопка отпущена
        /// </summary>
        public event Action OnButtonUp;

        #endregion

        #region Public methods

        /// <summary>
        /// Кнопка зажата
        /// </summary>
        public bool IsButtonPressed()
        {
            return _isPressed;
        }

        /// <summary>
        /// Кнопка нажата
        /// </summary>
        public bool IsButtonDown()
        {
            if (_isDown)
            {
                _isDown = false;

                return true;
            }

            return false;
        }

        #endregion

        #region Overriden methods

        /// <summary>
        /// Кнопка нажата
        /// </summary>
        /// <param name="eventData">Данные</param>
        public override void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _isDown = true;

            if (OnButtonDown != null)
            {
                OnButtonDown();
            }
        }

        /// <summary>
        /// Кнопка отпущена
        /// </summary>
        /// <param name="eventData">Данные</param>
        public override void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;

            if (OnButtonUp != null)
            {
                OnButtonUp();
            }
        }

        #endregion
    }
}
