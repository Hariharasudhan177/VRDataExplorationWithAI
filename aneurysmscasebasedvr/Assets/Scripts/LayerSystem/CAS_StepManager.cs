using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_StepManager : MonoBehaviour
    {
        public CAS_Manager manager; 

        //Super parent of the models 
        GameObject modelsParent;

        //Model Information 
        public Dictionary<string, GameObject> allModelsInformation;

        public Material defaultMaterial; 

        [HideInInspector]
        public List<CAS_EachStepManager> stepParents;

        [HideInInspector]
        public int totalInitialNumberOfModels;

        Vector3 originalScale;

        private void Awake()
        {
            allModelsInformation = new Dictionary<string, GameObject>();
            stepParents = new List<CAS_EachStepManager>();

            modelsParent = transform.gameObject;
            originalScale = transform.localScale; 

            InitialseStepParents();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //Initialisation 
        public void InitialseStepParents()
        {
            List<GameObject> models = GetInitialModels();
            totalInitialNumberOfModels = models.Count;

            CAS_EachStepManager stepParent = CreateNewStep();
            stepParent.SetModelsInitial(models);
            stepParents.Add(stepParent);
        }

        //Further steps when added 
        public void SetFilteredModelsToEditLayers(List<List<string>> modelsId)
        {
            int index = 0;

            //Original Layer 
            stepParents[index].SetModelsLayer(GetGameObjectsAll());

            foreach (List<string> modelsIdForThisStep in modelsId)
            {
                index++;
                List<GameObject> models = GetGameObjectsFromModelId(modelsIdForThisStep);
                if (index >= stepParents.Count)
                {
                    CAS_EachStepManager eachStepManager = CreateNewStep();
                    stepParents.Add(eachStepManager);
                    eachStepManager.SetModelsLayer(models);
                }
                else
                {
                    stepParents[index].SetModelsLayer(models);
                }
            }

            //Remove unnecessary steps 
            if (stepParents.Count - index > 1)
            {
                int stepsToDelete = stepParents.Count - index - 1;

                int lastStep = stepParents.Count - 1;

                for (int i = lastStep; i > lastStep - stepsToDelete; i--)
                {
                    DeleteStep(i);
                }
            }

            MoveModelsToRespectiveLayers();
        }

        CAS_EachStepManager CreateNewStep()
        {
            GameObject stepParent = new GameObject();
            stepParent.transform.parent = modelsParent.transform;

            stepParent.AddComponent<CAS_EachStepManager>();
            stepParent.AddComponent<CAS_PlaceModels>();
            return stepParent.GetComponent<CAS_EachStepManager>();
        }

        void DeleteStep(int index)
        {
            CAS_EachStepManager eachStepManager = stepParents[index];
            stepParents.RemoveAt(index);
            Destroy(eachStepManager.gameObject);
        }

        public List<GameObject> GetInitialModels()
        {
            List<GameObject> models = new List<GameObject>();
            foreach (Transform child in transform)
            {
                models.Add(child.gameObject);
            }
            return models;
        }

        List<GameObject> GetGameObjectsFromModelId(List<string> modelsId)
        {
            List<GameObject> models = new List<GameObject>();

            foreach (string id in modelsId)
            {
                models.Add(allModelsInformation[id]);
            }

            return models; 
        }

        List<GameObject> GetGameObjectsAll()
        {
            List<GameObject> models = new List<GameObject>();

            foreach (string id in allModelsInformation.Keys)
            {
                models.Add(allModelsInformation[id]);
            }

            return models;
        }

        //This function will be called once the objects are set to all the models 
        void MoveModelsToRespectiveLayers()
        {
            int index = 0; 
            //Reverse order as the finally layer would contain the models with all filters applied 
            for(int i = stepParents.Count-1; i >= 0; i--)
            {
                CAS_EachStepManager eachStepManager = stepParents[i];
                eachStepManager.stepIndex = index;
                eachStepManager.MoveModelToLayerPosition();
                index++; 
            }
        }

        public void SetGroupByModelsToEditLayers(Dictionary<string, List<string>> filteredPatientIdsGroupBy)
        {
            int index = 0;

            foreach (string key in filteredPatientIdsGroupBy.Keys)
            {
                foreach (string value in filteredPatientIdsGroupBy[key])
                {
                    if (allModelsInformation.ContainsKey(value))
                    {
                        if(index >= manager.dataManager.colorsForGrouping.Length)
                        {
                            index = 0; 
                        }
                        allModelsInformation[value].GetComponentInChildren<CAS_ContolModel>().SetGroupByColour(manager.dataManager.colorsForGrouping[index]);                
                    }
                }
                index++;
            }
        }

        public void RemoveGroupByModelsToEditLayers()
        {
            foreach (string value in allModelsInformation.Keys)
            {
                allModelsInformation[value].GetComponentInChildren<CAS_ContolModel>().UnSetGroupByColour();
            }           
        }

        public void SetTrueScale()
        {
            transform.localScale = Vector3.one;
        }

        public void SetOriginalScale()
        {
            transform.localScale = originalScale;
        }
    }
}
