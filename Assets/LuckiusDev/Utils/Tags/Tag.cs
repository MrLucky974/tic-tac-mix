using UnityEngine;

namespace LuckiusDev.Utils
{
    [CreateAssetMenu(fileName = "Tag", menuName = "GameObject/Tags")]
    public class Tag : ScriptableObject
    {
        private void OnEnable()
        {
#if UNITY_EDITOR
            // use platform dependent compilation so it only exists in editor, otherwise it'll break the build
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) this.RegisterTag();
#endif
        }

        private void Awake()
        {
            this.RegisterTag();
        }

        public static bool TryGetTag(string tagName, out Tag tag) => (TagsManager.tags.TryGetValue(tagName, out tag));
    }
}
