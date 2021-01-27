using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

namespace CAS
{
    public class CAS_ExtractMeshInformation : MonoBehaviour
    {
        private List<string[]> rowData = new List<string[]>();

        public Mesh defaultMesh; 

        // Start is called before the first frame update
        void Start()
        {
            //Save(); 
            ChangeMeshIfMoreTriangles(); 
        }

        void Save()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                Mesh mesh = meshFilter.sharedMesh;
                int numberOfVertices = mesh.vertexCount;
                int numberOfTriangles = mesh.triangles.Length;

                string[] rowDataTemp = new string[3];
                rowDataTemp[0] = meshFilter.transform.parent.name;
                rowDataTemp[1] = numberOfVertices.ToString();
                rowDataTemp[2] = numberOfTriangles.ToString();
                rowData.Add(rowDataTemp);
            }

            string[][] output = new string[rowData.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }

            int length = output.GetLength(0);
            string delimiter = ",";

            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));


            string filePath = getPath();

            Debug.Log(filePath); 
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();
        }

        private string getPath()
        {
            return Application.dataPath + "Saved_data.csv";
        }

        void ChangeMeshIfMoreTriangles()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                Mesh mesh = meshFilter.sharedMesh;
                int numberOfTriangles = mesh.triangles.Length/3;
                
                if(numberOfTriangles > 50000)
                {
                    meshFilter.sharedMesh = defaultMesh;
                    meshFilter.transform.parent.localScale = Vector3.one/10f; 
                }
            }
        }

         // Update is called once per frame
        void Update()
        {

        }
    }
}