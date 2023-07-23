using System;
using Ui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GalacticSystems
{
    [DisallowMultipleComponent]
    public class StarSystemBehavior : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMesh systemNameText;
        private StarSystemInfo _starSystemInfo;

        public Vector3 Position;
        public event Action<StarSystemInfo> Selected;

        public void Init(StarSystemInfo info)
        {
            _starSystemInfo = info;
            systemNameText.text = _starSystemInfo.SystemName;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Selected?.Invoke(_starSystemInfo);
            
            UiController.Instance.ShowWindow(UiWindowType.StarSystemInfo);
        }
    }
}