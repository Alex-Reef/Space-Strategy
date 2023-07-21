using UnityEngine;
using UnityEngine.EventSystems;

namespace GalacticSystems
{
    public class StarSystemBehavior : MonoBehaviour, IPointerClickHandler
    {
        private SolarSystemInfo _solarSystemInfo;

        public void Init(SolarSystemInfo info)
        {
            _solarSystemInfo = info;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Select");
        }
    }
}