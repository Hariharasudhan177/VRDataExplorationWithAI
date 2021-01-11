using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

namespace CAS
{
    public class CAS_FilterAndGroupSubOptions : MonoBehaviour
    {
        public CAS_FilterAndGroupManager filterAndGroupManager;

        public GameObject filterAndGroupSubOptionPanelPrefab;
        public GameObject filterAndGroupSubOptionPanelPrefabParent;

        public GameObject filterSubOptionTogglePrefab;
        public GameObject filterSubOptionSliderPrefab;

        //A way to do the same with only one rather than using both dict and list 
        public Dictionary<string, GameObject> filterSubOptionPanelsDict;
        string currentFilterSubOptionPanelSelected; 

        // Start is called before the first frame update
        void Awake()
        {
            filterSubOptionPanelsDict = new Dictionary<string, GameObject>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulateFilterOptions()
        {
            foreach (string key in filterAndGroupManager.GetOptionsUnderSpecificTypes().Keys)
            {
                List<string> filterOptionsOfThisType = filterAndGroupManager.GetOptionsUnderSpecificTypes()[key];

                foreach (string filterOption in filterOptionsOfThisType)
                {
                    GameObject filterSubOptionPanel = Instantiate(filterAndGroupSubOptionPanelPrefab, filterAndGroupSubOptionPanelPrefabParent.transform);
                    filterSubOptionPanel.GetComponent<CAS_FilterAndGroupSubOptionPanel>().SetFitlerOptionName(filterOption);
                    filterSubOptionPanel.name = filterOption; 
                    filterSubOptionPanelsDict.Add(filterOption, filterSubOptionPanel);
                    //filterSubOptionPanel.SetActive(false);
                    filterSubOptionPanel.GetComponent<CAS_FilterAndGroupSubOptionPanel>().initialStatus = false;  
                }
            }

            currentFilterSubOptionPanelSelected = filterSubOptionPanelsDict.Keys.ToList()[0];
            //filterSubOptionPanelsDict[currentFilterSubOptionPanelSelected].SetActive(true); 
            filterSubOptionPanelsDict[currentFilterSubOptionPanelSelected].GetComponent<CAS_FilterAndGroupSubOptionPanel>().initialStatus = true;
        }

        public void SetfilterSubOptionPanelsSelected(string key)
        {
            filterSubOptionPanelsDict[currentFilterSubOptionPanelSelected].SetActive(false);
            filterSubOptionPanelsDict[key].SetActive(true);
            currentFilterSubOptionPanelSelected = key; 
        }
    }
}