using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_CompareManager : MonoBehaviour
    {
        public CAS_Manager manager;

        //Layout 
        public GridLayoutGroup gridLayout;
        //Delete button and model pic
        public int initialColumns = 2;
        public Transform parentContent;

        public GameObject eachFieldHeading;
        public GameObject eachDeleteButton;
        public GameObject eachSnapShotPrefab;
        public GameObject eachFieldPrefab;

        //PatientIds 
        public int maxNumber = 10; 
        string[] patientIdsInCompareList;

        public GameObject[] compareObjects;
        List<GameObject>[] compareIndividualObjectsList;
        bool[] compareIndividualObjectsStatus;

        public RenderTexture[] renderTextures;

        bool initialized = false;

        bool compareUIVisibilityStatus = true;

        public CAS_CompareTrigger compareTriggerLeft;
        public CAS_CompareTrigger compareTriggerRight;


        // Start is called before the first frame update
        void Start()
        {
            patientIdsInCompareList = new string[maxNumber]; 
            compareIndividualObjectsStatus = new bool[maxNumber];
            compareIndividualObjectsList = new List<GameObject>[maxNumber];

            for (int i = 0; i < maxNumber; i++)
            {
                patientIdsInCompareList[i] = "";
                compareIndividualObjectsStatus[i] = false; 
                compareIndividualObjectsList[i] = new List<GameObject>(); 
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            /*if (Input.GetKeyDown(KeyCode.G))
            {
                RemoveIdFromList("Hannover_JTSH_left");
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                AddIdToList("HG_20151127_0");
            }*/
        }

        public void Initialize()
        {
            List<string> coloumnNames = manager.dataManager.GetAllColumnNamesForCompare();
            gridLayout.constraintCount = initialColumns + coloumnNames.Count;

            Instantiate(eachFieldHeading, parentContent).GetComponent<CAS_EachFieldOfCompare>().SetContent("");
            Instantiate(eachFieldHeading, parentContent).GetComponent<CAS_EachFieldOfCompare>().SetContent("Model");

            foreach (string column in coloumnNames)
            {
                GameObject eachfieldObject = Instantiate(eachFieldHeading, parentContent);
                eachfieldObject.GetComponent<CAS_EachFieldOfCompare>().SetContent(column);
            }

            //tientIdsInCompareList = new List<string>(); 
        }

        public void PopulateData(string patientId, int index, bool objectOfInterest)
        {
            compareIndividualObjectsList[index] = new List<GameObject>(); 

            GameObject deleteButtonObject = Instantiate(eachDeleteButton, parentContent);
            deleteButtonObject.GetComponent<CAS_DeleteComparedObject>().SetComparedPatientId(patientId);
            compareIndividualObjectsList[index].Add(deleteButtonObject);

            compareObjects[index].SetActive(true);

            if (objectOfInterest)
            {
                compareObjects[index].GetComponent<CAS_CompareObject>().ActivateCompareObject(manager.aiManager.allModelsInformationByRecordName[patientId]);
            }
            else
            {
                compareObjects[index].GetComponent<CAS_CompareObject>().ActivateCompareObject(manager.stepManager.allModelsInformationByRecordName[patientId]);
            }
 

            GameObject modelSnapshotObject = Instantiate(eachSnapShotPrefab, parentContent);
            modelSnapshotObject.GetComponent<CAS_CompareSnapshot>().SetRawImage(renderTextures[index]);
            compareIndividualObjectsList[index].Add(modelSnapshotObject);

            List<string> patientRecord = manager.dataManager.GetPatientRecordWithIdWithoutKeys(patientId);

            foreach (string patientRecordData in patientRecord)
            {
                GameObject eachfieldObject = Instantiate(eachFieldPrefab, parentContent);
                eachfieldObject.GetComponent<CAS_EachFieldOfCompare>().SetContent(patientRecordData);
                compareIndividualObjectsList[index].Add(eachfieldObject); 
            }
        }

        public void UnPopulateData(int index)
        {
            foreach (GameObject compareObject in compareIndividualObjectsList[index])
            {
                Destroy(compareObject); 
            }

            compareObjects[index].GetComponent<CAS_CompareObject>().DeActivateCompareObject();
            compareObjects[index].SetActive(false); 
        }

        public bool AddIdToList(string patientId, bool objectOfInterest)
        {
            //If patient id is already present 
            foreach(string patientIdPresent in patientIdsInCompareList)
            {
                if(patientIdPresent == patientId)
                {
                    return false; 
                }
            }

            //Compare is set to max of maxNumber. Find which index is not filled yet
            int index = -1; 
            for (int i = 0; i < compareIndividualObjectsStatus.Length; i++)
            {
                if (!compareIndividualObjectsStatus[i])
                {
                    index = i;
                    break;
                }
            }

            //If all slots filled return 
            if (index == -1) return false;

            patientIdsInCompareList[index] = patientId;
            compareIndividualObjectsStatus[index] = true;
            PopulateData(patientId, index, objectOfInterest);

            return true; 
        }

        public void RemoveIdFromList(string patientId)
        {
            int index = 0; 
            foreach(string id in patientIdsInCompareList)
            {
                if(id == patientId)
                {
                    patientIdsInCompareList[index] = "";
                    compareIndividualObjectsStatus[index] = false;
                    UnPopulateData(index);
                    break; 
                }
                index++;
            }
        }

        public void OpenClose()
        {
            compareUIVisibilityStatus = !compareUIVisibilityStatus;

            if (compareUIVisibilityStatus)
            {
                GetComponentInChildren<CanvasGroup>().alpha = 1;
            }
            else
            {
                GetComponentInChildren<CanvasGroup>().alpha = 0;
            }

            GetComponentInChildren<CanvasGroup>().interactable = compareUIVisibilityStatus;
            GetComponentInChildren<TrackedDeviceGraphicRaycaster>().enabled = compareUIVisibilityStatus;
        }

        public void ClearAllCompareSlots()
        {
            for(int i=0; i < compareIndividualObjectsStatus.Length; i++)
            {
                if (compareIndividualObjectsStatus[i])
                {

                    foreach (GameObject compareObject in compareIndividualObjectsList[i])
                    {
                        Destroy(compareObject);
                    }

                    compareObjects[i].GetComponent<CAS_CompareObject>().DeActivateCompareObject();
                    compareObjects[i].SetActive(false);

                    patientIdsInCompareList[i] = "";
                    compareIndividualObjectsStatus[i] = false; 
                    compareIndividualObjectsList[i] = new List<GameObject>(); 
                }
            }
        }
    }
}