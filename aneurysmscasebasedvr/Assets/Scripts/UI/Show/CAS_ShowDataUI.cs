﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

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

        public CAS_DataManager dataManager;

        bool visible = false;

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

        public void PopulateData(string id)
        {
            if (visible)
            {
                foreach(Transform toDelete in parentContent.transform)
                {
                    Destroy(toDelete.gameObject); 
                }

                Dictionary<string, string> patientRecord = dataManager.GetPatientRecordWithId(id);

                foreach (KeyValuePair<string, string> entry in patientRecord)
                {
                    GameObject eachFieldOfDataInstantiated = Instantiate(eachFieldOfData, parentContent.transform);
                    eachFieldOfDataInstantiated.GetComponent<CAS_EachFieldOfData>().SetColumnNameAndFieldData(entry.Key, entry.Value);
                }
            }

        }

        public void UnPopulateData()
        {
            if (visible)
            {
                foreach (Transform toDelete in parentContent.transform)
                {
                    Destroy(toDelete.gameObject);
                }
            }

        }
    }
}

