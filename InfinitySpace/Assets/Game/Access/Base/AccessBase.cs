using UnityEngine;

namespace Assets.Game.Access.Base
{
    /// <summary>
    /// Базовый класс для инстанцируемых один раз скриптуемых объектов
    /// Объекты должны лежать по пути Access/ТипОбъекта
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public class AccessBase<T>: ScriptableObject
        where T : AccessBase<T>
    {
        #region Statics

        /// <summary>
        /// Объект синхронизации
        /// </summary>
        private static object
            _lockObj = new object();

        /// <summary>
        /// Экземпляр скриптуемого объекта
        /// </summary>
        private static T
            _instance;

        /// <summary>
        /// Доступ к экзепляру скриптуемого объекта
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            string name = typeof(T).Name;

                            string path = string.Format("Access/{0}", typeof(T).Name);

                            _instance = (T)Resources.Load(path);

                            if (_instance == null)
                            {
                                throw new System.NotSupportedException(string.Format("(Access) В системе отсутствует скриптуемый объект {0} по пути {1}", name, path));
                            }

                            _instance.OnBegin();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        protected virtual void OnBegin()
        {

        }
    }
}