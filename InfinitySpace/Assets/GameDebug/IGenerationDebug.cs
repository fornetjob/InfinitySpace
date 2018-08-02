namespace Assets.GameDebug
{
    /// <summary>
    /// Интерфейс отладки генерации
    /// </summary>
    public interface IGenerationDebug
    {
        /// <summary>
        /// Начало генерации
        /// </summary>
        void OnBeginGenerate();
        /// <summary>
        /// Конец генерации
        /// </summary>
        void OnEndGenerate();
    }
}
