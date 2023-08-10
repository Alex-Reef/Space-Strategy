using UnityEngine;

namespace GalaxySystem.Configs
{
    [CreateAssetMenu(fileName = "Planet Config", menuName = "Galaxy Configs/Planet Config", order = 0)]
    public class PlanetConfig : ScriptableObject
    {
        public PlanetConfigItem[] Items;
    }
}