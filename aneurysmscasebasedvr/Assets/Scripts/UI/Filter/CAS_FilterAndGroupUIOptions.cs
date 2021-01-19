using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_FilterAndGroupUIOptions : MonoBehaviour
    {
        public CAS_FilterAndGroupUIManager filterAndGroupManager;

        public GameObject filterAndGroupOptionTypePanelPrefab;
        public GameObject filterAndGroupOptionTypePanelPrefabParent;

        public GameObject filterAndGroupOptionTypeButtonPrefab;

        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetFilterOptionSelected(string key)
        {
            filterAndGroupManager.SetFilterOptionSelected(key); 
        }

        public void PopulateFilterOptions()
        {
            List<GameObject> objectsToSwap = new List<GameObject>();

            foreach (string key in filterAndGroupManager.GetOptionsUnderSpecificTypes().Keys)
            {                
                GameObject filterAndGroupOptionTypePanel = Instantiate(filterAndGroupOptionTypePanelPrefab, filterAndGroupOptionTypePanelPrefabParent.transform);
                filterAndGroupOptionTypePanel.name = key;
                filterAndGroupOptionTypePanel.SetActive(false);

                CAS_EachFilterAndGroupOptionType filterAndGroupOptionsType = filterAndGroupOptionTypePanel.GetComponent<CAS_EachFilterAndGroupOptionType>();
                filterAndGroupOptionsType.SetOptionButtons(filterAndGroupOptionTypeButtonPrefab, filterAndGroupManager.GetOptionsUnderSpecificTypes()[key]);
                objectsToSwap.Add(filterAndGroupOptionTypePanel); 
            }

            CAS_TabGroup tabGroupOfFilterAndGroupOption = GetComponentInChildren<CAS_TabGroup>();
            tabGroupOfFilterAndGroupOption.ObjectsToSwap = objectsToSwap;
            objectsToSwap[0].SetActive(true); 
        }

        public TypeOfOptions FindTypeOfOptions(string columnHeading)
        {
            return filterAndGroupManager.manager.dataManager.GetColumnTypeOfOption(columnHeading); 
        }

        public void OpenClose(bool status)
        {
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
    }
}

