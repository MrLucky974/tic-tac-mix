using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// Read Only attribute.
    /// Attribute is use only to mark ReadOnly properties.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
        private bool _onPlayMode;
        public bool OnPlayMode => _onPlayMode;

        public ReadOnlyAttribute(bool onPlayMode = false)
        {
            _onPlayMode = onPlayMode;
        }
    }
}