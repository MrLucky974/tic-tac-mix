using LuckiusDev.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace RapidPrototyping.TicTacMix.SplatAttack
{
    public enum PlayerIdentifier
    {
        PLAYER_ONE,
        PLAYER_TWO,
    }

    public struct GameData
    {
        public int PlayerIndex;
    }

    public class GameManager : MinigameManager<GameData>
    {
        [SerializeField] private GameTimer m_gameTimer;

        [SerializeField] private Color m_playerOneColor;
        [SerializeField] private Color m_playerTwoColor;

        [Space]

        [SerializeField] private Transform m_splatContainer;
        public static Transform SplatContainer => ((GameManager)Instance).m_splatContainer;

        [SerializeField] private RectTransform m_outline;

        // Add reference to the render texture and camera for coverage calculation
        private RenderTexture m_coverageRenderTexture;
        public static RenderTexture CoverageRenderTexture => ((GameManager)Instance).m_coverageRenderTexture;

        private Camera m_coverageCamera;

        private Rect m_worldRect;
        public static Rect WorldRect => ((GameManager)Instance).m_worldRect;

        private List<Splat> m_playerOneSplats = new List<Splat>();
        private List<Splat> m_playerTwoSplats = new List<Splat>();

        // Arrays to store coverage calculation data
        private Texture2D m_coverageTexture;
        private Color[] m_pixels;

        // Coverage values
        private float m_playerOneCoverage;
        private float m_playerTwoCoverage;

        private CountdownTimer m_timer;

        private void Start()
        {
            m_worldRect = JMath.GetWorldSpaceRect(m_outline);
            InitializeCoverageCalculation();

            m_timer = new CountdownTimer(3f);
            m_timer.OnTimerStop += () =>
            {
                if (m_timer.IsFinished is false)
                    return;

                LoadGameplaySceneForNextTurn();
            };
        }

        private void Update()
        {
            var unscaledDeltaTime = Time.unscaledDeltaTime;
            m_timer.Tick(unscaledDeltaTime);
        }

        private void LateUpdate()
        {
            CalculateCoverage();
        }

        public void ConcludeGameOnTimeout()
        {
            Time.timeScale = 0f;

            var gameManager = ((GameManager)Instance);
            int winIndex = TIE_INDEX;
            if (gameManager.m_playerOneCoverage > gameManager.m_playerTwoCoverage)
            {
                winIndex = PLAYER_ONE_INDEX;
            }
            else if (gameManager.m_playerOneCoverage < gameManager.m_playerTwoCoverage)
            {
                winIndex = PLAYER_TWO_INDEX;
            }

            m_gameTimer.Stop();

            var data = new GameData
            {
                PlayerIndex = winIndex,
            };
            EndGame(data);

            MarkWinningSymbol(data.PlayerIndex);
            gameManager.m_timer.Start();
        }

        public static float GetPlayerCoverage(PlayerIdentifier identifier)
        {
            var instance = (GameManager)Instance;
            return identifier == PlayerIdentifier.PLAYER_ONE ?
                instance.m_playerOneCoverage :
                instance.m_playerTwoCoverage;
        }

        private void InitializeCoverageCalculation()
        {
            //// Create a render texture to capture the splats
            m_coverageRenderTexture = new RenderTexture(320, 240, 0);
            m_coverageRenderTexture.antiAliasing = 1;

            // Create a texture2D to read pixels into
            m_coverageTexture = new Texture2D(m_coverageRenderTexture.width, m_coverageRenderTexture.height,
                TextureFormat.RGB24, false);

            // Allocate pixel array
            m_pixels = new Color[m_coverageRenderTexture.width * m_coverageRenderTexture.height];

            // Setup coverage camera
            if (m_coverageCamera == null)
            {
                var cameraGo = new GameObject("Coverage Camera");
                m_coverageCamera = cameraGo.AddComponent<Camera>();
                m_coverageCamera.orthographic = true;
                m_coverageCamera.orthographicSize = m_worldRect.height / 2f;
                m_coverageCamera.transform.position = new Vector3(m_worldRect.center.x, m_worldRect.center.y, -10f);
                m_coverageCamera.transform.rotation = Quaternion.identity;
                m_coverageCamera.clearFlags = CameraClearFlags.SolidColor;
                m_coverageCamera.backgroundColor = Color.clear;
                m_coverageCamera.cullingMask = LayerMask.GetMask("Coverage"); // Adjust to your splat layer
                m_coverageCamera.targetTexture = m_coverageRenderTexture;
            }
        }

        private void CalculateCoverage()
        {
            // Render the splats to the render texture
            m_coverageCamera.Render();

            //// Read the pixels from the render texture
            RenderTexture.active = m_coverageRenderTexture;
            m_coverageTexture.ReadPixels(new Rect(0, 0, m_coverageRenderTexture.width, m_coverageRenderTexture.height), 0, 0);
            m_coverageTexture.Apply();
            RenderTexture.active = null;

            // Get the pixel data
            m_pixels = m_coverageTexture.GetPixels();

            int playerOnePixels = 0;
            int playerTwoPixels = 0;
            int totalPixels = m_pixels.Length;

            // Count pixels for each player based on their colors
            Color playerOneColor = m_playerOneColor;
            Color playerTwoColor = m_playerTwoColor;

            foreach (Color pixel in m_pixels)
            {
                if (pixel.a < 0.1f) continue; // Skip transparent pixels

                // TODO : ColorCloseTo work weird on P2 color
                // Compare the pixel color to each player's color
                // Using approximate comparison due to potential slight variations in rendered colors
                if (ColorCloseTo(pixel, playerOneColor))
                    playerOnePixels++;
                else if (ColorCloseTo(pixel, playerTwoColor, 0.5f))
                    playerTwoPixels++;
            }

            // Calculate coverage percentages
            m_playerOneCoverage = (float)playerOnePixels / totalPixels;
            m_playerTwoCoverage = (float)playerTwoPixels / totalPixels;
        }

        private bool ColorCloseTo(Color a, Color b, float threshold = 0.1f)
        {
            return Mathf.Abs(a.r - b.r) < threshold &&
                   Mathf.Abs(a.g - b.g) < threshold &&
                   Mathf.Abs(a.b - b.b) < threshold;
        }

        public static void AddSplat(Splat splat, PlayerIdentifier identifier)
        {
            switch (identifier)
            {
                case PlayerIdentifier.PLAYER_ONE:
                    ((GameManager)Instance).m_playerOneSplats.Add(splat);
                    break;
                case PlayerIdentifier.PLAYER_TWO:
                    ((GameManager)Instance).m_playerTwoSplats.Add(splat);
                    break;
            }
        }

        public static Color GetColor(PlayerIdentifier identifier)
        {
            switch (identifier)
            {
                case PlayerIdentifier.PLAYER_ONE:
                    return ((GameManager)Instance).m_playerOneColor;
                case PlayerIdentifier.PLAYER_TWO:
                    return ((GameManager)Instance).m_playerTwoColor;
                default:
                    return Color.white;
            }
        }
    }
}