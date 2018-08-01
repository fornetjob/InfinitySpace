using UnityEngine;

namespace Assets.Game.Core.PoolingSystem.Base
{
    /// <summary>
    /// Базовое поведение элемента пулинга
    /// </summary>
    public abstract class PoolingItemBase : MonoBehaviour, IPoolingItem
    {
        #region Fields

        /// <summary>
        /// Пулинг, для того чтобы элемент мог самостоятельно себя уничтожить
        /// </summary>
        private IPooling
            _parent;

        #endregion

        #region IPoolingItem implementation

        /// <summary>
        /// Вызывается один раз при создании элемента
        /// </summary>
        /// <param name="parent">Пулинг</param>
        void IPoolingItem.Begin(IPooling parent)
        {
            _parent = parent;

            OnBegin();
        }

        /// <summary>
        /// Вызывается каждый раз при создании элемента пулинга
        /// </summary>
        void IPoolingItem.Create()
        {
            OnCreate();
        }

        /// <summary>
        /// Вызывается каждый раз при уничтожении элемента пулинга
        /// </summary>
        public void Destroy()
        {
            OnDestroy();

            _parent.DestroyItem(this);
        }

        /// <summary>
        /// Возвращает уникальный идентификатор элемента
        /// </summary>
        /// <returns>Уникальный идентификатор</returns>
        public abstract int GetId();

        #endregion

        #region Protected methods

        /// <summary>
        /// Событие для переопределения для Begin
        /// </summary>
        protected virtual void OnBegin()
        {

        }

        /// <summary>
        /// Событие для переопределения для Create
        /// </summary>
        protected virtual void OnCreate()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Событие для переопределения для Destroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}
