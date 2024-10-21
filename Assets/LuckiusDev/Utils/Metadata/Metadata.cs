using UnityEngine;
using System.Collections.Generic;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// MonoBehaviour script for managing metadata using a dictionary with string keys and object values.
    /// </summary>
    public class Metadata : MonoBehaviour
    {
        /// <summary>
        /// Dictionary to store metadata with string keys and object values.
        /// </summary>
        private readonly Dictionary<string, object> _metadata = new();

        /// <summary>
        /// Add or update metadata for a given key.
        /// </summary>
        /// <typeparam name="T">Type of the metadata value.</typeparam>
        /// <param name="key">Key for the metadata.</param>
        /// <param name="value">Value of the metadata.</param>
        public void SetData<T>(string key, T value)
        {
            if (_metadata.TryAdd(key, value)) return;
            // If key already exists, update its value
            Debug.LogWarning("Key already exists. Updating value.");
            _metadata[key] = value;
        }

        /// <summary>
        /// Attempt to retrieve metadata value for a given key.
        /// </summary>
        /// <typeparam name="T">Type of the metadata value.</typeparam>
        /// <param name="key">Key for the metadata.</param>
        /// <param name="value">Retrieved value of the metadata if successful, default value otherwise.</param>
        /// <returns>True if metadata was found for the key, false otherwise.</returns>
        public bool TryGetData<T>(string key, out T value)
        {
            if (_metadata.TryGetValue(key, out var value1))
            {
                // If key exists, retrieve the value
                value = (T)value1;
                return true;
            }

            // If key does not exist, set the value to default and return false
            value = default(T);
            Debug.LogWarning("Key not found.");
            return false;
        }
        
        /// <summary>
        /// Check if a key exists in the metadata dictionary.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key exists, false otherwise.</returns>
        public bool HasData(string key)
        {
            return _metadata.ContainsKey(key);
        }

        /// <summary>
        /// Remove metadata associated with a given key.
        /// </summary>
        /// <param name="key">Key for the metadata to remove.</param>
        public void RemoveData(string key)
        {
            if (_metadata.ContainsKey(key))
            {
                // Remove metadata if the key exists
                _metadata.Remove(key);
            }
            else
            {
                // If key does not exist, log a warning
                Debug.LogWarning("Key not found. Nothing to remove.");
            }
        }
    }
}