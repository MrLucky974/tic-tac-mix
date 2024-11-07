using LuckiusDev.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix.Marelle
{
    public class GameManager : MonoBehaviour
    {
        public const int TIE_INDEX = -1;
        public const int PLAYER_ONE_INDEX = 0;
        public const int PLAYER_TWO_INDEX = 1;

        [SerializeField] private bool _isFinishedO;
        [SerializeField] private bool _isFinishedX;

        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private TMP_Text _text;

        [SerializeField] private Color[] _color;

        [Header("CHRONO")]
        //CHRONO OF THE GAME
        private float _time;
        [SerializeField] private float _endTime;
        private int _chrono;
        [SerializeField] private TMP_Text _chronoText;

        [Header("ArrowSequence")]
        private ArrowSequence _arrowSequence;

        private CountdownTimer m_timer;

        private void Start()
        {
            m_timer = new CountdownTimer(3f);
            m_timer.OnTimerStop += () =>
            {
                LoadGameplaySceneForNextTurn();
            };

            _arrowSequence = GetComponent<ArrowSequence>();
        }

        private void Update()
        {
            if (_arrowSequence._canMove)
            {
                _time += Time.deltaTime;
                _chrono = (int)_time;
                _chronoText.text = _chrono.ToString();

                if (_time >= _endTime)
                {
                    //_victoryPanel.SetActive(true);
                    DetermineWinner();
                    _arrowSequence._canMove = false;

                    if (_time <= 0)
                    {
                        _time = 0;
                    }
                }
            }

            m_timer.Tick(Time.unscaledDeltaTime);
        }

        public void PlayerFinished(bool isPlayerO)
        {
            _arrowSequence._canMove = false;

            if (isPlayerO)
            {
                _isFinishedO = true;
            }
            else
            {
                _isFinishedX = true;
            }

            DetermineWinner();
        }

        private void DetermineWinner()
        {
            Time.timeScale = 0;
            _victoryPanel.SetActive(true);

            int winIndex = TIE_INDEX;
            if (_isFinishedO)
            {
                _text.GetComponent<TMP_Text>().color = _color[0];
                _text.text = "Victory: Player O";
                winIndex = PLAYER_TWO_INDEX;
            }
            else if (_isFinishedX)
            {
                _text.GetComponent<TMP_Text>().color = _color[1];
                _text.text = "Victory: Player X";
                winIndex = PLAYER_ONE_INDEX;
            }
            else
            {
                _text.text = "Victory: Tie";
            }

            MarkWinningSymbol(winIndex);
            m_timer.Start();
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
