using System.Collections.Generic;
using Assets.Game.Inputs.Base;

using UnityEngine;

namespace Assets.Game.Inputs
{
    /// <summary>
    /// Управления игроком с клавиатуры
    /// </summary>
    public class KeyboardInput : MonoBehaviour, IPlayerInput
    {
        #region Fields

        /// <summary>
        /// Зарегестрированные клавиши в системе
        /// </summary>
        private static readonly KeyValuePair<KeyCode, Vector2Int>[]
            RegisteredKeys = new KeyValuePair<KeyCode, Vector2Int>[]
            {
                new KeyValuePair<KeyCode, Vector2Int>(KeyCode.W, Vector2Int.up),
                new KeyValuePair<KeyCode, Vector2Int>(KeyCode.S, Vector2Int.down),
                new KeyValuePair<KeyCode, Vector2Int>(KeyCode.A, Vector2Int.left),
                new KeyValuePair<KeyCode, Vector2Int>(KeyCode.D, Vector2Int.right),
            };

        /// <summary>
        /// Задержка между событиями, если клавиша зажата
        /// </summary>
        private CheckDelay
            _keyWait = new CheckDelay(150);

        /// <summary>
        /// Контроллер включен/выключен
        /// </summary>
        private bool
            _isEnabled = false;

        #endregion

        #region IPlayerInput implementation

        /// <summary>
        /// Возвращает направление движения игрока
        /// </summary>
        /// <returns>Направление движения</returns>
        public Vector2Int GetDirection()
        {
            var direction = Vector2Int.zero;

            if (_isEnabled)
            {
                bool isTimeChecked = _keyWait.Check();

                for (int i = 0; i < RegisteredKeys.Length; i++)
                {
                    var registeredKey = RegisteredKeys[i];

                    if (isTimeChecked
                        || IsKeyDown(registeredKey.Key))
                    {
                        _keyWait.Process();

                        if (IsKeyPressed(registeredKey.Key))
                        {
                            direction += registeredKey.Value;
                        }
                    }
                }
            }

            return direction;
        }

        /// <summary>
        /// Установить состояние контроллера
        /// </summary>
        /// <param name="isEnable">Контроллер включен/выключен</param>
        public void SetState(bool isEnable)
        {
            if (_isEnabled != isEnable)
            {
                _isEnabled = isEnable;

                OnStateChanged(isEnable);
            }
        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Клавиша нажата
        /// </summary>
        /// <param name="key">Клавиша</param>
        /// <returns>Результат проверки</returns>
        protected virtual bool IsKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        /// <summary>
        /// Клавиша удерживается
        /// </summary>
        /// <param name="key">Клавиша</param>
        /// <returns>Результат проверки</returns>
        protected virtual bool IsKeyPressed(KeyCode key)
        {
            return Input.GetKey(key);
        }

        /// <summary>
        /// Состояние контроллера изменилось
        /// </summary>
        /// <param name="isEnabled">Контроллер включен/выключен</param>
        protected virtual void OnStateChanged(bool isEnabled)
        {

        }

        #endregion
    }
}