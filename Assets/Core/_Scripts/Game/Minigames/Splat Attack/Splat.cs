using UnityEngine;

namespace RapidPrototyping.TicTacMix.SplatAttack
{
    public class Splat : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_renderer;
        [SerializeField] private SpriteRenderer m_coverageRenderer;

        [SerializeField] private Sprite[] m_sprites;

        private PlayerIdentifier m_identifier;
        public PlayerIdentifier Identifier => m_identifier;

        // Static counter for sorting order
        private static int s_currentSortingOrder = 0;

        public Splat Create(PlayerIdentifier identifier)
        {
            // Create a new instance of the splat
            var instance = Instantiate(this);

            // Select a random splat sprite
            var sprite = m_sprites.PickRandomUnity();
            instance.m_renderer.sprite = sprite;
            instance.m_coverageRenderer.sprite = sprite;

            // Randomize the splat rotation
            //var radians = Random.value * 2f * Mathf.PI;
            //instance.m_renderer.transform.rotation = Quaternion.Euler(Vector3.forward * radians * Mathf.Rad2Deg);

            // Increment and set the sorting order for the new splat
            s_currentSortingOrder++;
            instance.m_renderer.sortingOrder = s_currentSortingOrder;
            instance.m_coverageRenderer.sortingOrder = s_currentSortingOrder;

            // Set the color of the splat depending on the player
            var color = GameManager.GetColor(identifier) * new Color(1f, 0.8f, 0.8f);
            instance.m_renderer.color = color;
            instance.m_coverageRenderer.color = color;

            instance.m_identifier = identifier;
            GameManager.AddSplat(this, identifier);

            return instance;
        }
    }
}
