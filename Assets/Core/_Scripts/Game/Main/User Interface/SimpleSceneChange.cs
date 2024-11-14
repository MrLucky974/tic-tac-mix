using LuckiusDev.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix
{
    public class SimpleSceneChange : MonoBehaviour
    {
        [SerializeField] private SceneReference m_mainMenuScene;

        public void ChangeScene()
        {
            if (m_mainMenuScene != null && !string.IsNullOrEmpty(m_mainMenuScene.SceneName))
            {
                SceneManager.LoadScene(m_mainMenuScene);
            }
        }
    }
}
