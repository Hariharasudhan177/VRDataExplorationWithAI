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
        Dictionary<string, Vector3> originalPositionOfTheModels;

        CAS_PlaceModels placeModels;

        [HideInInspector]
        public bool initial = false; 

        int a = 0;

        private void Awake()
        {
            originalPositionOfTheModels = new Dictionary<string, Vector3>(); 
        }

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log(AdjustRadius(0.8031674f, 0.8031674f, 0.08687783f, 18));
            stepManager = GetComponentInParent<CAS_StepManager>(); 
            placeModels = GetComponent<CAS_PlaceModels>();

            if (initial)
            {
                PlaceModels(LayerChangeType.Initial);
                initial = false; 
            }
            else
            {
                PlaceModels(LayerChangeType.Forward);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetOriginalPositionOfTheModels(Dictionary<string, Vector3> positionOfTheModels)
        {
            originalPositionOfTheModels = positionOfTheModels; 
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

        public void SetModelsRefresh(List<GameObject> models)
        {
            modelsInThisStep = new Dictionary<string, GameObject>();
            foreach (GameObject model in models)
            {
                modelsInThisStep.Add(model.name, model);
                model.transform.parent = transform;
            }
            PlaceModels(LayerChangeType.Forward); 
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
                //model.transform.position = originalPositionOfTheModels[model.name];
                model.GetComponent<CAS_ContolModel>().SetStepOriginalPositionMoving(originalPositionOfTheModels[model.name], LayerChangeType.Backward); 

            }
        }

        public void PlaceModels(LayerChangeType layerChangeType) 
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
            placeModels.adjustmentDistance = radiusRequired / 200f;

            //this should increase as number of models decreases as it can spread out more 
            float distanceBetweenSmallCircles = 0.1f - ((0.1f - 0.050f) * (modelsInThisStep.Count  - stepManager.totalInitialNumberOfModels) / (0 - stepManager.totalInitialNumberOfModels));
            placeModels.distanceBetweenSmallCircles = distanceBetweenSmallCircles;

            placeModels.adjustmentAngle = 0f;

            float adjustedRadiusRequired = AdjustRadius(radiusRequired, radiusRequired, distanceBetweenSmallCircles, numberOfModelsAtFirstCircleRequired); 
            placeModels.radiusOfTheSphere = adjustedRadiusRequired;

            placeModels.PickAndPlace(layerChangeType);
        }

        public float AdjustRadius(float radiusRequired, float originalRadius, float distanceBetweenSmallCircles, float numberOfModelsAtFirstCircleRequired)
        {
            
            a++; 
            if(a > 1000)
            {
                return 0; 
            }
            float returningRadius = radiusRequired; 
            //Adjust radius if its less 
            float currentDistanceBetweenEachSmallCircle = distanceBetweenSmallCircles;
            float numberOfModelsAtEachCircle = numberOfModelsAtFirstCircleRequired;

            int index = 0;
            int circleIndex = 0;

            for (int i = 0; i < modelsInThisStep.Count; i++)
            {
                //Debug.Log(index);
                index++;
                if (index > numberOfModelsAtEachCircle)
                {
                    circleIndex++;
                    numberOfModelsAtEachCircle--; 
                    index = 0;
                    //Debug.Log(distanceBetweenSmallCircles + " " + ((circleIndex * (originalRadius / 200f)))) ;
                    currentDistanceBetweenEachSmallCircle += (distanceBetweenSmallCircles - (circleIndex * (originalRadius / 200f)));
                    //Debug.Log(currentDistanceBetweenEachSmallCircle); 
                    //Debug.Log(currentDistanceBetweenEachSmallCircle + " " + distanceBetweenSmallCircles + " " + circleIndex + " " + (originalRadius / 200f)); 
                    if (currentDistanceBetweenEachSmallCircle > radiusRequired)
                    {
                        //Debug.Log(currentDistanceBetweenEachSmallCircle + " " + radiusRequired);
                        //Debug.Log((radiusRequired + 0.05f) + "h");
                        returningRadius = AdjustRadius(radiusRequired + 0.05f, originalRadius, distanceBetweenSmallCircles, numberOfModelsAtFirstCircleRequired);
                        
                    }
                }
            }

            return returningRadius; 
        }
    }
}
