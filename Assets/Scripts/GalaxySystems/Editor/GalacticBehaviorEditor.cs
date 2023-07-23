using UnityEditor;
using UnityEngine;

namespace GalacticSystems
{
    [CustomEditor(typeof(GalaxyBehavior))]
    public class GalacticBehaviorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GalaxyBehavior galaxyBehavior = (GalaxyBehavior)target;
            if(GUILayout.Button("Generate"))
            {
                galaxyBehavior.Generate();
            }

            if (GUILayout.Button("Clear"))
            {
                galaxyBehavior.Clear();
            }
        }
    }
}