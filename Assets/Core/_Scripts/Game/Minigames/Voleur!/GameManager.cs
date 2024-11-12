using LuckiusDev.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix.Voleur
{
    public class GameManager : MonoBehaviour
    {
        public const int TIE_INDEX = -1;
        public const int PLAYER_ONE_INDEX = 0;
        public const int PLAYER_TWO_INDEX = 1;

        [Header("End1_HasbeenWatched")]
        private bool playerOIsAlive = true;
        private bool playerXIsAlive = true;

        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private TMP_Text _text;

        [SerializeField] private Color[] _color;

        [Header("End2_EndTimeBetterScore")]
        //CHRONO OF THE GAME
        private float _time;
        [SerializeField] private float _endTime;

        private int _chrono;
        [SerializeField] private TMP_Text _chronoText;

        private PlayerController[] _playerController;
        [SerializeField] private TMP_Text[] _scoreText;

        [Header("Countdown")]
        [SerializeField]
        private TMP_Text _countdownText;
        public bool _canMove = false;

        private CountdownTimer m_timer;

        [Header("Audio")]
        [SerializeField] private AudioClip[] _audioClip;

        private void Start()
        {
            m_timer = new CountdownTimer(3f);
            m_timer.OnTimerStop += () =>
            {
                LoadGameplaySceneForNextTurn();
            };

            _playerController = FindObjectsOfType<PlayerController>();

            if (!_canMove)
            {
                StartCoroutine(Countdown());
            }
        }

        private void Update()
        {
            m_timer.Tick(Time.unscaledDeltaTime);

            if (_canMove)
            {
                _time += Time.deltaTime;
                _chrono = (int)_time;
                _chronoText.text = _chrono.ToString();

                if (_time >= _endTime)
                {
                    _victoryPanel.SetActive(true);
                    Score();
                    _canMove = false;

                    if (_time <= 0)
                    {
                        _time = 0;
                    }
                }
            }
        }

        IEnumerator Countdown()
        {
            _countdownText.gameObject.SetActive(true);

            _countdownText.text = "3";
            SoundManager.Play(_audioClip[0]);
            yield return new WaitForSeconds(1f);
            _countdownText.text = "2";
            SoundManager.Play(_audioClip[0]);
            yield return new WaitForSeconds(1f);
            _countdownText.text = "1";
            SoundManager.Play(_audioClip[0]);
            yield return new WaitForSeconds(1f);
            _countdownText.text = "GO!";
            SoundManager.Play(_audioClip[1]);
            yield return new WaitForSeconds(0.5f);


            _countdownText.gameObject.SetActive(false);

            _canMove = true;
        }

        public void PlayerFinished(bool isPlayerO)
        {
            if (isPlayerO)
            {
                playerOIsAlive = false;
                Debug.Log("PlayerO is dead");
            }
            else
            {
                playerXIsAlive = false;
                Debug.Log("Player X is dead");
            }

            DetermineWinner();

        }

        private void DetermineWinner()
        {
            Time.timeScale = 0f;
            _victoryPanel.SetActive(true);

            int winIndex = TIE_INDEX;
            if (!playerOIsAlive && playerXIsAlive)
            {
                _text.GetComponent<TMP_Text>().color = _color[0];
                _text.text = "Victory: Player X";
                winIndex = PLAYER_ONE_INDEX;
                SoundManager.Play(_audioClip[2]);
            }
            else if (!playerXIsAlive && playerOIsAlive)
            {
                _text.GetComponent<TMP_Text>().color = _color[1];
                _text.text = "Victory: Player O";
                winIndex = PLAYER_TWO_INDEX;
                SoundManager.Play(_audioClip[2]);
            }
            else if (!playerOIsAlive && !playerXIsAlive)
            {
                _text.text = "Victory: Tie";
                SoundManager.Play(_audioClip[3]);
            }

            ShowScoreText();
            MarkWinningSymbol(winIndex);
            m_timer.Start();
        }

        void Score()
        {
            int winIndex = TIE_INDEX;
            if (_playerController[0]._obtainedCake > _playerController[1]._obtainedCake)
            {
                if (_playerController[0]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = _color[1];
                    _text.text = "Victory: Player O";
                    winIndex = PLAYER_TWO_INDEX;

                    SoundManager.Play(_audioClip[2]);
                }
            }
            else if (_playerController[0]._obtainedCake < _playerController[1]._obtainedCake)
            {
                if (!_playerController[1]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = _color[0];
                    _text.text = "Victory: Player X";
                    winIndex = PLAYER_ONE_INDEX;

                    SoundManager.Play(_audioClip[2]);
                }
            }
            else if (_playerController[0]._obtainedCake == _playerController[1]._obtainedCake)
            {
                _text.text = "Victory: Tie";

                SoundManager.Play(_audioClip[3]);
            }

            ShowScoreText();
            MarkWinningSymbol(winIndex);
            m_timer.Start();
        }

        void ShowScoreText()
        {
            _scoreText[0].text = _playerController[0]._obtainedCake.ToString();
            _scoreText[1].text = _playerController[1]._obtainedCake.ToString();
        }

        /// <summary>
        /// Advances the game by changing the player's turn and loading the main gameplay scene.
        /// </summary>
        public static void LoadGameplaySceneForNextTurn()
        {
            // Change the active player turn in GameDataHandler.
            GameDataHandler.ChangeTurn();

            // Set the time scale back to 1
            Time.timeScale = 1f;

            // Load the main gameplay scene based on the configured scene reference.
            SceneManager.LoadScene(GameDataHandler.MainGameplaySceneReference);
        }

        /// <summary>
        /// Marks the winning symbol on the grid at the specified position when a game ends.
        /// </summary>
        /// <param name="winIndex">
        /// The index indicating the winning player:
        /// - 0 for player using "Cross"
        /// - 1 for player using "Circle"
        /// - -1 if there is no winner (in which case, no symbol is placed).
        /// </param>
        public static void MarkWinningSymbol(int winIndex)
        {
            // Retrieve the current grid position from GameDataHandler.
            var gridPosition = GameDataHandler.DataHolder.GridPosition;

            // Check if there is a winning player.
            if (winIndex != TIE_INDEX)
            {
                // Determine the symbol to place based on the winning player's index.
                var symbol = winIndex == PLAYER_ONE_INDEX ? GridManager.Symbol.Cross : GridManager.Symbol.Circle;

                // Place the winning symbol on the grid at the designated position.
                GridManager.PlaceSymbol(symbol, gridPosition.x, gridPosition.y);
            }
        }
    }
}

