using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

namespace CAS
{
    public class CAS_StepManager : MonoBehaviour
    {
        public CAS_Manager manager; 

        //Super parent of the models 
        GameObject modelsParent;

        //Model Information 
        [HideInInspector]
        public Dictionary<string, GameObject> allModelsInformationByRecordName;
        [HideInInspector]
        public Dictionary<string, GameObject> allModelsInformationByGameObjectName;
        [HideInInspector]
        public Dictionary<string, List<string>> allModelsInformationGameobjectRecordName;

        public Material defaultMaterial;
        public Material boundingBoxLineMaterial;

        [HideInInspector]
        public List<CAS_EachStepManager> stepParents;

        [HideInInspector]
        public int totalInitialNumberOfModels;

        Vector3 originalScale;

        string gameObjectForWhichDataIsDisplayed = "";

        public Mesh defaultMesh;

        [Range(100, 800000)]
        public int thresholdTriangleCountForCubeConversion = 2000;

        public float limitSize = 0.75f;

        public bool controlledPull = false;
        public bool pushObjects = false;

        public CAS_GrabInteractable[] grabInteractables;

        private void Start()
        {
            grabInteractables = GetComponentsInChildren<CAS_GrabInteractable>(); 
        }

        public void InitializeAfterDataRead()
        {
            allModelsInformationByRecordName = new Dictionary<string, GameObject>();
            allModelsInformationByGameObjectName = new Dictionary<string, GameObject>();
            allModelsInformationGameobjectRecordName = new Dictionary<string, List<string>>();

            stepParents = new List<CAS_EachStepManager>();

            modelsParent = transform.gameObject;
            originalScale = transform.localScale;

            ChangeMeshIfMoreTriangles();

            InitialseStepParents();
        }

        void ChangeMeshIfMoreTriangles()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                Mesh mesh = meshFilter.sharedMesh;
                int numberOfTriangles = mesh.triangles.Length / 3;

