using LuckiusDev.Utils;
using RapidPrototyping.Utils.Input;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix.Trains
{
    public class GameManager : MonoBehaviour
    {
        public const int TIE_INDEX = -1;
        public const int PLAYER_ONE_INDEX = 0;
        public const int PLAYER_TWO_INDEX = 1;

        [Header("CHRONO")]
        //CHRONO OF THE GAME
        private float _time;
        [SerializeField] private float _endTime;

        private int _chrono;
        [SerializeField] private TMP_Text _chronoText;


        [Header("Score")]

        [SerializeField] private GameObject _victoryPanel;
        private PlayerController[] _player;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text[] _scoreText;

        [SerializeField] private Color[] _colors;

        [Header("Countdown")]
        [SerializeField] private TMP_Text _countdownText;
        public bool _canSpawn;

        private CountdownTimer m_timer;

        [Header("Audio")]
        [SerializeField] private AudioClip[] _audioClip;

        private void Start()
        {
            _player = FindObjectsOfType<PlayerController>();
            _canSpawn = false;
            StartCoroutine(Countdown());

            GameInputHandler.SetActionMap(GameInputHandler.ActionMapIndex.Default);
            m_timer = new CountdownTimer(3f);
            m_timer.OnTimerStop += () =>
            {
                LoadGameplaySceneForNextTurn();
            };
        }

        private void Update()
        {
            m_timer.Tick(Time.unscaledDeltaTime);

            if (_canSpawn)
            {
                _time += Time.deltaTime;
                _chrono = (int)_time;
                _chronoText.text = _chrono.ToString();

                if (_time >= _endTime)
                {
                    Score();
                    _canSpawn=false;

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
            _canSpawn = true;

            for (int i = 0; i < _player.Length; i++)
            {
                _player[i]._speed = 5;
            }
        }

        public void Score()
        {
            _victoryPanel.SetActive(true);

            //Player[0] = X & Player[1] = O
            if (_player[0]._score > _player[1]._score)
            {

                if (_player[0]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = _colors[1];
                    _text.text = "Victory: Player O";
                    MarkWinningSymbol(PLAYER_TWO_INDEX);

                    SoundManager.Play(_audioClip[2]);
                }
                else
                {
                    _text.GetComponent<TMP_Text>().color = _colors[0];
                    _text.text = "Victory: Player X";
                    MarkWinningSymbol(PLAYER_ONE_INDEX);

                    SoundManager.Play(_audioClip[2]);
                }
            }
            else if (_player[0]._score < _player[1]._score)
            {

                if (!_player[1]._isPlayerO)
                {
                    _text.GetComponent<TMP_Text>().color = _colors[0];
                    _text.text = "Victory: Player X";
                    MarkWinningSymbol(PLAYER_ONE_INDEX);

                    SoundManager.Play(_audioClip[2]);

                }
                else
                {
                    _text.GetComponent<TMP_Text>().color = _colors[1];
                    _text.text = "Victory: Player O";
                    MarkWinningSymbol(PLAYER_TWO_INDEX);

                    SoundManager.Play(_audioClip[2]);

                }
            }
            else if (_player[0]._score == _player[1]._score)
            {
                _text.text = "Victory: Tie";
                MarkWinningSymbol(TIE_INDEX);

                SoundManager.Play(_audioClip[3]);
            }

            ShowScoreText();
            Time.timeScale = 0f;
            m_timer.Start();
        }

        public void Tie()
        {
            _victoryPanel.SetActive(true);
            _text.text = "Victory: Tie";
            MarkWinningSymbol(TIE_INDEX);
            ShowScoreText();
            Time.timeScale = 0f;
            m_timer.Start();

            SoundManager.Play(_audioClip[3]);
        }

        public void PlayerDead(bool isPlayerO)
        {
            _victoryPanel.SetActive(true);

            if (isPlayerO)
            {
                _text.GetComponent<TMP_Text>().color = _colors[0];
                _text.text = "Victory: Player X";
                MarkWinningSymbol(PLAYER_ONE_INDEX);

                SoundManager.Play(_audioClip[2]);
            }
            else
            {
                _text.GetComponent<TMP_Text>().color = _colors[1];
                _text.text = "Victory: Player O";
                MarkWinningSymbol(PLAYER_TWO_INDEX);

                SoundManager.Play(_audioClip[2]);
            }

            ShowScoreText();
            Time.timeScale = 0f;
            m_timer.Start();
        }

        private void ShowScoreText()
        {
            _scoreText[0].text = _player[0]._score.ToString();
            _scoreText[1].text = _player[1]._score.ToString();
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
