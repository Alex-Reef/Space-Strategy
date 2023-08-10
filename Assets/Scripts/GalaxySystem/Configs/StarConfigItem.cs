using System;
using GalaxySystem.Models;
using UnityEngine;

namespace GalaxySystem.Configs
{
    [Serializable]
    public struct StarConfigItem
    {
        public StarType Type;
        public GameObject Object;
        public NumbersRange planetsCount;
    }
}