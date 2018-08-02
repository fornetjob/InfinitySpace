namespace Assets.GameDebug
{
    /// <summary>
    /// Интерфейс отладки
    /// </summary>
    public interface IGameDebug : ICellsVisitorDebug, IGenerationDebug
    {
        /// <summary>
        /// Включить отладку
        /// </summary>
        void StartDebug();
        /// <summary>
        /// Выключить отладку
        /// </summary>
        void StopDebug();
    }
}
