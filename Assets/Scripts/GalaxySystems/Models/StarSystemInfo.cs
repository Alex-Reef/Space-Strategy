using UnityEngine;

namespace GalacticSystems
{
    public class StarSystemInfo
    {
        public string SystemName;
        
        private SpaceObjectInfo[] _spaceObjects;
        public SpaceObjectInfo Star => _spaceObjects[0];

        public void Init(StarInfo star, PlanetInfo[] planetInfos)
        {
            _spaceObjects = new SpaceObjectInfo[planetInfos.Length + 1];
            
            _spaceObjects[0] = star;
            for (int i = 0; i < planetInfos.Length; i++)
            {
                _spaceObjects[i + 1] = planetInfos[i];
            }
        }
    }
}