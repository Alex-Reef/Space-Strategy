using System;
using GalaxySystem.Models;
using UnityEngine;

namespace GalaxySystem.Configs
{
    [Serializable]
    public struct PlanetConfigItem
    {
        public PlanetsType Type;
        public GameObject[] Objects;
    }
}