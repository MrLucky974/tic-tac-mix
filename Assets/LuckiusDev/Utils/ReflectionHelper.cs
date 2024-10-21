using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LuckiusDev.Utils
{
    public static class ReflectionHelper
    {
        public static IEnumerable<Type> GetSubclassesOf<T>()
        {
            var baseType = typeof(T);
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t != baseType);
        }
        
        public static bool HasAttribute<T>(Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).Length > 0;
        }
        
        public static bool TryGetAttribute<T>(Type type, out T attribute) where T : Attribute
        {
            attribute = type.GetCustomAttribute<T>(true);
            return attribute != null;
        }
        
        public static T InvokeStaticMethod<T>(Type type, string methodName, params object[] arguments)
        {
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);

            if (methodInfo != null)
            {
                object result = methodInfo.Invoke(null, arguments);

                if (result != null)
                {
                    return (T)result;
                }
                else
                {
                    throw new InvalidOperationException("Method did not return a value.");
                }
            }
            else
            {
                throw new ArgumentException($"Static method '{methodName}' not found in type '{type.FullName}'.");
            }
        }
        
        public static object GetStaticMemberValue(object instance, string memberName)
        {
            Type type = instance.GetType();
            MemberInfo memberInfo = type.GetMember(memberName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault();

            if (memberInfo != null)
            {
                switch (memberInfo.MemberType)
                {
                    case MemberTypes.Field:
                        FieldInfo fieldInfo = (FieldInfo)memberInfo;
                        return fieldInfo.GetValue(null);

                    case MemberTypes.Property:
                        PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                        return propertyInfo.GetValue(null);

                    case MemberTypes.Method:
                        MethodInfo methodInfo = (MethodInfo)memberInfo;
                        return methodInfo.Invoke(null, null);

                    default:
                        throw new ArgumentException($"Unsupported member type: {memberInfo.MemberType}");
                }
            }

            throw new ArgumentException($"Static member '{memberName}' not found in type '{type.FullName}'");
        }
    }
}