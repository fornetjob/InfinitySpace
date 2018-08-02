namespace Assets.GameDebug
{
    /// <summary>
    /// Интерфейс отладки поиска по ячейкам
    /// </summary>
    public interface ICellsVisitorDebug
    {
        /// <summary>
        /// Начало поиска
        /// </summary>
        void OnBeginSearch();
        /// <summary>
        /// Конец поиска
        /// </summary>
        void OnEndSearch();
    }
}
