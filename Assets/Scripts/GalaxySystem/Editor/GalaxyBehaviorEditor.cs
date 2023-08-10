using GalaxySystem.Behavior;
using UnityEditor;
using UnityEngine;

namespace GalaxySystem
{
    [CustomEditor(typeof(GalaxyBehavior))]
    public class GalaxyBehaviorEditor : Editor
    {
        private GalaxyBehavior _behavior;
        private Transform _transform;

        #region Property
        private SerializedProperty _galaxyRadiusProp;
        private SerializedProperty _systemsCountProp;
        private SerializedProperty _distanceBetweenStarsProp;
        private SerializedProperty _yOffsetProp;
        private SerializedProperty _starSystemPrefabProp;
        private SerializedProperty _starsConfigProp;
        private SerializedProperty _planetsConfigProp;
        #endregion

        #region Editor
        private bool _mainFoldout = true;
        private bool _spawnFoldout = true;
        private bool _configsFoldout = true;
        private bool _debugFoldout = true;
        private GUIStyleState _whiteText;
        private Color _drawColor = Color.red;
        private int _lineWidth = 1;
        private int _lineCount = 36;
        #endregion

        private void OnEnable()
        {
            _behavior = (GalaxyBehavior)target;
            _lineWidth = EditorPrefs.GetInt("lineWidth");
            _lineCount = EditorPrefs.GetInt("lineCount");
            
            _whiteText = new GUIStyleState() {
                textColor = Color.white
            };
            _galaxyRadiusProp = serializedObject.FindProperty("galaxyRadius");
            _systemsCountProp = serializedObject.FindProperty("starSystemsCount");
            _distanceBetweenStarsProp = serializedObject.FindProperty("distanceBetweenStars");
            _yOffsetProp = serializedObject.FindProperty("yPosOffset");
            _starSystemPrefabProp = serializedObject.FindProperty("starSystemPrefab");
            _starsConfigProp = serializedObject.FindProperty("starConfig");
            _planetsConfigProp = serializedObject.FindProperty("planetConfig");
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Galaxy Generator", new GUIStyle() {
                fontStyle = FontStyle.Bold, 
                fontSize = 18, 
                normal = _whiteText
            });
            GUILayout.Label("Created by Reef Interactive", new GUIStyle() {
                fontSize = 9,
                normal = _whiteText
            });
            EditorGUILayout.Space(15);
            
            _mainFoldout = EditorGUILayout.Foldout(_mainFoldout, "Main Settings", true);
            if (_mainFoldout)
            {
                EditorGUILayout.PropertyField(_galaxyRadiusProp, new GUIContent("Galaxy Radius"));
                EditorGUILayout.PropertyField(_systemsCountProp, new GUIContent("Number of Star Systems"));
                EditorGUILayout.Space(10);
            }
            
            _spawnFoldout = EditorGUILayout.Foldout(_spawnFoldout, "Spawn Settings", true);
            if (_spawnFoldout)
            {
                EditorGUILayout.PropertyField(_distanceBetweenStarsProp, new GUIContent("Distance Between Stars"));
                EditorGUILayout.PropertyField(_yOffsetProp);
                EditorGUILayout.PropertyField(_starSystemPrefabProp);
                EditorGUILayout.Space(10);
            }

            _configsFoldout = EditorGUILayout.Foldout(_configsFoldout, "Configs Settings", true);
            if (_configsFoldout)
            {
                EditorGUILayout.PropertyField(_starsConfigProp, new GUIContent("Stars Config"));
                EditorGUILayout.PropertyField(_planetsConfigProp, new GUIContent("Planets Config"));
                EditorGUILayout.Space(10);
            }
            
            _debugFoldout = EditorGUILayout.Foldout(_debugFoldout, "Debug Settings", true);
            if (_debugFoldout)
            {
                _drawColor = EditorGUILayout.ColorField("Render Color", _drawColor);
                _lineWidth = EditorGUILayout.IntSlider("Line Width", _lineWidth, 1, 10);
                _lineCount = EditorGUILayout.IntSlider("Number of lines", _lineCount, 1, 360);
                
                EditorPrefs.SetInt("lineWidth", _lineWidth);
                EditorPrefs.SetInt("lineCount", _lineCount);
                
                EditorGUILayout.Space(10);
            }
            
            GUILayout.Label("<color=white>Test</color> <color=red>(only in play mode)</color>", new GUIStyle(){richText = true});
            if (!_behavior.Locked)
            {
                if (GUILayout.Button("Generate"))
                {
                    if (Application.isPlaying)
                        _behavior.GenerateGalaxy();
                }

                if (GUILayout.Button("Clear"))
                {
                    if (Application.isPlaying)
                        _behavior.Clear();
                }
            }
            else
            {
                GUILayout.Label("Operation in progress, please wait...");
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            _transform = ((GalaxyBehavior)target).transform;
            DrawHandles();
        }

        private void DrawHandles()
        {
            Handles.color = _drawColor;
            Handles.DrawWireDisc(_transform.position + Vector3.up * _yOffsetProp.intValue, Vector3.up, _galaxyRadiusProp.intValue, _lineWidth);
            Handles.DrawWireDisc(_transform.position + Vector3.down * _yOffsetProp.intValue, Vector3.up, _galaxyRadiusProp.intValue, _lineWidth);

            float offset = 360f / _lineCount;
            for (int i = 0; i < _lineCount; i++)
            {
                float angle = i * offset * Mathf.Deg2Rad;
                float x = Mathf.Sin(angle) * _galaxyRadiusProp.intValue;
                float y = Mathf.Cos(angle) * _galaxyRadiusProp.intValue;

                Vector3 startPoint = new Vector3(x,  _yOffsetProp.intValue, y) + _transform.position;
                Vector3 endPoint = new Vector3(x, -_yOffsetProp.intValue, y) + _transform.position;

                Handles.DrawLine(startPoint, endPoint);
            }
        }
    }
}