using Assets.Game.Core;
using Assets.Game.Field;
using Assets.Game.Inputs;
using Assets.Game.Inputs.Base;
using Assets.Game.UI.Controls;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Game.UI
{
    /// <summary>
    /// Базовая форма игры
    /// </summary>
    public class GameFrame:MonoBehaviour
    {
        #region Mappings

        /// <summary>
        /// Маппинг на поле
        /// </summary>
        [Mapping(AbsolutePath = "FieldCamera")]
        [SerializeField]
        private FieldBehaviour
            _field;

        /// <summary>
        /// Маппинг на прогресс бар
        /// </summary>
        [Mapping]
        [SerializeField]
        private ProgressBar
           _progressBar;

        /// <summary>
        /// Маппинг на зум
        /// </summary>
        [Mapping]
        [SerializeField]
        private ZoomControl
           _zoom;

        /// <summary>
        /// Маппинг на мобильное управление
        /// </summary>
        [Mapping]
        [SerializeField]
        private MobileInput
            _mobileInput;

        #endregion

        void Awake()
        {
            IPlayerInput input;

#if (UNITY_ANDROID && !UNITY_EDITOR)
            input = _mobileInput;
#else
            if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Other)
            {
                input = _mobileInput;
            }
            else
            {
                input = gameObject.AddComponent<KeyboardInput>();
            }
#endif

            _field.Init(_zoom, _progressBar, input);
        }
    }
}
