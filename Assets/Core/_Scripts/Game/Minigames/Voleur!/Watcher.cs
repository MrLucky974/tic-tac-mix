using UnityEngine;

namespace RapidPrototyping.TicTacMix.Voleur
{
    public class Watcher : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerController[] _playerController;
        [SerializeField] private int m_minimumDelay = 3;
        [SerializeField] private int m_maximumDelay = 8;

        [Header("StartWatchDelay")]
        private float _time;
        private float _targetTime; //Random

        [Header("WatchingDelay")]
        private bool _isWatching;
        private float _watchTime;
        private float _targetWatchTime; //Random

        [Header("Animation")]
        private Animator _animator;

        [Header("GameManager")]
        private GameManager _gameManager;

        [Header("Audio")]
        [SerializeField] private AudioClip[] _audioClip;

        private void Start()
        {
            _playerController = FindObjectsOfType<PlayerController>();
            _gameManager = FindObjectOfType<GameManager>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {

            _time += Time.deltaTime;

            if (_time >= _targetTime)
            {
                _animator.SetBool("isWatching", true);
                _targetTime = Random.Range(m_minimumDelay, m_maximumDelay);
                _time = 0;
            }


            if (_isWatching)
            {
                Watch();
            }
        }

        public void SoundWatch()
        {
            SoundManager.Play(_audioClip[0]);

        }
        
        public void SoundPrevent()
        {
            SoundManager.Play(_audioClip[1]);

        }
        
        public void IsWatching()
        {
            _isWatching = true;
        }

        public void Watch()
        {
            foreach (PlayerController playerController in _playerController)
            {
                if (playerController._isTakingCake)
                {
                    //END
                    _gameManager.PlayerFinished(playerController._isPlayerO);
                    _isWatching = false;
                    print("end" + playerController.name);
                }
            }

            IsWatchingDelay();
        }

        public void IsWatchingDelay()
        {
            _watchTime += Time.deltaTime;

            if (_watchTime >= _targetWatchTime)
            {
                _targetWatchTime = Random.Range(m_minimumDelay, m_maximumDelay);
                _animator.SetBool("isWatching", false);
                _isWatching = false;
                _watchTime = 0;
            }
        }
    }
}
