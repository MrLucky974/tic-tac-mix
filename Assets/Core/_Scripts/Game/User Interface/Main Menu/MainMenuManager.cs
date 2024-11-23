using LuckiusDev.Experiments;
using LuckiusDev.Utils;
using UnityEditor;
using UnityEngine;

namespace LBB.BRA1NFvCK
{
    public enum MainMenuScreenType
    {
        MainMenu,
        Settings,
        Credits
    }

    public class MainMenuManager : UIMenuManager<MainMenuScreenType>
    {
        [Header("Scenes")]
        [SerializeField] private SceneReference m_gameplayScene;

        public void Play()
        {
            if (m_gameplayScene != null && !string.IsNullOrEmpty(m_gameplayScene))
            {
                SceneTransitionManager.Load(m_gameplayScene);
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
