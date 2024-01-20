using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tityx.Utilities.UI
{
    /// <summary>
    /// Абстрактый класс текста
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class AbstractText : MonoBehaviour
    {
        protected TMP_Text _text;

        protected virtual void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }
    }
}