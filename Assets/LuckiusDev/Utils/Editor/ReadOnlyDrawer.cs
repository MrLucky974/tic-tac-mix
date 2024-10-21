using UnityEngine;
using UnityEditor;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// This class contain custom drawer for ReadOnly attribute.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ReadOnlyAttribute readOnly = attribute as ReadOnlyAttribute;
            
            var previousGUIState = GUI.enabled;
            GUI.enabled = readOnly.OnPlayMode ? !Application.isPlaying : false;
            
            EditorGUI.PropertyField(position, property, label, true);
            
            GUI.enabled = previousGUIState;
        }
    }
}