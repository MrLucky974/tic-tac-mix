using UnityEngine;

namespace RapidPrototyping.TicTacMix.Marelle
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private bool _isPlayerO;
        [SerializeField] private GameManager _gameManager;

        private void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Victory"))
            {
                _gameManager.PlayerFinished(_isPlayerO);
                Time.timeScale = 0;
            }
        }
    }
}
