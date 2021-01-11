using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

namespace CAS
{
    [CustomEditor(typeof(CAS_PlaceModels))]
    public class CAS_PlaceModelsEditor : Editor
    {
        void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            //serializedObject.Update();

            DrawDefaultInspector();

            //EditorGUI.BeginChangeCheck();

            CAS_PlaceModels placeModels = (CAS_PlaceModels)target;

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

            if (GUILayout.Button("Update"))
            {
                placeModels.PickAndPlace(LayerChangeType.Forward);
            }

            if (GUILayout.Button("TempUpdate"))
            {
                placeModels.TempUpdate();
            }

        }
    }
}

