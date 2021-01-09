using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_EachStepManager : MonoBehaviour
    {
        CAS_StepManager stepManager; 

        //Model Information 
        Dictionary<string, GameObject> modelsInThisStep;

        CAS_PlaceModels placeModels; 

        // Start is called before the first frame update
        void Start()
        {
            stepManager = GetComponentInParent<CAS_StepManager>(); 
            placeModels = GetComponent<CAS_PlaceModels>();
            PlaceModels();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Dictionary<string, GameObject> GetModelsInThisStep()
        {
            return modelsInThisStep; 
        }

        public void SetModels(List<GameObject> models)
        {
            modelsInThisStep = new Dictionary<string, GameObject>();
            foreach (GameObject model in models)
            {
                modelsInThisStep.Add(model.name, model);
                model.transform.parent = transform;
            }
        }

        public void RemoveFilteredModels(List<GameObject> models)
        {
            foreach (GameObject model in models)
            {
                modelsInThisStep.Remove(model.name); 
            }
        }

        public void AddBackFilteredModels(List<GameObject> models)
        {
            foreach (GameObject model in models)
            {
                modelsInThisStep.Add(model.name, model);
                model.transform.parent = transform;
            }
        }

        public void PlaceModels() 
        {
            //Solving quadratic equation of the sum of natural number formula (only positive value) using the algebric formula 
            float numberOfModelsAtFirstCircleRequired = Mathf.Ceil((Mathf.Sqrt(1f + (4f * (modelsInThisStep.Count * 2f))) - 1f) / 2f);
            placeModels.numberOfModelsAtFirstCircle = numberOfModelsAtFirstCircleRequired;

            //This is an issue resolve later 
            float minimumRadius = 0.25f;

            //Adjusting the radius based on the number of models selected. This can be changed on the formula too rather than minimum radius 
            /*float modelSelectedPercentage = ((float)((float)stepManager.totalInitialNumberOfModels - (float)modelsInThisStep.Count) / stepManager.totalInitialNumberOfModels); 

            if (modelSelectedPercentage > 0.95)
            {
                minimumRadius = 0.5f; 
            }else if (modelSelectedPercentage > 0.90)
            {
                minimumRadius = 0.6f;
            }else if (modelSelectedPercentage > 0.85)
            {
                minimumRadius = 0.75f;
            }else if (modelSelectedPercentage > 0.80)
            {
                minimumRadius = 0.8f;
            }else if (modelSelectedPercentage > 0.75)
            {
                minimumRadius = 0.85f;
            }*/

            float radiusRequired = 1f - ((1f - minimumRadius) * (stepManager.totalInitialNumberOfModels - modelsInThisStep.Count) / (stepManager.totalInitialNumberOfModels - 0));
            placeModels.radiusOfTheSphere = radiusRequired;

            //this should increase as number of models decreases as it can spread out more 
            float distanceBetweenSmallCircles = 0.1f - ((0.1f - 0.075f) * (modelsInThisStep.Count  - stepManager.totalInitialNumberOfModels) / (0 - stepManager.totalInitialNumberOfModels));
            placeModels.distanceBetweenSmallCircles = distanceBetweenSmallCircles;

            placeModels.adjustmentDistance = radiusRequired / 200f;

            placeModels.adjustmentAngle = 0f; 

            placeModels.PickAndPlace();
        }
    }
}
