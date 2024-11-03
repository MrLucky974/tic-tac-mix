using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LuckiusDev.Utils.Editor
{
    public static class AttributeHelper
    {
        public static T GetPropertyAttribute<T>(this SerializedProperty property, bool inherit) where T : PropertyAttribute
        {
            if (property == null) return null;

            Type t = property.serializedObject.GetType();

            FieldInfo fieldInfo = null;
            PropertyInfo propertyInfo = null;
            foreach (var name in property.propertyPath.Split('.'))
            {
                fieldInfo = t.GetField(name, (BindingFlags)(-1));

                if (fieldInfo == null)
                {
                    propertyInfo = t.GetProperty(name, (BindingFlags)(-1));
                    if (propertyInfo == null)
                        return null;
                    t = propertyInfo.PropertyType;
                }
                else
                {
                    t = fieldInfo.FieldType;
                }
            }

            T[] attributes;
            if (fieldInfo != null)
            {
                attributes = fieldInfo.GetCustomAttributes(typeof(T), inherit) as T[];
            }
            else if (propertyInfo != null)
            {
                attributes = propertyInfo.GetCustomAttributes(typeof(T), inherit) as T[];
            }
            else
            {
                return null;
            }

            return attributes.Length > 0 ? attributes[0] : null;
        }
    }
}
