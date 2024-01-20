using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tityx.Utilities
{
    public class TimeScaleSetter : MonoBehaviour
    {
        [SerializeField]
        private float _timeScale = 0f;

        private float _prevTimeScale = 1f;

        private void OnEnable()
        {
            _prevTimeScale = Time.timeScale;
            Time.timeScale = _timeScale;
        }

        private void OnDisable()
        {
            Time.timeScale = _prevTimeScale;
        }
    }
}