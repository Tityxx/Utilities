using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tityx.Utilities
{
    public class TextEffect : MonoBehaviour
    {
        [SerializeField] private Transform _rotator;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private bool _needRotate;
        [SerializeField] private float _angle;

        public void Spawn(string value)
        {
            _text.text = value;

            if (_needRotate)
            {
                _rotator.localEulerAngles = Vector3.forward * Random.Range(-_angle, _angle);
            }
            else
            {
                _rotator.localEulerAngles = Vector3.zero;
            }
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}