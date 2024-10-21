using System.Runtime.CompilerServices;
using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// Extension methods for GameObjects to handle metadata using Metadata component.
    /// </summary>
    public static class MetadataGameObjectExtensions
    {
        // ConditionalWeakTable to associate GameObjects with their Metadata components
        private static readonly ConditionalWeakTable<GameObject, Metadata> ExtendedData = new();

        /// <summary>
        /// Gets the Metadata component associated with the specified GameObject.
        /// If the Metadata component does not exist, it adds one and returns it.
        /// </summary>
        /// <param name="gameObject">The GameObject to retrieve the Metadata for.</param>
        /// <returns>The Metadata component associated with the GameObject.</returns>
        public static Metadata GetMetadata(this GameObject gameObject)
        {
            // Try to get the Metadata associated with the GameObject
            if (ExtendedData.TryGetValue(gameObject, out var metadata))
                return metadata; // Return the existing Metadata if found

            // If Metadata does not exist, add a new one
            metadata = gameObject.AddComponent<Metadata>();

            // Associate the GameObject with its Metadata component
            ExtendedData.AddOrUpdate(gameObject, metadata);

            return metadata; // Return the newly added Metadata component
        }
    }
}