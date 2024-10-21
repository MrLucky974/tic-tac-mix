using System;
using UnityEngine;

namespace LuckiusDev.Utils.Editor
{
    public static class ComponentHelper
    {
        #region Transform

        public static Vector3 GetPosition(this Component component) => component.transform.position;
        public static Vector3 GetLocalPosition(this Component component) => component.transform.localPosition;
        public static Quaternion GetRotation(this Component component) => component.transform.rotation;
        public static Quaternion GetLocalRotation(this Component component) => component.transform.localRotation;

        #endregion

        #region Logging

        public static void Log(this Component component, string message)
        {
            Debug.Log(message, component);
        }

        public static void LogWarning(this Component component, string message)
        {
            Debug.LogWarning(message, component);
        }

        public static void LogError(this Component component, string message)
        {
            Debug.LogError(message, component);
        }

        public static void LogAssertion(this Component component, string message)
        {
            Debug.LogAssertion(message, component);
        }

        public static void LogException(this Component component, Exception exception)
        {
            Debug.LogException(exception, component);
        }

        #endregion
    }

}