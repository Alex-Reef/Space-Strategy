using System;
using UnityEngine;

namespace GalacticSystems
{
    [Serializable]
    public abstract class SpaceObjectConfigItem
    {
        public NumberRange TemperatureRange;
        public GameObject Prefab;
        public NumberRange PlanetAmounts;
    }
}