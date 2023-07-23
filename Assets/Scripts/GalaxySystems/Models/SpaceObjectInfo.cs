using UnityEngine;

namespace GalacticSystems
{
    public abstract class SpaceObjectInfo
    {
        public int Temperature { get; protected set; }
        public GameObject Object { get; protected set; }
    }
}