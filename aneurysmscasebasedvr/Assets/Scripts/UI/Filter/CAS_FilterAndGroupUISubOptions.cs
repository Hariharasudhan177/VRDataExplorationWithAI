using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_FilterAndGroupUISubOptions : MonoBehaviour
    {
        public CAS_FilterAndGroupUIManager filterAndGroupUIManager;

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
            /*if (Input.GetKeyDown(KeyCode.R))
            {
                RemoveAllFilters(); 
            }*/
        }

        public void PopulateFilterOptions()
        {
            foreach (string key in filterAndGroupUIManager.GetOptionsUnderSpecificTypes().Keys)
            {
                List<string> filterOptionsOfThisType = filterAndGroupUIManager.GetOptionsUnderSpecificTypes()[key];

                foreach (string filterOption in filterOptionsOfThisType)
                {
                    GameObject filterSubOptionPanel = Instantiate(filterAndGroupSubOptionPanelPrefab, filterAndGroupSubOptionPanelPrefabParent.transform);
                    filterSubOptionPanel.GetComponent<CAS_EachFilterAndGroupSubOptionPanel>().SetFitlerOptionName(filterOption);
                    filterSubOptionPanel.name = filterOption; 
                    filterSubOptionPanelsDict.Add(filterOption, filterSubOptionPanel);
                    //filterSubOptionPanel.SetActive(false);
                    filterSubOptionPanel.GetComponent<CAS_EachFilterAndGroupSubOptionPanel>().initialStatus = false;  
                }
            }

            currentFilterSubOptionPanelSelected = filterSubOptionPanelsDict.Keys.ToList()[0];
            //filterSubOptionPanelsDict[currentFilterSubOptionPanelSelected].SetActive(true); 
            filterSubOptionPanelsDict[currentFilterSubOptionPanelSelected].GetComponent<CAS_EachFilterAndGroupSubOptionPanel>().initialStatus = true;
        }

        public void SetfilterSubOptionPanelsSelected(string key)
        {
            filterSubOptionPanelsDict[currentFilterSubOptionPanelSelected].SetActive(false);
            filterSubOptionPanelsDict[key].SetActive(true);
            currentFilterSubOptionPanelSelected = key; 
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

        public void RemoveAllFilters()
        {
            Debug.Log(filterSubOptionPanelsDict.Keys.ToList().Count); 
            foreach (string key in filterSubOptionPanelsDict.Keys.ToList())
            {
                filterSubOptionPanelsDict[key].GetComponent<CAS_EachFilterAndGroupSubOptionPanel>().RemoveFilter(); 
            }
        }
    }
}