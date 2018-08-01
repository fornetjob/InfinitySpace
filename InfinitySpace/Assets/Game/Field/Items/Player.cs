using Assets.Game.Core;
using Assets.Game.Field.Items.Base;

using UnityEngine;

namespace Assets.Game.Field.Items
{
    /// <summary>
    /// Игрок
    /// </summary>
    [RequireComponent(typeof(ActorRenderer))]
    public class Player : MonoBehaviour
    {
        #region Constants

        /// <summary>
        /// Скорость вращения летающей тарелки игрока
        /// </summary>
        private const float PlayerRotationSpeed = 5f;

        #endregion

        #region Mappings

        /// <summary>
        /// Маппинг на Transform игрока 
        /// </summary>
        [Mapping(".")]
        [SerializeField]
        private Transform
            _tr;

        /// <summary>
        /// Мапинг на рендеринг планеты
        /// </summary>
        [Mapping(".")]
        [SerializeField]
        private ActorRenderer
            _render;

        /// <summary>
        /// Маппинг на Transform картинки 
        /// </summary>
        [Mapping("Sprite")]
        [SerializeField]
        private Transform
            _spriteTr;

        #endregion

        #region Game

        void Update()
        {
            _spriteTr.Rotate(0, 0, Time.deltaTime * PlayerRotationSpeed);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Включить отображение игрока
        /// </summary>
        /// <param name="playerRating">Рейтинг игрока</param>
        public void Show(ushort playerRating)
        {
            _render.Enable(playerRating);
        }

        /// <summary>
        /// Установить позицию игрока
        /// </summary>
        /// <param name="position">Координаты позиции</param>
        public void SetPosition(Vector3 position)
        {
            position[2] = 10;

            _tr.position = position;
        }

        #endregion
    }
}