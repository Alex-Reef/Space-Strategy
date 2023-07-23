using System.Collections.Generic;
using UnityEngine;

namespace GalacticSystems
{
    [CreateAssetMenu(fileName = "Planet Configs", menuName = "Space Objects Configs/Planet Configs", order = 0)]
    public class PlanetConfigs : ScriptableObject
    {
        public PlanetConfigItem[] Configs;

        public PlanetInfo[] GetRandomPlanets(int amount)
        {
            PlanetInfo[] planetInfos = new PlanetInfo[amount];
            
            return planetInfos;
        }
    }
}