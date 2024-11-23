using LuckiusDev.Utils;
using RapidPrototyping.Utils.Input;
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

        public override void Initialize()
        {
            base.Initialize();

            GameInputHandler.SetActionMap(GameInputHandler.ActionMapIndex.Default);

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
            m_coverageRenderTexture = new RenderTexture(160, 120, 0);
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

            // Read the pixels from the render texture
            RenderTexture.active = m_coverageRenderTexture;
            m_coverageTexture.ReadPixels(new Rect(0, 0, m_coverageRenderTexture.width, m_coverageRenderTexture.height), 0, 0);
            m_coverageTexture.Apply();
            RenderTexture.active = null;

            // Get the pixel data
            m_pixels = m_coverageTexture.GetPixels();

            int playerOnePixels = 0;
            int playerTwoPixels = 0;
            int totalPixels = m_pixels.Length;

            // Cache color values for faster comparison
            Vector3 playerOneRGB = new Vector3(m_playerOneColor.r, m_playerOneColor.g, m_playerOneColor.b);
            Vector3 playerTwoRGB = new Vector3(m_playerTwoColor.r, m_playerTwoColor.g, m_playerTwoColor.b);

            // Squared thresholds for faster comparison (avoiding square root)
            float thresholdSqrOne = 0.1f * 0.1f;  // Adjust this value as needed
            float thresholdSqrTwo = 0.3f * 0.3f;  // Slightly more lenient for player two if needed

            for (int i = 0; i < m_pixels.Length; i++)
            {
                Color pixel = m_pixels[i];
                if (pixel.a < 0.1f) continue; // Skip transparent pixels

                Vector3 pixelRGB = new Vector3(pixel.r, pixel.g, pixel.b);

                // Calculate squared distances
                float distSqrOne = SquaredColorDistance(pixelRGB, playerOneRGB);
                float distSqrTwo = SquaredColorDistance(pixelRGB, playerTwoRGB);

                // Classify pixel based on closest color within threshold
                if (distSqrOne < thresholdSqrOne)
                {
                    playerOnePixels++;
                }
                else if (distSqrTwo < thresholdSqrTwo)
                {
                    playerTwoPixels++;
                }
            }

            // Calculate coverage percentages
            float activePixels = playerOnePixels + playerTwoPixels;
            if (activePixels > 0)
            {
                m_playerOneCoverage = (float)playerOnePixels / totalPixels;
                m_playerTwoCoverage = (float)playerTwoPixels / totalPixels;
            }
            else
            {
                m_playerOneCoverage = 0f;
                m_playerTwoCoverage = 0f;
            }
        }

        #region Color Matching Methods

        private float SquaredColorDistance(Vector3 a, Vector3 b)
        {
            float dr = a.x - b.x;
            float dg = a.y - b.y;
            float db = a.z - b.z;
            return dr * dr + dg * dg + db * db;
        }

        // Optional: Keep a simplified version of ColorCloseTo for other use cases
        private bool ColorCloseTo(Color a, Color b, float threshold = 0.1f)
        {
            Vector3 colorA = new Vector3(a.r, a.g, a.b);
            Vector3 colorB = new Vector3(b.r, b.g, b.b);
            return SquaredColorDistance(colorA, colorB) < threshold * threshold;
        }

        #endregion

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