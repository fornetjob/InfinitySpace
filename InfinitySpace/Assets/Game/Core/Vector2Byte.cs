namespace Assets.Game.Core
{
    /// <summary>
    /// Представление двумерного вектора с использованием байтов
    /// </summary>
    public struct Vector2Byte
    {
        #region ctor

        /// <summary>
        /// Создать вектор
        /// </summary>
        /// <param name="xPos">X компонента вектора</param>
        /// <param name="yPos">Y компонента вектора</param>
        public Vector2Byte(byte xPos, byte yPos)
        {
            x = xPos;
            y = yPos;
        }

        #endregion

        #region Properties

        /// <summary>
        /// X компонента вектора
        /// </summary>
        public byte x;

        /// <summary>
        /// Y компонента вектора
        /// </summary>
        public byte y;

        #endregion
    }
}
