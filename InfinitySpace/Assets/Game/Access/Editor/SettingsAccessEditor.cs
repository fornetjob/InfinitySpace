using Assets.Game.Core;
using Assets.Game.Editor.Tools;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Assets.Game.Access.Editor
{
    /// <summary>
    /// Редактор настроек
    /// </summary>
    [CustomEditor(typeof(SettingsAccess))]
    public class SettingsAccessEditor : UnityEditor.Editor
    {
        #region Statics

        /// <summary>
        /// Проверить маппинги при релоаде скриптов
        /// </summary>
        [DidReloadScripts]
        private static void RefreshMappings()
        {
            var allComponents = Resources.FindObjectsOfTypeAll<Component>();

            bool isMappingChanged = false;

            foreach (var current in allComponents)
            {
                // Если маппинги были изменены - пометим объект как изменённый
                if (CheckMappings(current))
                {
                    EditorUtility.SetDirty(current.gameObject);

                    isMappingChanged = true;
                }
            }

            if (isMappingChanged)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Проверка изменения маппингов для компонента
        /// </summary>
        /// <param name="component">Компонент</param>
        /// <returns>Результат проверки</returns>
        private static bool CheckMappings(Component component)
        {
            bool isChanged = false;

            var parentTr = component.gameObject.GetComponent<Transform>();

            var type = component.GetType();

            var fields = ReflectionTool.GetAllFields<MappingAttribute>(type);

            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i].Key;

                var mapAttr = fields[i].Value;

                Transform valueTr = null;

                string path;

                if (string.IsNullOrEmpty(mapAttr.AbsolutePath) == false)
                {
                    path = mapAttr.AbsolutePath;

                    var searchObj = GameObject.Find(path);

                    if (searchObj != null)
                    {
                        valueTr = searchObj.GetComponent<Transform>();
                    }
                }
                else
                {
                    path = string.IsNullOrEmpty(mapAttr.Path) ? field.Name : mapAttr.Path;

                    if (path.StartsWith("_"))
                    {
                        path = path.Substring(1);
                        path = path[0].ToString().ToUpper() + path.Substring(1);
                    }

                    if (path.ToLower().Trim() == ".")
                    {
                        valueTr = parentTr;
                    }
                    else
                    {
                        valueTr = parentTr.Find(path);
                    }
                }

                if (valueTr == null)
                {
                    if (mapAttr.CanEmpty == false)
                    {
                        Debug.LogError("Field " + field.Name + " on path " + path + " not exists in " + type.Name + " for component " + component.name + " in " + component.gameObject.name);
                    }
                }
                else
                {
                    if (field.FieldType.IsArray)
                    {
                        var elementType = field.FieldType.GetElementType();

                        var existComponents = valueTr.GetComponentsInChildren(elementType, true);

                        List<Component> values = new List<Component>();

                        for (int j = 0; j < existComponents.Length; j++)
                        {
                            var comp = existComponents[j];

                            if (comp.transform != valueTr
                                && comp.CompareTag("IgnoreComponent") == false)
                            {
                                values.Add(comp);
                            }
                        }

                        if (values == null)
                        {
                            if (mapAttr.CanEmpty == false)
                            {
                                Debug.LogError("Component " + field.FieldType.Name + " in field " + field.Name + " on path " + path + " not exists in " + type.Name + " for component " + component.name + " in " + component.gameObject.name);
                            }
                        }
                        else
                        {
                            var arr = Array.CreateInstance(elementType, values.Count);
                            Array.Copy(values.ToArray(), arr, values.Count);

                            field.SetValue(component, arr);

                            isChanged = true;
                        }
                    }
                    else
                    {
                        var value = valueTr.GetComponent(field.FieldType);

                        if (value == null)
                        {
                            if (mapAttr.CanEmpty == false)
                            {
                                Debug.LogError("Component " + field.FieldType.Name + " in field " + field.Name + " on path " + path + " not exists in " + type.Name + " for component " + component.name + " in " + component.gameObject.name);
                            }
                        }
                        else
                        {
                            var currentValue = (Component)field.GetValue(component);

                            if (currentValue != value)
                            {
                                field.SetValue(component, value);

                                isChanged = true;
                            }
                        }
                    }
                }
            }

            return isChanged;
        }
        #endregion
    }
}
