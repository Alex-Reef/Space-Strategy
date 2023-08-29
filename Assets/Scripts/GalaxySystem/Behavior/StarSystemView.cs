using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GalaxySystem.Behavior
{
    [DisallowMultipleComponent]
    public class StarSystemView : MonoBehaviour
    {
        [SerializeField] private TextMesh textMesh;

        public void Init(string systemName)
        {
            textMesh.text = systemName;
        }
    }
}