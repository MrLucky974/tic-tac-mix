/*
 * Most of the code is from Neon Age on Github.
 * Link to the original code : https://github.com/neon-age/Tags
 * I rewrote the code because I couldn't import the original "package".
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class TagsManager
    {
        internal static readonly Dictionary<string, Tag> tags = new Dictionary<string, Tag>();
        private static readonly Dictionary<Tag, HashSet<GameObject>> allObjectsWithTag = new Dictionary<Tag, HashSet<GameObject>>();
        private static readonly Dictionary<GameObject, Tags> cachedTags = new Dictionary<GameObject, Tags>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            cachedTags.Clear();
        }

        #region FindGameObjectWithTag
        public static GameObject FindWithTag(this GameObject gameObject, string tagName)
        {
            if (!tags.TryGetValue(tagName, out Tag tag))
            {
                Debug.LogError($"No tag called {tagName}");
                return null;
            }

            CheckFindWithTagArguments(tag);

            //if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0) throw new NullReferenceException($"No Game Objects found with tag {tag.name}.");
            if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0)
            {
                //Debug.LogError($"No Game Objects found with tag {tagName}");
                return null;
            }

            GameObject firstObject = null;
            foreach (GameObject obj in objectsLookup)
            {
                firstObject = obj;
                break;
            }

            //Debug.Log($"Game Object [{firstObject.name}]");

            return firstObject;
        }

        public static GameObject FindWithTag(this GameObject gameObject, Tag tag)
        {
            CheckFindWithTagArguments(tag);

            //if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0) throw new NullReferenceException($"No Game Objects found with tag {tag.name}.");
            if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0) return null;

            GameObject firstObject = null;
            foreach (GameObject obj in objectsLookup)
            {
                firstObject = obj;
                break;
            }

            return firstObject;
        }

        public static HashSet<GameObject> FindAllWithTag(this GameObject gameObject, string tagName)
        {
            if (!tags.TryGetValue(tagName, out Tag tag)) return null;

            CheckFindWithTagArguments(tag);

            //if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0) throw new NullReferenceException($"No Game Objects found with tag {tag.name}.");
            if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0) return null;

            return objectsLookup;
        }

        public static HashSet<GameObject> FindAllWithTag(this GameObject gameObject, Tag tag)
        {
            CheckFindWithTagArguments(tag);

            //if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0) throw new NullReferenceException($"No Game Objects found with tag {tag.name}.");
            if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0) return null;

            return objectsLookup;
        }

        public static bool TryFindWithTag(this GameObject gameObject, Tag tag, out GameObject objectWithTag)
        {
            CheckFindWithTagArguments(tag);

            objectWithTag = null;
            if (!allObjectsWithTag.TryGetValue(tag, out var objectsLookup) || objectsLookup.Count == 0) return false;

            foreach (GameObject obj in objectsLookup)
            {
                objectWithTag = obj;
                break;
            }

            return true;
        }

        public static bool TryFindAllWithTag(this GameObject gameObject, Tag tag, out HashSet<GameObject> objectsLookup)
        {
            CheckFindWithTagArguments(tag);
            return allObjectsWithTag.TryGetValue(tag, out objectsLookup) && objectsLookup.Count != 0;
        }

        private static void CheckFindWithTagArguments(Tag tag)
        {
            if (!tag) throw new NullReferenceException("Trying to find with none tag.");
        }
        #endregion

        #region HasTag
        //HasTag
        public static bool HasTag(this Component instance, string tagName)
        {
            if (!tags.TryGetValue(tagName, out Tag tag)) return false;
            return HasTag(instance.gameObject, tag);
        }

        public static bool HasTag(this Component instance, Tag tag) => HasTag(instance.gameObject, tag);

        public static bool HasTag(this GameObject gameObject, string tagName)
        {
            if (!tags.TryGetValue(tagName, out Tag tag)) return false;
            return HasTag(gameObject, tag);
        }

        public static bool HasTag(this GameObject gameObject, Tag tag)
        {
            if (!TryGetTagComponent(gameObject, out var component)) return false;

            bool passed = (component.GetTags().Contains(tag));
            return passed;
        }

        //HasOnlyTag
        public static bool HasOnlyTag(this Component instance, string tagName)
        {
            if (!tags.TryGetValue(tagName, out Tag tag)) return false;
            return HasOnlyTag(instance.gameObject, tag);
        }

        public static bool HasOnlyTag(this Component instance, Tag tag) => HasOnlyTag(instance.gameObject, tag);

        public static bool HasOnlyTag(this GameObject gameObject, string tagName)
        {
            if (!tags.TryGetValue(tagName, out Tag tag)) return false;
            return HasOnlyTag(gameObject, tag);
        }

        public static bool HasOnlyTag(this GameObject gameObject, Tag tag)
        {
            if (!TryGetTagComponent(gameObject, out var component)) return false;

            var compareTags = component.GetTags();
            if (compareTags.Count > 1) return false;

            bool passed = (compareTags[0] == tag);
            return passed;
        }

        //HasAnyTag
        public static bool HasAnyTag(this Component instance, params Tag[] tags) => HasAnyTag(instance.gameObject, tags);

        public static bool HasAnyTag(this GameObject gameObject, params Tag[] tags)
        {
            if (!TryGetTagComponent(gameObject, out var component)) return false;

            List<Tag> compareTags = component.GetTags();
            bool passed = false;

            foreach (var tag in tags) if (compareTags.Contains(tag)) passed = true;

            return passed;
        }

        //HasAllTag
        public static bool HasAllTags(this Component instance, params Tag[] tags) => HasAllTags(instance.gameObject, tags);

        public static bool HasAllTags(this GameObject gameObject, params Tag[] tags)
        {
            if (!TryGetTagComponent(gameObject, out var component)) return false;

            List<Tag> compareTags = component.GetTags();
            bool passed = false;

            foreach (var tag in tags) if (!compareTags.Contains(tag)) passed = false;

            return passed;
        }
        #endregion

        internal static void RegisterTag(this Tag tag)
        {
            if (!tags.TryGetValue(tag.name, out _))
            {
                tags.Add(tag.name, tag);
            }
        }

        internal static void RegisterGameObjectWithTag(this GameObject gameObject, Tag tag)
        {
            if (!allObjectsWithTag.TryGetValue(tag, out HashSet<GameObject> objectsList))
            {
                objectsList = new HashSet<GameObject>();
                allObjectsWithTag.Add(tag, objectsList);
            }
            objectsList.Add(gameObject);
        }

        internal static void RemoveGameObjectWithTag(this GameObject gameObject, Tag tag)
        {
            if (!allObjectsWithTag.TryGetValue(tag, out HashSet<GameObject> objectsList)) return;
            objectsList.Remove(gameObject);
        }

        private static bool TryGetTagComponent(GameObject gameObject, out Tags component)
        {
            if (!cachedTags.TryGetValue(gameObject, out component))
            {
                if (!gameObject.TryGetComponent(out component)) return false;

                cachedTags.Add(gameObject, component);
            }
            return true;
        }

        #region MenuItem
        [MenuItem("GameObject/Create Empty (Tags)", false, 0)]
        static void CreateEmptyWithTag(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject("GameObject");

            // Add Tags component
            go.AddComponent<Tags>();

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
        #endregion
    }
}
