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
            if (Input.GetKeyDown(KeyCode.A))
            {
                List<string> filteredModels = new List<string>();
                System.Random random = new System.Random();

                List<string> keyList = new List<string>(allModelsInformation.Keys);
                UnityEngine.Debug.Log(allModelsInformation.Count);

                for (int i = 0; i < 50; i++)
                {
                    
                    int randomNumber = random.Next(0, keyList.Count - 1);
                    filteredModels.Add(keyList[randomNumber]);
                    keyList.Remove(keyList[randomNumber]); 
                }

                
                IncreaseSteps(filteredModels); 
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                DecreaseSteps();
            }
        }

        public void IncreaseSteps(List<string> modelsId)
        {
            List<GameObject> models = new List<GameObject>(); 
            foreach(string id in modelsId)
            {
                models.Add(allModelsInformation[id]); 
            }

            GameObject stepParent = new GameObject(); 
            stepParent.transform.parent = modelsParent.transform;

            stepParent.AddComponent<CAS_EachStepManager>();
            stepParent.AddComponent<CAS_PlaceModels>();
            CAS_EachStepManager stepModelManager = stepParent.GetComponent<CAS_EachStepManager>();
            stepModelManager.SetModels(models);

            CAS_EachStepManager lastStepParent = stepParents[stepParents.Count - 1];
            lastStepParent.RemoveFilteredModels(models); 

            stepParents.Add(stepModelManager);
            stepParent.name = "Step" + (stepParents.Count-1);
        }

        public void RefreshStep(List<string> modelsId, int stepNumber)
        {
            //First add the models back, refresh the old step and then communicate with the new step 
            CAS_EachStepManager stepParentManager = stepParents[stepNumber];    
            CAS_EachStepManager lastStepParentManager = stepParents[stepNumber - 1];

            List<GameObject> modelsToAddBack = new List<GameObject>();
            foreach (string key in stepParentManager.GetModelsInThisStep().Keys)
            {
                modelsToAddBack.Add(stepParentManager.GetModelsInThisStep()[key]);
            }
            lastStepParentManager.AddBackFilteredModels(modelsToAddBack);

            List<GameObject> models = new List<GameObject>();
            foreach (string id in modelsId)
            {
                Debug.Log(id);
                models.Add(allModelsInformation[id]);
            }

            stepParentManager.SetModelsRefresh(models);
            lastStepParentManager.RemoveFilteredModels(models);
        }

        public void DecreaseSteps()
        {
            CAS_EachStepManager lastStepParent = stepParents[stepParents.Count - 1];

            List<GameObject> models = new List<GameObject>(); 
            foreach(Transform child in lastStepParent.transform)
            {
                models.Add(child.gameObject); 
            }

            CAS_EachStepManager previousStepParent = stepParents[stepParents.Count - 2];
            previousStepParent.AddBackFilteredModels(models); 

            stepParents.Remove(lastStepParent);
            Destroy(lastStepParent.gameObject);
        }

        public void InitialseStepParents()
        {
            GameObject firstParent = new GameObject();
            firstParent.name = "Step" + (stepParents.Count);
            firstParent.AddComponent<CAS_EachStepManager>();
            firstParent.AddComponent<CAS_PlaceModels>();
            firstParent.transform.parent = transform; 
            CAS_EachStepManager stepParent = firstParent.GetComponent<CAS_EachStepManager>();
            stepParents.Add(stepParent.GetComponent<CAS_EachStepManager>());

            InitialiseModels(); 
        }

        public void InitialiseModels()
        {
            List<GameObject> models = new List<GameObject>();
            foreach (Transform child in transform)
            {
                models.Add(child.gameObject);
            }

            totalInitialNumberOfModels = models.Count;
            stepParents[0].SetModels(models); 
            stepParents[0].initial = true; 
        }

        public void SetTrueScale()
        {
            transform.localScale = Vector3.one; 
        }

        public void SetOriginalScale()
        {
            transform.localScale = originalScale; 
        }

        public void GroupByStep(Dictionary<string, List<string>> filteredPatientIdsGroupBy, int stepNumber)
        {
            CAS_EachStepManager stepParentManager = stepParents[stepNumber];
            stepParentManager.GroupModels(filteredPatientIdsGroupBy); 
        }
    }
}
