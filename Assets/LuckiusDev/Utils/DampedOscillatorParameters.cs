using System;
using UnityEngine;

namespace LuckiusDev.Utils
{
    [Serializable]
    public struct DampedOscillatorParameters
    {
        [field: SerializeField, Range(0f, 1000f)]
        public float SpringForce { get; private set; }

        [field: SerializeField, Range(0f, 50f)]
        public float Damp { get; private set; }

        [field: SerializeField, Min(0)] public float ScaleFactor { get; private set; }

        public DampedOscillatorParameters(float springForce, float damp, float scaleFactor)
        {
            SpringForce = springForce;
            Damp = damp;
            ScaleFactor = scaleFactor;
        }
    }

}