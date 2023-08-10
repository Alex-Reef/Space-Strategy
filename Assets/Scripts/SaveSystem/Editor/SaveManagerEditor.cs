using System;
using GalaxySystem.Behavior;
using UnityEditor;
using UnityEngine;

namespace SaveSystem
{
    [CustomEditor(typeof(SaveManagerBehavior))]
    public class SaveManagerEditor : Editor
    {
        private bool[] _foldouts;
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Load All"))
            { 
                SaveManager.Load(() =>
                {
                    Debug.Log($"Loaded: {SaveManager.Saves.Count}");
                });
            }

            if (GUILayout.Button("Save"))
            {
                SaveData saveData = new SaveData()
                {
                    dateTime = DateTime.UtcNow.ToString("dd-mm-yy-hh-mm-ss"),
                    starSystems = GalaxyBehavior.Instance.StarSystemsData
                };
                
                SaveManager.Save(saveData, (savePath) =>
                {
                    Debug.Log("Saved at: " + savePath);
                });
            }

            if (SaveManager.Saves != null && SaveManager.Saves.Count > 0)
            {
                int count = SaveManager.Saves.Count;
                _foldouts = new bool[count];
                for (int i = 0; i < count; i++)
                {
                    _foldouts[i] = true;
                    var save = SaveManager.Saves[i];
                    _foldouts[i] = EditorGUILayout.Foldout(_foldouts[i], $"Save #{i+1}");
                    if (_foldouts[i])
                    {
                        GUILayout.Label($"DateTime: {save.dateTime}");
                        GUILayout.Label($"Star Systems: {save.starSystems.Length}");
                        if (GUILayout.Button("Load"))
                        {
                            GalaxyBehavior.Instance.LoadSystemsData(save.starSystems);
                            GalaxyBehavior.Instance.SpawnSystems();
                        }
                    }
                }
            }
            else
            {
                GUILayout.Label("Saves not loaded");
            }
            EditorGUILayout.EndVertical();
        }
    }
}