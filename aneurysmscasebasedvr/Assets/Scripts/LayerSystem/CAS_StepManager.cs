using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_StepManager : MonoBehaviour
    {
        public GameObject manager; 

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

        //Red, Green, Blue, Orange, Yellow, Teal, Pink, Lavender, Apricot, Brown, Maroon, Olive, Beige, Cyan, Mint, Purple, Lime, 
        //DarkOliveGreen, Palevioletred, Goldenrod, Darkslategray
        Color[] colorsForGroupBy = new Color[] { new Color(0.901f, 0.098f, 0.294f, 0f), new Color(0.235f, 0.705f, 0.294f, 0f), 
            new Color(0.262f, 0.388f, 0.847f, 0f), new Color(0.960f, 0.509f, 0.192f, 0f), new Color(1f, 0.882f, 0.098f, 0f), 
            new Color(0.274f, 0.6f, 0.564f, 0f), new Color(0.862f, 0.745f, 1f, 0f), new Color(0.980f, 0.745f, 0.831f, 0f), 
            new Color(1f, 0.847f, 0.694f, 0f), new Color(0.603f, 0.388f, 0.141f, 0f), new Color(0.603f, 0.388f, 0.141f, 0f), 
            new Color(0.501f, 0f, 0f, 0f), new Color(0.501f, 0.501f, 0f, 0f), new Color(1f, 0.980f, 0.784f, 0f), 
            new Color(0.258f, 0.831f, 0.956f, 0f), new Color(0.666f, 1f, 0.764f, 0f), new Color(0.568f, 0.117f, 0.705f, 0f),
            new Color(0.749f, 0.937f, 0.270f, 0f), new Color(0.333f, 0.419f, 0.184f, 0f), new Color(0.858f, 0.439f, 0.576f, 0f),
            new Color(0.854f, 0.647f, 0.125f, 0f), new Color(0.184f, 0.309f, 0.309f, 0f)};

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
                        if(index >= colorsForGroupBy.Length)
                        {
                            index = 0; 
                        }
                        allModelsInformation[value].GetComponentInChildren<CAS_ContolModel>().SetGroupByColour(colorsForGroupBy[index]);                
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
