using UnityEngine;

namespace LuckiusDev.Utils
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
        [ScriptableObjectId] public string m_identifier;
    }
}