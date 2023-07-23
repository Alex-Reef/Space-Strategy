using GalacticSystems;
using UnityEngine;

namespace Ui
{
    public class StarSystemWindow : UiWindow
    {
        private StarSystemInfo _starSystemInfo;
        
        public override void Show()
        {
            _starSystemInfo = GalaxyBehavior.Instance.SelectedSystem;
            Init();
            base.Show();
        }

        protected override void Init()
        {
            Debug.Log(_starSystemInfo.SystemName);
        }
    }
}