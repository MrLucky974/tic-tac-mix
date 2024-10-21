using System;
using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// A component that enables the game object to look at the camera,
    /// with the ability to lock rotation axes individually.
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        private enum BillboardType
        {
            LOOK_AT_CAMERA,
            CAMERA_FORWARD
        }

        [Flags]
        private enum RotationLock
        {
            NONE = 0,
            LOCK_X = 1 << 0,
            LOCK_Y = 1 << 1,
            LOCK_Z = 1 << 2
        }

        [SerializeField] private BillboardType m_billboardType;

        [Header("Lock Rotation")] [SerializeField]
        private RotationLock m_rotationLock;

        private Vector3 _originalRotation;

        private void Awake()
        {
            _originalRotation = transform.rotation.eulerAngles;
        }

        // LateUpdate() is used to make sure the object is not moving
        private void LateUpdate()
        {
            // There are two ways to billboard things
            switch (m_billboardType)
            {
                case BillboardType.LOOK_AT_CAMERA:
                    transform.LookAt(Camera.main.transform.position, Vector3.up);
                    break;
                case BillboardType.CAMERA_FORWARD:
                    transform.forward = Camera.main.transform.forward;
                    break;
                default:
                    break;
            }

            // Modify the rotation in Euler space to lock certain dimensions
            Vector3 rotation = transform.rotation.eulerAngles;
            if (m_rotationLock.HasFlag(RotationLock.LOCK_X))
            {
                rotation.x = _originalRotation.x;
            }

            if (m_rotationLock.HasFlag(RotationLock.LOCK_Y))
            {
                rotation.y = _originalRotation.y;
            }

            if (m_rotationLock.HasFlag(RotationLock.LOCK_Z))
            {
                rotation.z = _originalRotation.z;
            }

            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}