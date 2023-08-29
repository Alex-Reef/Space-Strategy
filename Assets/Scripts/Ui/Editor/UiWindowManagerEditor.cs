using System;
using UnityEditor;
using UnityEngine;

namespace Ui
{
    [CustomEditor(typeof(UiWindowManager))]
    public class UiWindowManagerEditor : Editor
    {
        private UiWindowManager _uiWindowManager;
        private bool _showActiveWindows;

        private void OnEnable()
        {
            _uiWindowManager = (UiWindowManager)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                GUILayout.Label("Testing");
                _showActiveWindows = EditorGUILayout.Toggle("Show only active windows", _showActiveWindows);
                var windows = _uiWindowManager.GetAllWindow(_showActiveWindows);
                foreach (var window in windows)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(Enum.GetName(typeof(UiWindowType), window));
                    var active = _uiWindowManager.IsActive(window);
                    if (GUILayout.Button(active ? "Hide" : "Show", new GUIStyle(){ fixedWidth = 120}))
                    {
                        if (active)
                            _uiWindowManager.HideWindow(window);
                        else
                            _uiWindowManager.ShowWindow(window);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }
    }
}