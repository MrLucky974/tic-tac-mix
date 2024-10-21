using UnityEditor;
using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class GUIHelper
    {
        public static void DrawLine(Color color, int thickness = 2, int padding = 10, int margin = 0)
        {
            color = color != default ? color : Color.grey;
            Rect r = EditorGUILayout.GetControlRect(false, GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding * 0.5f;
 
            switch (margin)
            {
                // expand to maximum width
                case < 0:
                    r.x = 0;
                    r.width = EditorGUIUtility.currentViewWidth;
 
                    break;
                case > 0:
                    // shrink line width
                    r.x += margin;
                    r.width -= margin * 2;
 
                    break;
            }
 
            EditorGUI.DrawRect(r, color);
        }
    }
}