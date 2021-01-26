using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_FilterUI : MonoBehaviour
    {
        public GameObject filterOptionButton;
        public GameObject parentContent;

        public GameObject filterSubOptionPanel;
        //Need seperate parent because of tracked** - stop interaction when hidden 
        public GameObject filterSubOptionPanelParent;

        public GameObject filterSubOptionToggle;

        public CAS_DataManager dataManager;
        public CAS_StepManager stepManager;

        //For hiding and showing - Should only use one in future 
        Dictionary<string, CAS_FilterSubOptionUI> filterSubOptionUIDictionary;
        CAS_FilterSubOptionUI[] filtersubOptionUIList; 
        
        //First time value population - Move this to onEnable later if possible 
        bool instantiated = false;

        bool visible = false; 

        // Start is called before the first frame update
        void Start()
        {
            filterSubOptionUIDictionary = new Dictionary<string, CAS_FilterSubOptionUI>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public void OpenCloseFilterPanel()
        {
            if(!instantiated)
            { 
                Instantiate();
            }

            visible = !visible;
            ActivateFilterPanel(visible); 
        }

        void Instantiate()
        {
            instantiated = true;
            PopulateFilterOptions(); 
        }

        void PopulateFilterOptions()
        {
            string[] filterOptions = dataManager.GetFilterOptions(); 

            foreach(string filterOption in filterOptions)
            {
                //Skipping index and id directly for now 
                if(filterOption == "index" || filterOption == "id")
                {
                    continue; 
                }
                GameObject filterOptionInstantiatedButton = Instantiate(filterOptionButton, parentContent.transform);
                filterOptionInstantiatedButton.name = filterOption;
                filterOptionInstantiatedButton.GetComponent<CAS_FilterOption>().SetFitlerOptionLabel(filterOption);

                //Parent set this one's parent 
                GameObject filterSubOptionInstantiatedPanel = Instantiate(filterSubOptionPanel, filterSubOptionPanelParent.transform);
                filterSubOptionInstantiatedPanel.name = filterOption + "FilterSubOptionUI";
                filterSubOptionInstantiatedPanel.GetComponent<CAS_FilterSubOptionUI>().SetFitlerOptionName(filterOption);
                //Setting position and rotation for now 
                filterSubOptionInstantiatedPanel.transform.localPosition = new Vector3(-1.2f, 0f, 0.3f);
                filterSubOptionInstantiatedPanel.transform.Rotate(0, 210, 0);  

                //Dictionary which has the filter option name and its corresponding filterSubOptionUI
                filterSubOptionUIDictionary.Add(filterOption, filterSubOptionInstantiatedPanel.GetComponent<CAS_FilterSubOptionUI>());
                filtersubOptionUIList = FindObjectsOfType<CAS_FilterSubOptionUI>();
            }
        }

        void ActivateFilterPanel(bool _visible)
        {
            if(_visible)
            {
                GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0;
                CloseFilterSubOptionUI(); 
            }
            GetComponent<CanvasGroup>().interactable = _visible;
            GetComponent<TrackedDeviceGraphicRaycaster>().enabled = _visible; 
        }

        //Clicked filter option - opening corresponding filter suboption and hiding others 
        public void ClickedThisFilterOption(string name)
        {
            foreach(var fitlerSubOptionUI in filtersubOptionUIList)
            {
                if (fitlerSubOptionUI.filterOptionName == name)
                {
                    fitlerSubOptionUI.OpenSubOptionFilterPanel();
                }
                else
                {
                    fitlerSubOptionUI.CloseSubOptionFilterPanel();
                }
            }
        }

        //Close SubOption UI when Filteroptio ui is closed 
        public void CloseFilterSubOptionUI()
        {
            foreach (var fitlerSubOptionUI in filtersubOptionUIList)
            {
                fitlerSubOptionUI.CloseSubOptionFilterPanel();
            }
        }

        //OnClick filter button - take all the filteroptionstatus values and send query 
        public void OnClickFilterButton()
        {
            List<string> filterOptions = new List<string>();
            List<List<string>> filterOptionsValues = new List<List<string>>();

            foreach (var fitlerSubOptionUI in filtersubOptionUIList)
            {
                bool oneOfTheSubOptionOfThisOptionIsTrue = false; 
                List<string> filterSubOptions = new List<string>();
                foreach (KeyValuePair<string, bool> entry in fitlerSubOptionUI.filterSubOptionsStatus)
                {
                    if(entry.Value == true)
                    {
                        filterSubOptions.Add(entry.Key);
                        oneOfTheSubOptionOfThisOptionIsTrue = true; 
                    }
                }
                if (oneOfTheSubOptionOfThisOptionIsTrue)
                {
                    filterOptions.Add(fitlerSubOptionUI.filterOptionName);
                    filterOptionsValues.Add(filterSubOptions);
                }
            }

            //List<string> filteredPatientIds = dataManager.GetFilteredPatientIds(filterOptions, filterOptionsValues);
            //stepManager.IncreaseSteps(filteredPatientIds); 
        }
    }
}

