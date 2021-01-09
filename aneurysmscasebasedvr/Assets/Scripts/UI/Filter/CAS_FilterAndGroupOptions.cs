using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace CAS
{
    public class CAS_FilterAndGroupOptions : MonoBehaviour
    {
        public CAS_FilterAndGroupManager filterAndGroupManager;

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

        public void PopulateFilterOptions()
        {
            List<GameObject> objectsToSwap = new List<GameObject>();

            foreach (string key in filterAndGroupManager.GetOptionsUnderSpecificTypes().Keys)
            {                
                GameObject filterAndGroupOptionTypePanel = Instantiate(filterAndGroupOptionTypePanelPrefab, filterAndGroupOptionTypePanelPrefabParent.transform);
                filterAndGroupOptionTypePanel.name = key;
                filterAndGroupOptionTypePanel.SetActive(false); 

                CAS_FilterAndGroupOptionsType filterAndGroupOptionsType = filterAndGroupOptionTypePanel.GetComponent<CAS_FilterAndGroupOptionsType>();
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
    }
}

