using LuckiusDev.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix
{
    [CreateAssetMenu(menuName = "Tic Tac Mix/New Minigame Data")]
    public class MinigameData : BaseScriptableObject
    {
        public string Name;
        public SceneReference[] Scenes;

        public string GetRandomSceneName()
        {
            var reference = Scenes.PickRandomUnity();
            return reference.SceneName;
        }

        public int SelectRandomSceneAsIndex()
        {
            var reference = Scenes.PickRandomUnity();
            var scene = SceneManager.GetSceneByName(reference);
            return scene.buildIndex;
        }
    }
}
