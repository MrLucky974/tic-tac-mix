using System;
using System.Collections;
using UnityEngine;

namespace LuckiusDev.Utils
{
    public class CoroutineTimer
    {
        public event Action TimerComplete;
        
        private readonly float _delay;
        public float TimeLeft { get; private set; }

        private IEnumerator _timerCoroutine;
        
        public CoroutineTimer(float delay)
        {
            _delay = delay;    
        }

        public void Stop(MonoBehaviour mono)
        {
            mono.StopCoroutine(_timerCoroutine);
        }

        public void Start(MonoBehaviour mono)
        {
            _timerCoroutine = TimerCoroutine();
            mono.StartCoroutine(_timerCoroutine);
        }

        private IEnumerator TimerCoroutine()
        {
            float eventTime = Time.time + _delay;
            while (Time.time < eventTime)
            {
                TimeLeft = eventTime - TimeLeft;
                yield return null;
            }

            yield return null;
            
            TimerComplete?.Invoke();
        }
    }
}
