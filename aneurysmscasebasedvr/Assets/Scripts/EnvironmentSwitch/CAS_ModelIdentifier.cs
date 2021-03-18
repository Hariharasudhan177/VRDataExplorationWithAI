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
            meshFilter.mesh = normalMesh; 
        }

        public void SetToSciFi()
        {
            meshFilter.mesh = fresnelMesh;
        }
    }
}

