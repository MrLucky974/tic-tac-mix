using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LuckiusDev.Utils
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private Object m_sceneAsset;
        [SerializeField] private string m_sceneName;

        public string SceneName => m_sceneName;

        public static implicit operator string(SceneReference @object) {
            return @object.SceneName;
        }
    }
}