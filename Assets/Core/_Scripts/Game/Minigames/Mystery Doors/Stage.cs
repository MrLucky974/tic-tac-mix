using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public abstract class Stage : MonoBehaviour
    {
        [SerializeField] private Transform m_leftHandle;
        [SerializeField] private Transform m_rightHandle;

        private int m_index = -1;
        public int Index => m_index;

        public void SetIndex(int index) => m_index = index;

        public Vector3 GetPosition(float offset)
        {
            offset = Mathf.Clamp01(offset);
            return Vector3.Lerp(m_leftHandle.position,
                m_rightHandle.position, offset);
        }

        public float GetDistance()
        {
            return Vector3.Distance(m_leftHandle.position, m_rightHandle.position);
        }
    }
}
