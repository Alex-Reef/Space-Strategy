using System;
using Random = UnityEngine.Random;

namespace GalacticSystems
{
    public class StarInfo : SpaceObjectInfo
    {
        public StarType Type;

        public StarInfo()
        {
            Array values = Enum.GetValues(typeof(StarType));
            Type = (StarType)values.GetValue(Random.Range(0, values.Length));
        }
    }
}