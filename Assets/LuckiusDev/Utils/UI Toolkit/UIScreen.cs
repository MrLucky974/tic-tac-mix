using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace LuckiusDev.Utils.UIToolkit
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(UIDocument))]
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField] protected UIDocument m_Document;
        [SerializeField] protected StyleSheet m_StyleSheet;

        private void Awake() {
            // Check for UIDocument component
            if (m_Document == null)
                m_Document = GetComponent<UIDocument>();

            Debug.Assert(m_Document != null, "UI Document not specified.", this);
        }
        private void OnEnable() {
            StartCoroutine(Generate());
        }

        private void OnValidate() {
            // Update the UI Screen in the Editor
            if (Application.isPlaying) return; // Only validate in Edit mode
            if (!gameObject.activeInHierarchy) return; // Only validate if game object is enabled
            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            // Wait a frame to prevent null exception error
            // when going from Play to Edit mode
            yield return null;
            
            // Create root element
            var root = m_Document.rootVisualElement;
            root.Clear();
            
            // Add base stylesheet
            if (m_StyleSheet)
                root.styleSheets.Add(m_StyleSheet);
            
            // Call virtual method
            CreateScreenGUI(root);
        }

        protected virtual void CreateScreenGUI( VisualElement root ) {
            throw new NotImplementedException();
        }
    }
}
