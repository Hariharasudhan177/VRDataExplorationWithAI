using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_ModelIdentifier : MonoBehaviour
    {
        public Mesh normalMesh;
        public Mesh fresnelMesh;

        public MeshFilter meshFilter;

        public int dataAvailable = -1; 

        private void Start()
        {
            meshFilter = GetComponentInChildren<MeshFilter>();     
        }

        public void SetToNormal()
        {
            if (meshFilter == null) meshFilter = GetComponentInChildren<MeshFilter>();
            meshFilter.mesh = normalMesh; 
        }

        public void SetToSciFi()
        {
            if(meshFilter == null) meshFilter = GetComponentInChildren<MeshFilter>();
            meshFilter.mesh = fresnelMesh;
        }
    }
}

