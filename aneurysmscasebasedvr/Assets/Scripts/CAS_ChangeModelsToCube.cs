using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_ChangeModelsToCube : MonoBehaviour
    {
        public GameObject prefab; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateCubeModels()
        {
            GameObject cubeModels = new GameObject("CubeModels");


            foreach (Transform child in transform)
            {
                GameObject cubeModel = Instantiate(prefab);
                cubeModel.transform.parent = cubeModels.transform;
                cubeModel.name = child.name;
            }
        }
    }
}
