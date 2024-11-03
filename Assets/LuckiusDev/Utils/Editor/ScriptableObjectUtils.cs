using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LuckiusDev.Utils.Editor
{
    public class ScriptableObjectUtils
    {
        /// <summary>
        /// Gets all instances of a specified type of ScriptableObject in the project.
        /// </summary>
        /// <typeparam name="T">The type of ScriptableObject to gather.</typeparam>
        /// <returns>A list containing all instances of the specified ScriptableObject type.</returns>
        public static List<T> GetAllInstances<T>() where T : ScriptableObject
        {
            List<T> instances = new List<T>();

            // Find all ScriptableObjects of the specified type in the project
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T instance = AssetDatabase.LoadAssetAtPath<T>(path);
                if (instance != null)
                {
                    instances.Add(instance);
                }
            }

            return instances;
        }
    }
}