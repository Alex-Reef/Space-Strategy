using SaveSystem;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManagerBehavior : MonoBehaviour
    {
        public void Load()
        {
            SaveManager.Load(OnLoaded);
        }

        private void OnLoaded()
        {
            Debug.Log($"Save Manager: Loaded {SaveManager.Saves.Count} saves");
        }
    }
}