using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class GameObjectUtils
    {
        /// <summary>
        /// Returns the object itself if it exists, null otherwise.
        /// </summary>
        /// <remarks>
        /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
        /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
        /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
        /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
        /// </remarks>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object being checked.</param>
        /// <returns>The object itself if it exists and not destroyed, null otherwise.</returns>
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

        public static bool FindComponent<T>(this GameObject obj, out T component)
        {
            if (obj.TryGetComponent(out component))
            {
                return true;
            }

            if ((component = obj.transform.GetComponentInChildren<T>()) != null)
            {
                return true;
            }

            if (obj.transform.parent != null)
            {
                return FindComponent(obj.transform.parent, out component);
            }

            return false;
        }

        public static bool FindComponent<T>(this Component obj, out T component)
        {
            if (obj.TryGetComponent(out component))
            {
                return true;
            }

            if ((component = obj.transform.GetComponentInChildren<T>()) != null)
            {
                return true;
            }

            if (obj.transform.parent != null)
            {
                return FindComponent(obj.transform.parent, out component);
            }

            return false;
        }
    }
}