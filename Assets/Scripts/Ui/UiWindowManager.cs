using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
    public class UiWindowManager : MonoBehaviour
    {
        [SerializeField] private UiWindowItem[] _windows;

        private Dictionary<int, UiWindow> _uiWindows;

        private void Awake()
        {
            _uiWindows = new Dictionary<int, UiWindow>();
            for (int i = 0; i < _windows.Length; i++)
            {
                _uiWindows.Add((int)_windows[i].type, _windows[i].window);
            }
        }

        public UiWindowType[] GetAllWindow(bool onlyActive = false)
        {
            List<UiWindowType> windowTypes = new List<UiWindowType>();
            foreach (var uiWindow in _uiWindows)
            {
                if (onlyActive)
                {
                    if (uiWindow.Value.IsActive)
                        windowTypes.Add((UiWindowType)uiWindow.Key);
                }
                else
                {
                    windowTypes.Add((UiWindowType)uiWindow.Key);
                }
            }

            return windowTypes.ToArray();
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

        public bool IsActive(UiWindowType type)
        {
            if (_uiWindows.TryGetValue((int)type, out var window))
            {
                return window.IsActive;
            }

            return false;
        }
    }
}