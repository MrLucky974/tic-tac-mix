using UnityEngine;

namespace RapidPrototyping.TicTacMix.Targets
{
    public class Destructible : MonoBehaviour
    {
        [SerializeField] private DestroyedObject m_destroyedInstancePrefab;

        public void Destroy()
        {
            Instantiate(m_destroyedInstancePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
