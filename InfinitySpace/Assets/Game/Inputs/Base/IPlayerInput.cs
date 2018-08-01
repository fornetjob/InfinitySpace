using UnityEngine;

namespace Assets.Game.Inputs.Base
{
    /// <summary>
    /// Интерфейс контроллера игрока
    /// </summary>
    public interface IPlayerInput
    {
        /// <summary>
        /// Возвращает направление движения игрока
        /// </summary>
        /// <returns>Направление движения</returns>
        Vector2Int GetDirection();

        /// <summary>
        /// Установить состояние контроллера
        /// </summary>
        /// <param name="isEnable">Контроллер включен/выключен</param>
        void SetState(bool isEnable);
    }
}
