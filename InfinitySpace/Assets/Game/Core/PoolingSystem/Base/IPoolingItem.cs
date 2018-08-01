namespace Assets.Game.Core.PoolingSystem.Base
{
    /// <summary>
    /// Интерфейс элемента пулинга
    /// </summary>
    public interface IPoolingItem
    {
        /// <summary>
        /// Возвращает уникальный идентификатор элемента
        /// </summary>
        /// <returns>Уникальный идентификатор</returns>
        int GetId();
        /// <summary>
        /// Вызывается один раз, при создании элемента пулинга
        /// </summary>
        /// <param name="parent">Пулинг</param>
        void Begin(IPooling parent);
        /// <summary>
        /// Вызывается каждый раз при создании элемента пулинга
        /// </summary>
        void Create();
        /// <summary>
        /// Вызывается каждый раз при уничтожении элемента пулинга
        /// </summary>
        void Destroy();
    }
}
