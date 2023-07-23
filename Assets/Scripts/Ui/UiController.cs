using System.Linq;
using UnityEngine;

namespace Ui
{
    [DisallowMultipleComponent]
    public class UiController : MonoBehaviour
    {
        [SerializeField] private UiWindow[] _uiWindows;
        public static UiController Instance { get; private set; }

        private UiWindow _currentOpened;
        
        private void Awake()
        {
            if(Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void ShowWindow(UiWindowType type)
        {
            _currentOpened = (from uiWindow in _uiWindows where uiWindow.Type == type select uiWindow).FirstOrDefault();
            if (_currentOpened != null)
            {
                _currentOpened.Show();
            }
        }

        private void HideWindow()
        {
            if(_currentOpened != null) _currentOpened.Hide();
            _currentOpened = null;
        }
    }
}
