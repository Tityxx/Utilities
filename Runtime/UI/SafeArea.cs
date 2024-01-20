using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tityx.Utilities.UI
{
    /// <summary>
    /// Обработка SafeArea
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        private RectTransform rect;

        private void Awake()
        {
            rect = transform as RectTransform;
            SetSafeArea();
        }

#if UNITY_EDITOR
        private void Update()
        {
            SetSafeArea();
        }
#endif

        private void SetSafeArea()
        {
            Vector2 minAnchor = Screen.safeArea.position;
            Vector2 maxAnchor = Screen.safeArea.size + minAnchor;

            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            rect.anchorMin = minAnchor;
            rect.anchorMax = maxAnchor;
        }
    }
}