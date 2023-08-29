using UnityEditor;
using UnityEngine;

namespace CameraSystem
{
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : Editor
    {
        #region Property
        private SerializedProperty _zoomRangeProperty;
        private SerializedProperty _zoomSpeedProperty;
        private SerializedProperty _moveSpeed;
        private SerializedProperty _moveRadius;
        private SerializedProperty _camera;
        #endregion
         
        private bool _mainFoldout, _zoomFoldout, _moveFoldout;

        private void OnEnable()
        {
            _zoomRangeProperty = serializedObject.FindProperty("zoom");
            _zoomSpeedProperty = serializedObject.FindProperty("zoomSpeed");
            _moveSpeed = serializedObject.FindProperty("moveSpeed");
            _moveRadius = serializedObject.FindProperty("moveRadius");
            _camera = serializedObject.FindProperty("camera");
        }

        public override void OnInspectorGUI()
        {
            _mainFoldout = EditorGUILayout.Foldout(_mainFoldout, "Main Settings", true);
            if (_mainFoldout)
            {
                EditorGUILayout.PropertyField(_camera, new GUIContent("Main Camera"));
                EditorGUILayout.Space(10);
            }
            
            _zoomFoldout = EditorGUILayout.Foldout(_zoomFoldout, "Zoom Settings", true);
            if (_zoomFoldout)
            {
                EditorGUILayout.PropertyField(_zoomRangeProperty, new GUIContent("Range"));
                EditorGUILayout.PropertyField(_zoomSpeedProperty, new GUIContent("Speed"));
                EditorGUILayout.Space(10);
            }
            
            _moveFoldout = EditorGUILayout.Foldout(_moveFoldout, "Movement Settings", true);
            if (_moveFoldout)
            {
                EditorGUILayout.PropertyField(_moveSpeed, new GUIContent("Speed"));
                EditorGUILayout.PropertyField(_moveRadius, new GUIContent("Radius"));
                EditorGUILayout.Space(10);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}