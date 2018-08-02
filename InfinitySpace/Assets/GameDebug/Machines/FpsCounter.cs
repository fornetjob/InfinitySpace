using UnityEngine;

namespace Assets.GameDebug.Machines
{
    /// <summary>
    /// Счётчик фпс
    /// </summary>
    public class FpsCounter
    {
        #region Fields

        /// <summary>
        /// Накопленный за интервал ФПС
        /// </summary>
        private float
            _fpsCount = 0;

        /// <summary>
        /// Количество фреймов за интервал
        /// </summary>
        private int
            _framesCount = 0;

        #endregion

        #region Public methods

        /// <summary>
        /// Обновить счётчик фпс
        /// </summary>
        public void OnUpdate()
        {
            _fpsCount += Time.timeScale / Time.deltaTime;

            _framesCount++;
        }

        /// <summary>
        /// Получить текущее значение фпс
        /// </summary>
        /// <returns></returns>
        public int GetValue()
        {
            if (_framesCount == 0)
            {
                return 0;
            }

            int fps = System.Convert.ToInt32(_fpsCount / _framesCount);

            _fpsCount = 0;
            _framesCount = 0;

            return fps;
        }

        #endregion
    }
}
