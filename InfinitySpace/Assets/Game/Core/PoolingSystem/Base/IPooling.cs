namespace Assets.Game.Core.PoolingSystem.Base
{
    /// <summary>
    /// Интерфейс, передаваемый в элементы пулинга, чтобы они могли самостоятельно вызывать своё уничтожение
    /// </summary>
    public interface IPooling
    {
        /// <summary>
        /// Передать элемент пулинга для уничтожения
        /// </summary>
        /// <param name="item"></param>
        void DestroyItem(IPoolingItem item);
    }
}
