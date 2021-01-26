using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.UI;
using TMPro; 

namespace CAS
{
    public class CAS_PatientIdListUI : MonoBehaviour
    {
        public CAS_DisplayPatientDetailsUIManager displayPatientDetailsUIManager;

        public GameObject eachPatientIdButtonPrefab;
        public GameObject eachPatientIdButtonParent;

        public TextMeshProUGUI selectedPatientIdTextBox; 

        Dictionary<string, CAS_EachPatientIdButton> patientIdButtonList; 

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulatePatientIdList()
        {
            patientIdButtonList = new Dictionary<string, CAS_EachPatientIdButton>();
            List<string> uniquePatientIdsList = displayPatientDetailsUIManager.manager.dataManager.GetUniquePatientIds(); 

            foreach(string patientId in uniquePatientIdsList)
            {
                GameObject patientIdButton = Instantiate(eachPatientIdButtonPrefab, eachPatientIdButtonParent.transform);
                patientIdButton.name = patientId; 
                CAS_EachPatientIdButton eachPatientIdButton = patientIdButton.GetComponent<CAS_EachPatientIdButton>();
                eachPatientIdButton.SetPatientIdButtonContent(patientId); 
                patientIdButtonList.Add(patientId, eachPatientIdButton); 
            }
        }

        public void OnClickPatientIdButton(string patientId)
        {
            //Debug.Log(patientId
            displayPatientDetailsUIManager.showDataUI.PopulateData(patientId); 
        }

        public void OpenClose(bool status)
        {
            if (status)
            {
                GetComponent<CanvasGroup>().alpha = 1; 

                foreach(string key in patientIdButtonList.Keys)
                {
                    patientIdButtonList[key].gameObject.SetActive(true); 
                }
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0;

                foreach (string key in patientIdButtonList.Keys)
                {
                    patientIdButtonList[key].gameObject.SetActive(false);
                }
            }

            GetComponent<CanvasGroup>().interactable = status;
            GetComponent<TrackedDeviceGraphicRaycaster>().enabled = status;
        }

        public void SetSelectedPatientId(string selectedPatientId)
        {
            selectedPatientIdTextBox.text = selectedPatientId; 
        }
    }
}