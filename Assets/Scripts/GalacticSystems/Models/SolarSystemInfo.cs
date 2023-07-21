using UnityEngine;

namespace GalacticSystems
{
    public class SolarSystemInfo
    {
        private SpaceObjectInfo[] _spaceObjects;
        public SpaceObjectInfo Star => _spaceObjects[0];

        public SolarSystemInfo()
        {
            int objectsAmount = Random.Range(2, 7);
            _spaceObjects = new SpaceObjectInfo[objectsAmount];

            _spaceObjects[0] = new StarInfo();
            for (int i = 1; i < objectsAmount; i++)
            {
                _spaceObjects[i] = new PlanetInfo();
            }
        }
    }
}