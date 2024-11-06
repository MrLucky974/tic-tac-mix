using LuckiusDev.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LBB.BRA1NFvCK
{
    public class SceneLoad : MonoBehaviour
    {
        [SerializeField] private SceneReference m_sceneReference;

        public void LoadScene()
        {
            if (m_sceneReference != null && string.IsNullOrEmpty(m_sceneReference.SceneName) is false)
            {
                SceneManager.LoadScene(m_sceneReference);
                Time.timeScale = 1f;
            }
        }
    }
}
