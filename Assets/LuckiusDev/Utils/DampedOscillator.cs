using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace LuckiusDev.Utils
{
    [DefaultExecutionOrder(-1000)]
    public static class DampedOscillator
    {
        private static readonly List<IAnimationInstance> Instances = new();
        private static readonly List<IAnimationInstance> Deletion = new();
        
        public delegate void AnimationProcessDelegate<T>(in T originalValue, in float displacement, in float scaleFactor);
        public delegate void AnimationEndDelegate<T>(in T originalValue);
        
        private interface IAnimationInstance
        {
            public void Process();
        }
        
        private class AnimationInstance<T> : IAnimationInstance
        {
            public event Action<AnimationInstance<T>> AnimationFinished;
            
            private readonly T _originalValue;
            private readonly AnimationProcessDelegate<T> _animation;
            private readonly AnimationEndDelegate<T> _reset;
            
            private readonly float _springForce;
            private readonly float _damp;

            private float _velocity;
            private float _displacement;
            private readonly float _scaleFactor;
            
            public AnimationInstance(AnimationProcessDelegate<T> animation, AnimationEndDelegate<T> reset, T originalValue, float springForce, float damp, float velocity, float scaleFactor)
            {
                _animation = animation;
                _originalValue = originalValue;
                
                _springForce = springForce;
                _damp = damp;

                _velocity = velocity;
                _scaleFactor = scaleFactor;
            }

            public void Process()
            {
                var force = -_springForce * _displacement - _damp * _velocity;
                _velocity += force * Time.deltaTime;
                _displacement += _velocity * Time.deltaTime;
                
                _animation?.Invoke(_originalValue, _displacement, _scaleFactor);

                if (Mathf.Abs(_velocity) < Mathf.Epsilon)
                {
                    _reset?.Invoke(_originalValue);
                    AnimationFinished?.Invoke(this);
                }
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Awake()
        {
            // Clear the lists
            Deletion.Clear();
            Instances.Clear();
            
            // Add the Update method to the player loop
            #region Player Loop

            var systemRoot = PlayerLoop.GetDefaultPlayerLoop();
            var updateSystem = new PlayerLoopSystem()
            {
                subSystemList = null,
                updateDelegate = Update,
                type = typeof(DampedOscillator)
            };
            
            PlayerLoopSystem dampedOscillatorLoop = new()
            {
                loopConditionFunction = systemRoot.loopConditionFunction,
                type = systemRoot.type,
                updateDelegate = systemRoot.updateDelegate,
                updateFunction = systemRoot.updateFunction
            };
            List<PlayerLoopSystem> subSystemList = new();
            foreach (var subSystem in systemRoot.subSystemList)
            {
                subSystemList.Add(subSystem);
                
                // Add the new update system after the physics update
                if (subSystem.type == typeof(Update))
                    subSystemList.Add(updateSystem);
            }
            dampedOscillatorLoop.subSystemList = subSystemList.ToArray();
            
            PlayerLoop.SetPlayerLoop(dampedOscillatorLoop);

            #endregion
        }

        private static void Update()
        {
            // Prevent the loop from updating in the Editor
            if (Application.isPlaying is false)
            {
                // Clear everything to be sure nothing gets updated
                Instances.Clear();
                Deletion.Clear();
                
                return;
            }
            
            // Remove every element queued for deletion from the Instances list
            foreach (var instance in Deletion)
            {
                if (Instances.Contains(instance))
                {
                    Instances.Remove(instance);
                }
            }
            Deletion.Clear(); // Clear the Deletion list
            
            // Update each animation instance
            foreach (var instance in Instances)
            {
                instance.Process();
            }
        }
        
        public static void Animate<T>(AnimationProcessDelegate<T> animation, AnimationEndDelegate<T> reset, T originalValue, 
            float springForce, float damp, float scaleFactor, float velocity)
        {
            var instance = new AnimationInstance<T>(animation, reset, originalValue, springForce, damp, velocity, scaleFactor);
            Instances.Add(instance);
            
            instance.AnimationFinished += (i) => Deletion.Add(i);
        }

        public static void Animate<T>(AnimationProcessDelegate<T> animation, AnimationEndDelegate<T> reset, T originalValue,
            DampedOscillatorParameters parameters, float velocity)
        {
            Animate(animation, reset, originalValue, parameters.SpringForce, parameters.Damp, parameters.ScaleFactor, velocity);
        }

        public static Vector3 Squash(Vector3 originalValue, float displacement, float factor)
        {
            return originalValue + new Vector3(displacement, -displacement, displacement) * factor;
        }
        
        public static Vector3 Stretch(Vector3 originalValue, float displacement, float factor)
        {
            return originalValue + new Vector3(-displacement, displacement, -displacement) * factor;
        }
    }
}