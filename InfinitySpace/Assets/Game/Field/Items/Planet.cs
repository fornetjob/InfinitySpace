using Assets.Game.Access;
using Assets.Game.Core;
using Assets.Game.Core.PoolingSystem.Base;
using Assets.Game.Field.Items.Base;

using UnityEngine;

namespace Assets.Game.Field.Items
{
    /// <summary>
    /// Планета - элемент пулинга
    /// </summary>
    [RequireComponent(typeof(ActorRenderer))]
    public class Planet : PoolingItemBase
    {
        #region Mappings

        /// <summary>
        /// Маппинг на Transform текущей планеты 
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

        #endregion

        #region Fields

        /// <summary>
        /// Текущая позиция планеты
        /// </summary>
        private Vector2Int
            _cellItemPosition;

        #endregion

        #region Overriden methods

        /// <summary>
        /// Отключить отображение планеты при уничтожении элемента пулинга
        /// </summary>
        protected override void OnDestroy()
        {
            _render.Disable();
        }

        /// <summary>
        /// Уникальный индентификатор элемента пулинга
        /// </summary>
        /// <returns>Идентификатор</returns>
        public override int GetId()
        {
            return SettingsAccess.GetId(_cellItemPosition);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Включить отображение планеты
        /// </summary>
        /// <param name="cellItemPosition">Позиция планеты на поле</param>
        /// <param name="pos">Позиция планеты в мировых координатах</param>
        /// <param name="rating">Рейтинг планеты</param>
        public void Show(Vector2Int cellItemPosition, Vector2 pos, ushort rating)
        {
            _cellItemPosition = cellItemPosition;

            _tr.localPosition = pos;

            _render.Enable(rating);
        }

        #endregion
    }
}
