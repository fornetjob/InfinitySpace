using Assets.Game.Core;

using TMPro;

using UnityEngine;

namespace Assets.Game.Field.Items.Base
{
    /// <summary>
    /// Базовое поведение для планет и игрока - способность отображать картинку и текст
    /// </summary>
    public class ActorRenderer:MonoBehaviour
    {
        #region Mappings

        /// <summary>
        /// Маппинг на рендеринг картинки
        /// </summary>
        [Mapping]
        [SerializeField]
        private SpriteRenderer 
            _sprite;

        /// <summary>
        /// Маппинг на текст
        /// </summary>
        [Mapping]
        [SerializeField]
        private TextMeshPro
            _text;

        #endregion

        #region Public methods

        /// <summary>
        /// Включить
        /// </summary>
        /// <param name="rating">Рейтинг</param>
        public void Enable(ushort rating)
        {
            _text.text = (rating - 1).ToString();

            _sprite.enabled = true;
            _text.enabled = true;
        }
        
        /// <summary>
        /// Отключить
        /// </summary>
        public void Disable()
        {
            _sprite.enabled = false;
            _text.enabled = false;
        }

        #endregion
    }
}
