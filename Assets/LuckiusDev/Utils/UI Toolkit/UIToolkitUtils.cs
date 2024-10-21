using UnityEngine.UIElements;

namespace LuckiusDev.Utils.UIToolkit
{
    public struct UIToolkitUtils
    {
        public static VisualElement Create( params string[] classNames ) {
            return Create<VisualElement>(classNames);
        }
        
        public static T Create<T>(params string[] classNames ) where T : VisualElement, new() {
            var element = new T();
            foreach (var className in classNames)
            {
                element.AddToClassList(className);
            }
            return element;
        }
        
        public static Button Button(string text = "", params string[] classNames)
        {
            var button = Create<Button>(classNames);
            button.text = text;

            return button;
        }
    }
}