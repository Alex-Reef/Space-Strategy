using System;
using GalaxySystem.Configs;
using UnityEngine;

namespace GalaxySystem.Models
{
    [Serializable]
    public struct StarSystemData
    {
        public int systemId;
        public string systemName;
        public Vector3 position;
        public StarType type;
        public PlanetData[] spaceObjectsData;
    }
}