using UnityEngine;

namespace RapidPrototyping.TicTacMix.MysteryDoors
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] private Transform m_leftHandle;
        [SerializeField] private Transform m_rightHandle;

        [HideInInspector] public Floor nextFloor;

        public Vector3 GetPosition(float offset)
        {
            offset = Mathf.Clamp01(offset);
            return Vector3.Lerp(m_leftHandle.position,
                m_rightHandle.position, offset);
        }
    }
}
