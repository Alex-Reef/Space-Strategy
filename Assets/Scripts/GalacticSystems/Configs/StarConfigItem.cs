using System;
using UnityEngine;

namespace GalacticSystems
{
    [Serializable]
    public class StarConfigItem
    {
        public NumberRange TemperatureRange;
        public StarSystemBehavior[] PrefabVariables;
        public StarType Type;
    }
}