using UnityEngine;

namespace GalacticSystems
{
    [CreateAssetMenu(fileName = "Star Configs", menuName = "Space Objects Configs/Star Configs", order = 0)]
    public class StarConfigs : ScriptableObject
    {
        public StarConfigItem[] Configs;
    }
}