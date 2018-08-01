namespace Assets.Game.Editor.Tests.DataContracts
{
    /// <summary>
    /// Контракт для хранения отсортированных ячеек
    /// </summary>
    public struct SortedCell
    {
        /// <summary>
        /// Х координата
        /// </summary>
        public int X;
        /// <summary>
        /// Y координата
        /// </summary>
        public int Y;
        /// <summary>
        /// Рейтинг
        /// </summary>
        public ushort Rating;
    }
}
