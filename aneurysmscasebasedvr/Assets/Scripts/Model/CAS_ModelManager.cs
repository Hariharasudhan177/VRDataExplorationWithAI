using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_ModelManager : MonoBehaviour
    {
        /*//Model Information 
        public Dictionary<string, GameObject> allModelsInformation;

        //Default Material 
        public Material defaultMaterial; 
        
        public Dictionary<string, GameObject> filteredModelsListInFirstLayer;
        public Dictionary<string, GameObject> filteredModelsListInFirstLayerPrevious;

        // Start is called before the first frame update
        void Start()
        {
            allModelsInformation = new Dictionary<string, GameObject>();

            filteredModelsListInFirstLayer = new Dictionary<string, GameObject>();
            filteredModelsListInFirstLayerPrevious = new Dictionary<string, GameObject>();

            //Initialise prepare models 
            //PrepareModels(); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FilterFirstLayer(List<string> filteredPatientIds)
        {

            filteredModelsListInFirstLayerPrevious = filteredModelsListInFirstLayer;
            filteredModelsListInFirstLayer = new Dictionary<string, GameObject>();

            if (filteredPatientIds == null)
            {
                //clear filter at least move object back 
                SendModelsToOriginalLayer();
                return; 
            }

            foreach (string patientId in filteredPatientIds)
            {
                //Debug.Log(patientId);

                //Check if already present 
                GameObject filteredGameObject = GameObject.Find(patientId);

                if(filteredGameObject != null)
                {
                    filteredModelsListInFirstLayer.Add(patientId, filteredGameObject);
                    BringFilteredModelToFirstLayer(filteredGameObject);
                }
            }

            //remaining in the filteredModelsListInFirstLayerPrevious
            SendModelsToOriginalLayer(); 

        }

        void BringFilteredModelToFirstLayer(GameObject filteredObject)
        {
            filteredObject.GetComponent<CAS_ContolModel>().MoveModelToFirstLayer();
        }

        void SendModelsToOriginalLayer()
        {
            foreach(KeyValuePair<string, GameObject> unfilteredModel in filteredModelsListInFirstLayerPrevious)
            {
                if (!filteredModelsListInFirstLayer.ContainsKey(unfilteredModel.Key))
                {
                    unfilteredModel.Value.transform.GetComponent<CAS_ContolModel>().MoveModelToOriginalLayer(); 
                }
            }
        }*/

    }
}

