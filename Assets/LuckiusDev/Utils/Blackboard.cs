using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
    Dictionary<Enum, object> Values = new Dictionary<Enum, object>();

    public void Set<E, T>(E key, T value) where E : Enum {
        Values[key] = value;
    }

    public T Get<E, T>(E key) where E : Enum {
        if (!Values.ContainsKey(key))
            throw new System.ArgumentException($"Could not find value for key: {key} in values.");
        return (T) Values[key];
    }

    public bool TryGet<E, T>(E key, out T value, T defaultValue = default) where E : Enum {
        if (Values.ContainsKey(key)) {
            value = (T) Values[key];
            return true;
        }

        value = defaultValue;
        return false;
    }
}
