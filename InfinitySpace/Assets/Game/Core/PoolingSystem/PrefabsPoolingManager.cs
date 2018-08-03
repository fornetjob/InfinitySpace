using Assets.Game.Access;
using Assets.Game.Access.Enums;
using Assets.Game.Core.PoolingSystem.Base;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.Game.Core.PoolingSystem
{
    /// <summary>
    /// Пулинг префабов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PrefabsPoolingManager<T>: IPooling
        where T : IPoolingItem
    {
        #region Fields

        /// <summary>
        /// Объект синхронизации
        /// </summary>
        private object
            _lockObj = new object();

        /// <summary>
        /// Тип префаба
        /// </summary>
        private PrefabType
            _type;

        /// <summary>
        /// Словарь активных элементов пулинга в разрезе идентификаторов
        /// </summary>
        private Dictionary<int, T>
            _dict = new Dictionary<int, T>();

        /// <summary>
        /// Активные элементы пулинга
        /// </summary>
        private List<T>
            _activeArray = new List<T>();

        /// <summary>
        /// Уничтоженные элементы пулинга
        /// </summary>
        private Queue<T>
            _destroyedStack = new Queue<T>();

        #endregion

        #region ctor

        /// <summary>
        /// Создать пулинг для указанного типа префабов
        /// </summary>
        /// <param name="type">Тип префаба</param>
        public PrefabsPoolingManager(PrefabType type)
        {
            _type = type;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Возвращает все активные элементы пулинга
        /// </summary>
        /// <returns></returns>
        public IList<T> GetValues()
        {
            return _activeArray;
        }

        /// <summary>
        /// Возвращает активный элемент пулинга по его идентификатору
        /// </summary>
        /// <param name="id">Идинтификатор элемента пулинга</param>
        /// <returns>Активный пулинга</returns>
        public T GetByIdOrNull(int id)
        {
            T result;

            _dict.TryGetValue(id, out result);

            return result;
        }

        /// <summary>
        /// Создаёт элемент пулинга с определённым уникальным идентификатором
        /// </summary>
        /// <param name="id">Уникальный идентификатор</param>
        /// <returns>Созданный элемент пулинга</returns>
        public T Create(int id)
        {
            lock (_lockObj)
            {
                T newItem;

                if (_destroyedStack.Count == 0)
                {
                    GameObject instance = GameObject.Instantiate(ResourcesAccess.Instance.GetPrefab(_type));

                    newItem = instance.GetComponent<T>();

                    newItem.Begin(this);
                }
                else
                {
                    newItem = _destroyedStack.Dequeue();
                }

                _activeArray.Add(newItem);

                _dict.Add(id, newItem);

                newItem.Create();

                return newItem;
            }
        }

        /// <summary>
        /// Уничтожает все элементы пулинга
        /// </summary>
        public void DestroyAll()
        {
            lock (_lockObj)
            {
                while (_activeArray.Count > 0)
                {
                    _activeArray[_activeArray.Count - 1].Destroy();
                }
            }
        }

        #endregion

        #region IPooling implementation
        
        /// <summary>
        /// Уничтожает определённый элемент пулинга
        /// </summary>
        /// <param name="item">Элемент пулинга к уничтожению</param>
        void IPooling.DestroyItem(IPoolingItem item)
        {
            lock (_lockObj)
            {
                _dict.Remove(item.GetId());

                if (_activeArray.Remove((T)item))
                {
                    _destroyedStack.Enqueue((T)item);
                }
            }
        }

        #endregion
    }
}