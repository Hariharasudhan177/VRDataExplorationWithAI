using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

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
        List<string> patientIdsInCompareList;

        public GameObject[] compareObjects;
        List<GameObject> rowsOfData; 

        public RenderTexture[] renderTextures;

        bool initialized = false; 

        // Start is called before the first frame update
        void Start()
        {
            rowsOfData = new List<GameObject>(); 
        }

        // Update is called once per frame
        void Update()
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;

                //AddIdToList("Hannover_JTSH_left");
                //AddIdToList("BL_20150731");
            }

            /*if (Input.GetKeyDown(KeyCode.G))
            {
                RemoveIdFromList("Hannover_JTSH_left");
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                AddIdToList("HG_20151127");
            }*/
        }

        public void Initialize()
        {
            List<string> coloumnNames = manager.dataManager.GetAllColumnNames();
            gridLayout.constraintCount = initialColumns + coloumnNames.Count;

            Instantiate(eachFieldHeading, parentContent).GetComponent<CAS_EachFieldOfCompare>().SetContent("");
            Instantiate(eachFieldHeading, parentContent).GetComponent<CAS_EachFieldOfCompare>().SetContent("Model");

            foreach (string column in coloumnNames)
            {
                GameObject eachfieldObject = Instantiate(eachFieldHeading, parentContent);
                eachfieldObject.GetComponent<CAS_EachFieldOfCompare>().SetContent(column);
            }

            patientIdsInCompareList = new List<string>(); 
        }

        public void PopulateData()
        {
            foreach (GameObject compareObject in compareObjects)
            {
                compareObject.GetComponent<CAS_CompareObject>().DeActivateCompareObject();
                compareObject.SetActive(false); 
            }

            foreach (GameObject rowOfData in rowsOfData)
            {
                Destroy(rowOfData);
            }

            int index = 0;

            foreach (string patientId in patientIdsInCompareList)
            {
                GameObject deleteButtonObject = Instantiate(eachDeleteButton, parentContent);
                deleteButtonObject.GetComponent<CAS_DeleteComparedObject>().SetComparedPatientId(patientId);
                rowsOfData.Add(deleteButtonObject);

                compareObjects[index].SetActive(true);
                compareObjects[index].GetComponent<CAS_CompareObject>().ActivateCompareObject(manager.stepManager.allModelsInformationByRecordName[patientId]);

                GameObject modelSnapshotObject = Instantiate(eachSnapShotPrefab, parentContent);
                modelSnapshotObject.GetComponent<CAS_CompareSnapshot>().SetRawImage(renderTextures[index]);
                rowsOfData.Add(modelSnapshotObject);

                List<string> patientRecord = manager.dataManager.GetPatientRecordWithIdWithoutKeys(patientId);

                foreach (string patientRecordData in patientRecord)
                {
                    GameObject eachfieldObject = Instantiate(eachFieldPrefab, parentContent);
                    eachfieldObject.GetComponent<CAS_EachFieldOfCompare>().SetContent(patientRecordData);
                    rowsOfData.Add(eachfieldObject); 
                }

                index++;
            }
        }

        public void AddIdToList(string patientId)
        {
            if(!(patientIdsInCompareList.Count > maxNumber))
            {
                patientIdsInCompareList.Add(patientId); 
            }

            PopulateData();
        }

        public void RemoveIdFromList(string patientId)
        {
            int index = 0; 
            foreach(string id in patientIdsInCompareList)
            {
                if(id == patientId)
                {
                    patientIdsInCompareList.RemoveAt(index);
                    Debug.Log(patientIdsInCompareList.Count);
                    break; 
                }
                index++;
            }

            PopulateData();
        }
    }
}