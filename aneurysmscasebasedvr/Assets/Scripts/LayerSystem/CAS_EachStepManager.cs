using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

namespace CAS
{
    public class CAS_EachStepManager : MonoBehaviour
    {
        CAS_StepManager stepManager; 

        //Model Information 
        Dictionary<string, GameObject> modelsInThisStep;

        CAS_PlaceModels placeModels;

        [HideInInspector]
        public bool initial = false; 

        public int stepIndex;

        private void Awake()
        {
            stepManager = GetComponentInParent<CAS_StepManager>();
            placeModels = GetComponent<CAS_PlaceModels>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetModelsLayer(List<GameObject> models)
        {
            SetModels(models);
        }

        public void SetModelsInitial(List<GameObject> models)
        {
            SetModels(models);
            PlaceModels();
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

        public Dictionary<string, GameObject> GetModelsInThisStep()
        {
            return modelsInThisStep; 
        }

        public void PlaceModels() 
        {        
            if(!placeModels) placeModels = GetComponent<CAS_PlaceModels>();

            placeModels.numberOfModelsAtFirstCircle = 25;
            placeModels.adjustmentDistance = 0.0014f;
            placeModels.distanceBetweenSmallCircles = 0.0385f; 
            placeModels.adjustmentAngle = 0f;
            placeModels.radiusOfTheSphere = 0.45f; 
            placeModels.PickAndPlace();
        }

        public void MoveModelToLayerPosition()
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<CAS_ContolModel>().ChangeLayer();
            }
        }
    }
}
