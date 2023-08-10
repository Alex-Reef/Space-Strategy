using System;
using UnityEngine;

namespace Ui
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UiWindow : MonoBehaviour
    {
        protected Canvas _canvas;

        public event Action OnHide;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public virtual void Show()
        {
            _canvas.enabled = true;
        }

        public virtual void Hide()
        {
            _canvas.enabled = false;
            OnHide?.Invoke();
        }
    }
}