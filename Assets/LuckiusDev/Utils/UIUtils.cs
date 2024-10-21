using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LuckiusDev.Utils
{
    public static class UIUtils
    {
        private static PointerEventData _eventData;
        private static List<RaycastResult> _results;

        public static bool OverUI()
        {
            _eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventData, _results);
            return _results.Count > 0;
        }

        public static Vector2 GetCanvasElementWorldPosition(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera.main,
                    out var result);
            return result;
        }

        public static void Lerp(this Slider slider, float targetValue, float delta)
        {
            float currentValue = slider.value;
            slider.value = Mathf.Lerp(currentValue, targetValue, delta);
        }
    }
}