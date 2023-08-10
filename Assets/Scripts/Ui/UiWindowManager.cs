using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
    public class UiWindowManager : MonoBehaviour
    {
        [SerializeField] private UiWindowItem[] _windows;

        private Dictionary<int, UiWindow> _uiWindows;
        private UiWindow _currentWindow;

        private void Awake()
        {
            _uiWindows = new Dictionary<int, UiWindow>();
            for (int i = 0; i < _windows.Length; i++)
            {
                _uiWindows.Add((int)_windows[i].type, _windows[i].window);
            }
        }

        public void ShowWindow(UiWindowType type)
        {
            if(_uiWindows.TryGetValue((int)type, out var window))
            {
                window.Show();
            }
        }

        public void HideWindow(UiWindowType type)
        {
            if (_uiWindows.TryGetValue((int)type, out var window))
            {
                window.Hide();
            }
        }
    }
}