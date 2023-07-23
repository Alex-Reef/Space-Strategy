using System;
using UnityEngine;

namespace Ui
{
    [RequireComponent(typeof(Canvas)), DisallowMultipleComponent]
    public abstract class UiWindow : MonoBehaviour
    {
        private Canvas _canvas;
        public UiWindowType Type;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        protected virtual void Init() { }

        public virtual void Show()
        {
            _canvas.enabled = true;
        }

        public virtual void Hide()
        {
            _canvas.enabled = false;
        }
    }
}