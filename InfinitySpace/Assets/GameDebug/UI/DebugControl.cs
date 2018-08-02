using Assets.Game;
using Assets.Game.Core;
using Assets.GameDebug.Machines;
using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace Assets.GameDebug.UI
{
    /// <summary>
    /// Отладка
    /// </summary>
    public class DebugControl : MonoBehaviour, IGameDebug
    {
        #region Mappings

        /// <summary>
        /// Маппинг на мобильное управление
        /// </summary>
        [Mapping]
        [SerializeField]
        private TextMeshProUGUI
            _debugText;

        #endregion

        #region Fields

        /// <summary>
        /// Маппинг на счётчик
        /// </summary>
        private FpsCounter
            _fpsCounter = new FpsCounter();

        /// <summary>
        /// Последнее время генерации
        /// </summary>
        private long
            _lastGenerateMs;

        /// <summary>
        /// Последнее время поиска
        /// </summary>
        private long
            _lastSearchMs;

        /// <summary>
        /// Задержка между обновлением состояния
        /// </summary>
        private CheckDelay
            _delay = new CheckDelay(1000);

        /// <summary>
        /// Для измерения интервалов генерации
        /// </summary>
        private Stopwatch
            _generateWatch = new Stopwatch();

        /// <summary>
        /// Для измерения интервалов поиска
        /// </summary>
        private Stopwatch
            _searchWatch = new Stopwatch();

        #endregion

        #region Game

        void Update()
        {
            _fpsCounter.OnUpdate();

            if (_delay.Check())
            {
                RefreshDebugText();
            }
        }

        #endregion

        #region IGameDebug implementation

        /// <summary>
        /// Включить отладку
        /// </summary>
        public void StartDebug()
        {
            RefreshDebugText();

            gameObject.SetActive(true);
        }

        /// <summary>
        /// Выключить отладку
        /// </summary>
        public void StopDebug()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Начало генерации
        /// </summary>
        public void OnBeginGenerate()
        {
            _generateWatch.Reset();
            _generateWatch.Start();
        }

        /// <summary>
        /// Начало поиска
        /// </summary>
        public void OnBeginSearch()
        {
            _searchWatch.Reset();
            _searchWatch.Start();
        }

        /// <summary>
        /// Конец генерации
        /// </summary>
        public void OnEndGenerate()
        {
            _generateWatch.Stop();
            
            _lastGenerateMs = _generateWatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Конец поиска
        /// </summary>
        public void OnEndSearch()
        {
            _searchWatch.Stop();

            _lastSearchMs = _searchWatch.ElapsedMilliseconds;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Обновить текст отладки
        /// </summary>
        private void RefreshDebugText()
        {
            _debugText.text = string.Format("ST:{0}ms GT:{1}ms FPS:{2}", _lastSearchMs, _lastGenerateMs, _fpsCounter.GetValue());
        }

        #endregion
    }
}
