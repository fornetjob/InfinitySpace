using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Game.Editor.Tools
{
    public static class EditorTool
    {
        public static List<KeyValuePair<FieldInfo, TAttr>> GetAllFields<TAttr>(Type type)
            where TAttr : System.Attribute
        {
            var fields = type.GetFields(
                    BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance);

            List<KeyValuePair<FieldInfo, TAttr>> result = new List<KeyValuePair<FieldInfo, TAttr>>();

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];

                var attrs = field.GetCustomAttributes(typeof(TAttr), false);

                if (attrs.Length == 0)
                {
                    continue;
                }

                result.Add(new KeyValuePair<FieldInfo, TAttr>(field, (TAttr)attrs[0]));
            }

            return result;
        }

        public static T GetField<T>(object obj, string name)
        {
            var type = obj.GetType();

            var field = type.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
            {
                throw new System.ArgumentOutOfRangeException(string.Format("Field {0} not found", name));
            }

            return (T)field.GetValue(obj);
        }
    }
}
