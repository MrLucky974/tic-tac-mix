using UnityEngine;

namespace LuckiusDev.Utils
{
    public static class WorldText {
        public const int SORTING_ORDER_DEFAULT = 5000;

        /// <summary>
        /// Creates a TextMesh object in your world to display text.
        /// </summary>
        public static TextMesh CreateWorldText( string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = SORTING_ORDER_DEFAULT )
        {
            color ??= Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color) color, textAnchor, textAlignment, sortingOrder);
        }

        // Create Text in the World
        private static TextMesh CreateWorldText( Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder ) {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
    }
}