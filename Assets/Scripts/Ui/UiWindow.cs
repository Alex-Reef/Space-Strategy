using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace Ui
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UiWindow : MonoBehaviour
    {
        [SerializeField] private RectTransform animationTarget;
        private Vector2 _defaultPos;
        
        protected Canvas _canvas;

        public bool IsActive => _canvas.enabled;

        public event Action OnHide;
        
        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _defaultPos = animationTarget.anchoredPosition;
            Hide();
        }

        public virtual void Show()
        {
            if (_canvas.enabled)
                return;
            
            _canvas.enabled = true;
        }

        public virtual void Hide()
        {
            if (!_canvas.enabled)
                return;
            
            _canvas.enabled = false;
            OnHide?.Invoke();
        }
    }
}