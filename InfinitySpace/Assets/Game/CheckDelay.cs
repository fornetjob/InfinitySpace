using System;

namespace Assets.Game
{
    /// <summary>
    /// Используется проверки интервалов задержки
    /// </summary>
    public class CheckDelay
    {
        #region Fields

        /// <summary>
        /// Предыдущее время успешной проверки
        /// </summary>
        private DateTime
            _prevTime;

        /// <summary>
        /// Время задержки
        /// </summary>
        private int
            _waitMs;

        #endregion

        /// <summary>
        /// Новый проверка задержки
        /// </summary>
        /// <param name="waitMs">Задержка в милисекундах</param>
        public CheckDelay(int waitMs)
        {
            _waitMs = waitMs;
        }

        #region Public methods

        /// <summary>
        /// Проверить что текущее время больше или равно задержке
        /// </summary>
        /// <returns>Результат проверки</returns>
        public bool Check()
        {
            return Check(DateTime.Now);
        }

        /// <summary>
        /// Установить текущее время
        /// </summary>
        public void Process()
        {
            Process(DateTime.Now);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Установить время
        /// </summary>
        /// <param name="now">Время</param>
        private void Process(DateTime now)
        {
            _prevTime = now;
        }

        /// <summary>
        /// Проверить что время больше или равно задержке
        /// </summary>
        /// <param name="now">Время</param>
        /// <returns>Результат проверки</returns>
        private bool Check(DateTime now)
        {
            if ((now - _prevTime).TotalMilliseconds >= _waitMs)
            {
                Process();

                return true;
            }

            return false;
        }

        #endregion
    }
}
