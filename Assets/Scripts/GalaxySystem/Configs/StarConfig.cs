using UnityEngine;

namespace GalaxySystem.Configs
{
    [CreateAssetMenu(fileName = "Star Config", menuName = "Galaxy Configs/Star Config", order = 0)]
    public class StarConfig : ScriptableObject
    {
        public StarConfigItem[] Items;
    }
}