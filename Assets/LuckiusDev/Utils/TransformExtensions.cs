namespace LuckiusDev.Utils
{
    using System.Collections.Generic;
    using UnityEngine;
    
    /// <summary>
    /// Extension methods for the Transform class providing additional functionality for managing child objects.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Removes all child objects from the given transform.
        /// </summary>
        /// <param name="transform">The transform whose children are to be removed.</param>
        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Instantiates multiple instances of a prefab as children of the given parent transform.
        /// </summary>
        /// <typeparam name="T">Type of the component attached to the prefab.</typeparam>
        /// <param name="parentTransform">The parent transform under which the instances will be created.</param>
        /// <param name="prefab">Prefab to instantiate.</param>
        /// <param name="count">Number of instances to instantiate.</param>
        /// <returns>An enumerable collection containing references to the instantiated objects.</returns>
        public static IEnumerable<T> Instantiate<T>(this Transform parentTransform, T prefab, int count) where T : Component
        {
            List<T> instantiatedObjects = new List<T>();

            for (int i = 0; i < count; i++)
            {
                T obj = Object.Instantiate(prefab, parentTransform);
                instantiatedObjects.Add(obj);
            }

            return instantiatedObjects;
        }

        /// <summary>
        /// Instantiates a single instance of a prefab as a child of the given parent transform.
        /// </summary>
        /// <typeparam name="T">Type of the component attached to the prefab.</typeparam>
        /// <param name="parentTransform">The parent transform under which the instance will be created.</param>
        /// <param name="prefab">Prefab to instantiate.</param>
        /// <returns>A reference to the instantiated object.</returns>
        public static T Instantiate<T>(this Transform parentTransform, T prefab) where T : Component
        {
            return Object.Instantiate(prefab, parentTransform);
        }
    }
}
