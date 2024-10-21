using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace LuckiusDev.Utils
{
    [Serializable]
    public struct Optional<T>
    {
        [FormerlySerializedAs("enabled")] [SerializeField] private bool m_enabled;
        [FormerlySerializedAs("value")] [SerializeField] private T m_value;

        public bool Enabled => m_enabled;
        public T Value => m_value;

        public Optional(T initialValue, bool enabled = true)
        {
            m_enabled = enabled;
            m_value = initialValue;
        }

        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        public static implicit operator T(Optional<T> value)
        {
            return value.m_value;
        }
    }
}