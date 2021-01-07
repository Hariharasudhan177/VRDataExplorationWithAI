using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_FilterAndGroupSubOptions : MonoBehaviour
    {
        public CAS_FilterAndGroupManager filterAndGroupManager;

        public GameObject filterAndGroupSubOptionPanelPrefab;
        public GameObject filterAndGroupSubOptionPanelPrefabParent;

        public GameObject filterSubOptionTogglePrefab;

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
            foreach(string key in filterAndGroupManager.GetOptionsUnderSpecificTypes().Keys)
            {
                List<string> filterOptionsOfThisType = filterAndGroupManager.GetOptionsUnderSpecificTypes()[key]; 

                foreach(string filterOption in filterOptionsOfThisType)
                {
                    GameObject filterSubOptionPanel = Instantiate(filterAndGroupSubOptionPanelPrefab, filterAndGroupSubOptionPanelPrefabParent.transform);
                    filterSubOptionPanel.GetComponent<CAS_FilterAndGroupSubOptionPanel>().SetFitlerOptionName(filterOption);
                    filterSubOptionPanel.SetActive(false); 
                }
            }
        }
    }
}