using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data; 
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_ObjectOfInterestUI : MonoBehaviour
    {
        public CAS_AIUI aiUI; 

        public TMP_Dropdown examples;
        List<string> dropDownOptions;

        public GameObject parentContentForData;
        public GameObject eachFieldOfDataPrefab; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateExamplesDropDown()
        {
            examples.ClearOptions(); 
            dropDownOptions = new List<string>();

            dropDownOptions.Add("None"); 

            foreach (CAS_ObjectOfInterest example in aiUI.aiManager.GetObjectsOfInterest())
            {
                dropDownOptions.Add(example.gameObject.name); 
            }
            //Add the options created in the List above
            examples.AddOptions(dropDownOptions);
            //PopulateData(dropDownOptions[0]);
            //aiUI.aiManager.SetObjectOfInterest(0);
        }

        public void PopulateData(string id)
        {
            foreach (Transform toDelete in parentContentForData.transform)
            {
                Destroy(toDelete.gameObject);
            }

            Debug.Log(id);
            DataTable rowToKeyValuePair = aiUI.aiManager.manager.dataManager.GetPatientRecordWithId(id);

            foreach (var column in rowToKeyValuePair.Columns)
            {
                GameObject eachFieldOfDataInstantiated = Instantiate(eachFieldOfDataPrefab, parentContentForData.transform);
                eachFieldOfDataInstantiated.GetComponent<CAS_EachFieldOfData>().SetColumnNameAndFieldData(column.ToString(), rowToKeyValuePair.Rows[0][column.ToString()].ToString());
            }
        }

        public void UnPopulateData()
        {
            foreach (Transform toDelete in parentContentForData.transform)
            {
                Destroy(toDelete.gameObject);
            }
        }

        public void OnDropDownValueChanged(int dropDownIndex)
        {
            if(dropDownIndex != 0)
            {
                PopulateData(dropDownOptions[dropDownIndex]);
                aiUI.aiManager.SetObjectOfInterest(dropDownIndex-1);
            }
            else
            {
                UnPopulateData();
                aiUI.aiManager.UnSetObjectOfInterest();
            }
        }

        public void ActivatePanel(bool _visible)
        {
            if (_visible)
            {
                GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0;
            }
            GetComponent<CanvasGroup>().interactable = _visible;
            GetComponent<TrackedDeviceGraphicRaycaster>().enabled = _visible;
        }
    }
}