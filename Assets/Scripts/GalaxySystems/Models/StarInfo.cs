using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GalacticSystems
{
    public class StarInfo : SpaceObjectInfo
    {
        public StarType Type { get; private set; }

        public StarInfo()
        {
            Array values = Enum.GetValues(typeof(StarType));
            Type = (StarType)values.GetValue(Random.Range(0, values.Length));
        }
        
        public void Init(StarConfigItem configItem)
        {
            Temperature = Random.Range(configItem.TemperatureRange.Min, configItem.TemperatureRange.Max);
            Object = configItem.Prefab;
        }
    }
}