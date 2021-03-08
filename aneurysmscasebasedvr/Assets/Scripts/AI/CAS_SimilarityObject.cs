using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_SimilarityObject : MonoBehaviour
    {
        public float similarity;

        // Start is called before the first frame update
        void Start()
        {
            similarity = Random.Range(0f, 2f); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PlaceObject(Vector3 position, Color similarityColor)
        {
            Debug.Log(position); 
            transform.position = position;
            GetComponent<MeshRenderer>().material.color = similarityColor; 
        }
    }
}
