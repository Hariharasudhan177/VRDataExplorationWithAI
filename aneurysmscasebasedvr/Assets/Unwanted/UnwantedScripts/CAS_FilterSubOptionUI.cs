using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_FilterSubOptionUI : MonoBehaviour
    {
        public CAS_DataManager dataManager;
        public CAS_FilterUI filterUI;

        public string filterOptionName;

        public GameObject parentContent;

        //Store names and status for each of the sub obtion to loop through filter button is clicked 
        //replacing with dictionary - delete others if this is working 
        //List<string> filterSubOptionsName; 
        //bool[] filterSubOptionsStatus;  
        [HideInInspector]
        public Dictionary<string, bool> filterSubOptionsStatus; 

        //First time value population - Move this to onEnable later if possible 
        bool instantiated = false;

        bool visible = false;

        // Start is called before the first frame update
        void Start()
        {
            //Issue if the hierarchy changes 
            filterUI = transform.parent.GetComponentInChildren<CAS_FilterUI>();
            dataManager = filterUI.dataManager;

            //Setting camera for worldspace ui - Not set automatically when created 
            GetComponent<Canvas>().worldCamera = Camera.main; 

            if (!instantiated)
            {
                Instantiate();
            } 
        }

        void Instantiate()
        {
            instantiated = true;
            PopulateFilterOptions();
        }


        void PopulateFilterOptions()
        {
            List<object> filterSubOptions = dataManager.GetFilterSubOptions(filterOptionName);
            filterSubOptionsStatus = new Dictionary<string, bool>(); 
            //filterSubOptionsName = new List<string>();

            foreach (var filterSubOption in filterSubOptions)
            {
                GameObject filterSubOptionInstantiatedToggle = Instantiate(filterUI.filterSubOptionToggle, parentContent.transform);
                filterSubOptionInstantiatedToggle.name = filterSubOption.ToString();
                filterSubOptionInstantiatedToggle.GetComponent<CAS_FilterSubOption>().SetFitlerSubOptionLabel(filterSubOption.ToString());
                filterSubOptionsStatus.Add(filterSubOption.ToString(), false); 
                //filterSubOptionsName.Add(filterSubOption.ToString()); 
            }

            //filterSubOptionsStatus = new bool[filterSubOptionsName.Count]; 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetFitlerOptionName(string value)
        {
            filterOptionName = value;
        }

        public void OpenSubOptionFilterPanel()
        {
            visible = true;
            ActivateFilterPanel(visible);
        }

        public void CloseSubOptionFilterPanel()
        {
            visible = false;
            ActivateFilterPanel(visible);
        }

        public void OpenCloseSubOptionFilterPanel()
        {
            visible = !visible;
            ActivateFilterPanel(visible);
        }

        void ActivateFilterPanel(bool _visible)
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

        //Storing all the subobtion toggle status here to access from filterUI 
        //So having the click at each of the filterSubOptionUI
        //Clicked filter option - opening corresponding filter suboption and hiding others 
        public void ToggledThisFilterSubOption(string name, bool value)
        {
            Debug.Log(name + " " + value); 
            filterSubOptionsStatus[name] = value; 
        }
    }
}

