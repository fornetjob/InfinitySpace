namespace Assets.Game.Core
{
    /// <summary>
    /// Атрибут для кешируемых компонентов
    /// </summary>
    public class MappingAttribute:System.Attribute
    {
        #region Ctor

        /// <summary>
        /// Кешировать дочерний компонент по его названию
        /// </summary>
        public MappingAttribute()
            : this(string.Empty)
        {

        }

        /// <summary>
        /// Кешировать дочерний компонент по указанному пути
        /// </summary>
        /// <param name="path">Путь до дочернего компонента</param>
        public MappingAttribute(string path)
        {
            Path = path;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Абсолютный путь до компонента. Проверьте, что объект не является скрытым, иначе маппинга не произойдёт.
        /// </summary>
        public string AbsolutePath;
        /// <summary>
        /// Путь до дочернего компонента
        /// </summary>
        public string Path;
        /// <summary>
        /// Компонент может отсутствовать
        /// </summary>
        public bool CanEmpty;

        #endregion
    }
}
