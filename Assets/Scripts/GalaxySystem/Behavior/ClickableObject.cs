using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GalaxySystem.Behavior
{
    public class ClickableObject : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClicked;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}