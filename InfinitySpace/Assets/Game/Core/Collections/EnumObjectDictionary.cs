using System;
using System.Collections.Generic;

namespace Assets.Game.Core.Collections
{
    /// <summary>
    /// Доступ к объектам по перечислению (в качестве значения перечисления берётся названия объекта)
    /// </summary>
    /// <typeparam name="TEnum">Тип перечисления</typeparam>
    /// <typeparam name="TValue">Тип объекта</typeparam>
    public class EnumObjectDictionary<TEnum, TValue>
        where TEnum : struct
        where TValue : UnityEngine.Object
    {
        #region Fields

        /// <summary>
        /// Словарь объектов по перечислению
        /// </summary>
        private Dictionary<TEnum, TValue>
            _dict = new Dictionary<TEnum, TValue>();

        /// <summary>
        /// Объекты в словаре
        /// </summary>
        private TValue[]
            _values;

        #endregion

        #region ctor

        public EnumObjectDictionary(TValue[] values)
        {
            var enumType = typeof(TEnum);

            if (enumType.IsEnum ==false)
            {
                throw new System.ArgumentOutOfRangeException(string.Format("Type {0} is not enumeration", enumType.Name));
            }

            _values = values;

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];

                var enumValue = (TEnum)Enum.Parse(enumType, value.name);

                _dict.Add(enumValue, value);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получить объект по значению перечисления
        /// </summary>
        /// <param name="enumValue">Значение перечисления</param>
        /// <returns>Объект</returns>
        public TValue GetValue(TEnum enumValue)
        {
            return _dict[enumValue];
        }

        /// <summary>
        /// Получить все объекты
        /// </summary>
        /// <returns>Все объекты</returns>
        public TValue[] GetValues()
        {
            return _values;
        }

        #endregion
    }
}