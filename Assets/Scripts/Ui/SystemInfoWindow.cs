using GalaxySystem.Behavior;
using GalaxySystem.Models;
using UnityEngine;
using Zenject;

namespace Ui
{
    public class SystemInfoWindow : UiWindow
    {
        [Inject] private GalaxyBehavior _galaxyBehavior;
        
        private StarSystemData _data;

        private void Awake()
        {
            base.Awake();
            
            OnHide += () => _galaxyBehavior.ClearSelected();
            
            _galaxyBehavior.OnStarSelected += (id) =>
            {
                if (id != -1)
                {
                    Init(_galaxyBehavior.StarSystemsData[id]);
                    Show();
                }
            };
        }
        
        private void Init(StarSystemData data)
        {
            _data = data;
        }

        public override void Show()
        {
            Debug.Log(_data.systemName);
            base.Show();
        }
    }
}