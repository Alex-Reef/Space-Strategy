using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GalaxySystem.Behavior
{
    [DisallowMultipleComponent]
    public class StarSystemBehavior : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMesh textMesh;

        private int SystemId;
        
        public event Action<int> OnSelected;

        public void Init(string systemName, int Id)
        {
            SystemId = Id;
            textMesh.text = systemName;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnSelected?.Invoke(SystemId);
        }
    }
}