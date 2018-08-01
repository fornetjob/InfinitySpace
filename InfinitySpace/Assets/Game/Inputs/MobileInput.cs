using Assets.Game.UI.Controls;
using Assets.Game.Core;
using Assets.Game.Core.Collections;

using UnityEngine;

namespace Assets.Game.Inputs
{
    /// <summary>
    /// Управления игроком с кнопок на экране
    /// </summary>
    public class MobileInput : KeyboardInput
    {
        #region Mappings

        /// <summary>
        /// Кнопки управления
        /// </summary>
        [Mapping(".")]
        [SerializeField]
        public ExtraButton[] Buttons;

        #endregion

        #region Fields

        /// <summary>
        /// Словарь с кнопками
        /// </summary>
        private EnumObjectDictionary<KeyCode, ExtraButton>
            _buttonsDict;

        #endregion

        #region Game

        void Awake()
        {
            _buttonsDict = new EnumObjectDictionary<KeyCode, ExtraButton>(Buttons);
        }

        #endregion

        #region Overriden methods

        /// <summary>
        /// Клавиша нажата
        /// </summary>
        /// <param name="key">Клавиша</param>
        /// <returns>Результат проверки</returns>
        protected override bool IsKeyDown(KeyCode key)
        {
            return _buttonsDict.GetValue(key).IsButtonDown();
        }

        /// <summary>
        /// Клавиша удерживается
        /// </summary>
        /// <param name="key">Клавиша</param>
        /// <returns>Результат проверки</returns>
        protected override bool IsKeyPressed(KeyCode key)
        {
            return _buttonsDict.GetValue(key).IsButtonPressed();
        }

        /// <summary>
        /// Состояние контроллера изменилось
        /// </summary>
        /// <param name="isEnabled">Контроллер включен/выключен</param>
        protected override void OnStateChanged(bool isEnabled)
        {
            if (gameObject.activeSelf != isEnabled)
            {
                gameObject.SetActive(isEnabled);
            }
        }

        #endregion
    }
}