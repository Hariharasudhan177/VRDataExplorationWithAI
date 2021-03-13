using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.UI;
using System.Linq; 

namespace CAS
{
    /// <summary>
    /// To show data on hover 
    /// Similar to filter UI script 
    /// </summary>
    public class CAS_ShowDataUI : MonoBehaviour
    {
        public GameObject eachFieldOfData;
        public GameObject parentContent;

        public CAS_DisplayPatientDetailsUIManager displayPatientDetailsUIManager;

        bool visible = false;

        public Image lockUnlockImage;
        public Sprite lockSprite;
        public Sprite unLockSprite;

        bool modelsPlaced = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenCloseShowDataPanel()
        {
            visible = !visible;
            ActivateShowDataPanel(visible);
        }

        void ActivateShowDataPanel(bool _visible)
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

        public void DisplayData(string id)
        {
            if (visible)
            {
                PopulateData(id); 
            }
        }

        public void PopulateData(string id)
        {
            UnPopulateData();

            Dictionary<string, string> patientRecord = displayPatientDetailsUIManager.manager.dataManager.GetPatientRecordWithId(id);

            foreach (KeyValuePair<string, string> entry in patientRecord)
            {
                GameObject eachFieldOfDataInstantiated = Instantiate(eachFieldOfData, parentContent.transform);
                eachFieldOfDataInstantiated.GetComponent<CAS_EachFieldOfData>().SetColumnNameAndFieldData(entry.Key, entry.Value);
            }
            displayPatientDetailsUIManager.patientIdListUI.SetSelectedPatientId(id);
            displayPatientDetailsUIManager.manager.stepManager.HighlightModelForWhichDataIsDisplayed(id);
        }

        public void UnPopulateData()
        {
            foreach (Transform toDelete in parentContent.transform)
            {
                Destroy(toDelete.gameObject);
            }
        }

        public void OpenClose(bool status)
        {
            visible = status;

            if (status)
            {
                GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0;
            }

            GetComponent<CanvasGroup>().interactable = status;
            GetComponent<TrackedDeviceGraphicRaycaster>().enabled = status;
        }

        public void OnClickSwitchOnShowData(float value)
        {
            if (value == 0)
            {
                visible = false;
            }
            else
            {
                visible = true;
            }
        }

        public void OnClickLockUnlockButton()
        {
            visible = !visible;

            if (visible)
            {
                lockUnlockImage.sprite = unLockSprite;
            }
            else
            {
                lockUnlockImage.sprite = lockSprite;
            }
        }

        public void Initialize(string patientId)
        {
            PopulateData(patientId); 
        }
    }
}

