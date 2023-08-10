using System;
using GalaxySystem.Models;

namespace SaveSystem
{
    [Serializable]
    public struct SaveData
    {
        public string dateTime;
        public StarSystemData[] starSystems;
    }
}