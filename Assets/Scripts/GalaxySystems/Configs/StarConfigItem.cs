using System;
using UnityEngine;

namespace GalacticSystems
{
    [Serializable]
    public class StarConfigItem : SpaceObjectConfigItem
    {
        public string StarTypeName;
        public StarType Type;
    }
}