using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor; 

namespace CAS
{
    public class CAS_MeshForEnvironementalSwitch : MonoBehaviour
    {
        public int dataAvailable = -1;
        public void AddMeshesForEnvironmentalSwitch()
        {
            string assetsFolderPath = "Assets/AneurysmData/"; 

            foreach (Transform child in transform)
            {
                CAS_ModelIdentifier modelMeshToSwitch = child.GetComponent<CAS_ModelIdentifier>(); 

                string fresnelMeshPath = assetsFolderPath + "ModelObjSimplifiedForFresnelShader/" + child.gameObject.name + ".obj";
                modelMeshToSwitch.fresnelMesh = (Mesh)AssetDatabase.LoadAssetAtPath(fresnelMeshPath, typeof(Mesh));

                string normalMeshPath = assetsFolderPath + "ModelObjSimplifiedForNormalShader/" + child.gameObject.name + ".obj";
                modelMeshToSwitch.normalMesh = (Mesh)AssetDatabase.LoadAssetAtPath(normalMeshPath, typeof(Mesh));
            }
        }
    }

}