                if (numberOfTriangles > thresholdTriangleCountForCubeConversion)
                {
                    meshFilter.sharedMesh = defaultMesh;
                    meshFilter.transform.localScale = Vector3.one / 50f;
                }
            }
        }

        //Initialisation 
        public void InitialseStepParents()
        {
            GameObject firstModel = GetFirstModel();
            firstModel.AddComponent<CAS_FirstModel>(); 

            List<GameObject> models = GetInitialModels();
            totalInitialNumberOfModels = models.Count;
            Debug.Log(models.Count);

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
            stepParent.name = "Step " + (stepParents.Count + 1); 

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

        public GameObject GetFirstModel()
        {
            return transform.GetChild(0).gameObject; 
        }

        List<GameObject> GetGameObjectsFromModelId(List<string> modelsId)
        {
            List<GameObject> models = new List<GameObject>();

            foreach (string id in modelsId)
            {
                models.Add(allModelsInformationByRecordName[id]);
            }

            return models; 
        }

        List<GameObject> GetGameObjectsAll()
        {
            List<GameObject> models = new List<GameObject>();

            foreach (string id in allModelsInformationByRecordName.Keys)
            {
                models.Add(allModelsInformationByRecordName[id]);
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
            RemoveGroupByModelsToEditLayers(); 

            int index = 0;

            foreach (string key in filteredPatientIdsGroupBy.Keys)
            {
                foreach (string value in filteredPatientIdsGroupBy[key])
                {
                    if (allModelsInformationByRecordName.ContainsKey(value))
                    {
                        if(index >= manager.dataManager.colorsForGrouping.Length)
                        {
                            index = 0; 
                        }
                        allModelsInformationByRecordName[value].GetComponentInChildren<CAS_ContolModel>().SetGroupByColour(manager.dataManager.colorsForGrouping[index]);                
                    }
                }
                index++;
            }
        }

        public void SetSortByModelsToEditLayers(CAS_FilterAndGroupUIStep.SortByStructure sorted, bool withFilter)
        {
            RemoveGroupByModelsToEditLayers();

            GameObject sortManager = new GameObject();
            sortManager.AddComponent<CAS_EachStepManager>();
            sortManager.AddComponent<CAS_PlaceModels>();
            sortManager.transform.parent = transform;

            List<List<GameObject>> originalObjectsList = new List<List<GameObject>>();            
            foreach (CAS_EachStepManager stepManager in stepParents)
            {
                List<GameObject> originalObjectList = new List<GameObject>();
                foreach (Transform child in stepManager.transform)
                {
                    originalObjectList.Add(child.gameObject);
                }
                originalObjectsList.Add(originalObjectList);  
            }

            int index = 0;
            int stringIndex = 0;
            string previousKey = "";
            string currentKey = "";
            if (sorted.isString)
            {
                previousKey = sorted.stringValues[0];
                currentKey = sorted.stringValues[0];
            }

            foreach (List<GameObject> originalObjectList in originalObjectsList)
            {
                foreach (GameObject originalObject in originalObjectList)
                {
                    originalObject.transform.parent = sortManager.transform;
                }
            }

            foreach (string patientId in sorted.patientIds)
            {
                if (allModelsInformationByRecordName.ContainsKey(patientId))
                {
                    allModelsInformationByRecordName[patientId].transform.SetAsLastSibling();

                    if (!sorted.isString)
                    {
                        allModelsInformationByRecordName[patientId].GetComponentInChildren<CAS_ContolModel>().SetGroupByColour(GetSortColorDouble((float)sorted.min, (float)sorted.max, (float)sorted.doubleValues[index]));
                    }
                    else
                    {
                        previousKey = currentKey;
                        currentKey = sorted.stringValues[index];

                        if (previousKey != currentKey) stringIndex++; 
                        if (stringIndex >= manager.dataManager.colorsForGrouping.Length)
                        {
                            stringIndex = 0;
                        }
                        allModelsInformationByRecordName[patientId].GetComponentInChildren<CAS_ContolModel>().SetGroupByColour(manager.dataManager.colorsForGrouping[stringIndex]);                       
                    }
                }
                index++; 
            }

            sortManager.GetComponent<CAS_EachStepManager>().PlaceModels();

            int stepParentsIndex = 0; 
            foreach (CAS_EachStepManager stepManager in stepParents)
            {
                foreach (GameObject originalObject in originalObjectsList[stepParentsIndex])
                {
                    originalObject.transform.parent = stepManager.transform;
                    for(int i = 0; i <= stepParentsIndex; i++)
                    {
                        originalObject.GetComponent<CAS_ContolModel>().ChangeLayer();

                        if (withFilter)
                        {
                            if (stepParentsIndex != stepParents.Count - 1)
                            {
                                originalObject.GetComponentInChildren<CAS_ContolModel>().UnSetGroupByColour();
                            }
                        }
                    }
                }
                stepParentsIndex++;
            }

            Destroy(sortManager.gameObject); 
        }

        public void RemoveGroupByModelsToEditLayers()
        {
            foreach (string value in allModelsInformationByRecordName.Keys.ToList())
            {
                if (value == null) continue; 
                allModelsInformationByRecordName[value].GetComponentInChildren<CAS_ContolModel>().UnSetGroupByColour();
            }           
        }

        /*public void SetTrueScale()
        {
            transform.localScale = Vector3.one;
        }

        public void SetOriginalScale()
        {
            transform.localScale = originalScale;
        }*/

        public void SetAllModelsInformation(Dictionary<string, GameObject> allModelsInformation)
        {
            allModelsInformationByRecordName = allModelsInformation;

            foreach (string id in allModelsInformationByRecordName.Keys)
            {

                if (!allModelsInformationByGameObjectName.ContainsKey(allModelsInformation[id].name))
                {
                    allModelsInformationByGameObjectName.Add(allModelsInformation[id].name, allModelsInformation[id]);
                }

                if (allModelsInformationGameobjectRecordName.ContainsKey(allModelsInformation[id].name))
                {
                    allModelsInformationGameobjectRecordName[allModelsInformation[id].name].Add(id); 
                }
                else
                {
                    List<string> idList = new List<string>();
                    idList.Add(id);
                    allModelsInformationGameobjectRecordName.Add(allModelsInformation[id].name, idList);
                }
                 
            }
        }

        public void DisplayThisModelData(string name)
        {
            manager.displayPatientDetailsUIManager.showDataUI.DisplayData(allModelsInformationGameobjectRecordName[name][0]);
        }

        public void HighlightModelForWhichDataIsDisplayed(string gameObjectName)
        {
            if(gameObjectForWhichDataIsDisplayed != "")
                allModelsInformationByRecordName[gameObjectForWhichDataIsDisplayed].GetComponentInChildren<CAS_ContolModel>().HighlightModelSinceSelected(false);
            gameObjectForWhichDataIsDisplayed = gameObjectName;

            if(gameObjectName != "")
            {
                allModelsInformationByRecordName[gameObjectName].GetComponentInChildren<CAS_ContolModel>().HighlightModelSinceSelected(true);
            }
        }

        public Color GetSortColorDouble(float min, float max, float value)
        {
            if(value == -1)
            {
                return Color.white; 
            }

            float colourValue = 1f - ((max - value) / (max - min));
            return new Color(0.90f - (colourValue / 1.20f), 0.90f - (colourValue / 1.10f), 0.90f - (colourValue / 1.60f));
        }

        public void OnClickControlledPullSwitch()
        {
            controlledPull = !controlledPull;

            if (controlledPull)
            {
                foreach(CAS_GrabInteractable grabInteractable in grabInteractables)
                {
                    grabInteractable.attachEaseInTime = 20f; 
                }
            }
            else
            {
                foreach (CAS_GrabInteractable grabInteractable in grabInteractables)
                {
                    grabInteractable.attachEaseInTime = 0.5f;
                }
            }
        }

        public void OnClickPushObjectsSwitch()
        {
            pushObjects = !pushObjects;

            if (pushObjects)
            {
                foreach (CAS_GrabInteractable grabInteractable in grabInteractables)
                {
                    grabInteractable.attachEaseInTime = 20f;
                }
            }
            else
            {
                foreach (CAS_GrabInteractable grabInteractable in grabInteractables)
                {
                    grabInteractable.attachEaseInTime = 0.5f;
                }
            }
        }
    }
}
