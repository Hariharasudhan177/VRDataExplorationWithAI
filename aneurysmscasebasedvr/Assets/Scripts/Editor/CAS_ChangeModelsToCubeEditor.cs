using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CAS
{
    [CustomEditor(typeof(CAS_ChangeModelsToCube))]
    public class CAS_ChangeModelsToCubeEditor : Editor
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnInspectorGUI()
        {
            //serializedObject.Update();

            DrawDefaultInspector();

            //EditorGUI.BeginChangeCheck();

            CAS_ChangeModelsToCube changeModelsToCube = (CAS_ChangeModelsToCube)target;

            //Later if validation needed while entering values 
            /*SerializedProperty m_radiusOftheSphere = serializedObject.FindProperty("radiusOfTheSphere");
            SerializedProperty m_numberOfModelsAtFirstCircle = serializedObject.FindProperty("numberOfModelsAtFirstCircle");
            SerializedProperty m_distanceBetweenSmallCircles = serializedObject.FindProperty("distanceBetweenSmallCircles");

            EditorGUILayout.PropertyField(m_radiusOftheSphere);
            EditorGUILayout.PropertyField(m_numberOfModelsAtFirstCircle);
            EditorGUILayout.PropertyField(m_distanceBetweenSmallCircles);

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                //placeModels.ValidateValues(); 
            }*/

            if (GUILayout.Button("CreateCubeModels"))
            {
                changeModelsToCube.CreateCubeModels();

            }

        }
    }
}
