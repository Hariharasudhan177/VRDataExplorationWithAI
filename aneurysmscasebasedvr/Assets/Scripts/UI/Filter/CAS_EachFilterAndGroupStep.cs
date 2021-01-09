using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace CAS
{
    public enum FilterStepStatus
    {
        Inactive, 
        Active, 
        FilterAdded, 
        Filtered
    }

    public class CAS_EachFilterAndGroupStep : MonoBehaviour
    {
        CAS_FilterAndGroupManager filterAndGroupManager;

        public FilterStepStatus filterStepStatus;

        Dictionary<string, List<string>> filterSubOptionsSelected;
        Dictionary<string, List<double>> filterSubOptionsSelectedInteger;

        public Transform filterDisplayListParent;
        
        public int stepNumber;

        public GameObject filterButton;
        public GameObject removeAllButton; 

        void Awake()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupManager = GetComponentInParent<CAS_FilterAndGroupManager>(); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddFilterToThisStep(string filterKey, List<string> filterOptions)
        {
            filterStepStatus = FilterStepStatus.FilterAdded; 
            filterSubOptionsSelected.Add(filterKey, filterOptions);

            string displayList = "";
            foreach (string filterOptionKey in filterSubOptionsSelected.Keys)
            {
                displayList += filterOptionKey + " : ";
                foreach (string option in filterSubOptionsSelected[filterOptionKey])
                {
                    displayList += option + ",";
                }
            }

            SetDisplayContent(displayList); 
        }

        public void AddFilterToThisStepInteger(string filterKey, List<double> filterOptions)
        {
            filterStepStatus = FilterStepStatus.FilterAdded;
            filterSubOptionsSelectedInteger.Add(filterKey, filterOptions);

            string displayList = "";
            foreach (string filterOptionKey in filterSubOptionsSelectedInteger.Keys)
            {
                displayList += filterOptionKey + " : ";
                foreach (double option in filterSubOptionsSelectedInteger[filterOptionKey])
                {
                    displayList += option.ToString() + ",";
                }
            }

            SetDisplayContent(displayList);
        }

        public void RemoveFilterFromThisStep(string filterKey)
        {
            filterStepStatus = FilterStepStatus.FilterAdded;
            filterSubOptionsSelected.Remove(filterKey); 

            string displayList = "";
            foreach (string filterOptionKey in filterSubOptionsSelected.Keys)
            {
                displayList += filterOptionKey + " : ";               
                foreach(string option in filterSubOptionsSelected[filterOptionKey])
                {
                    displayList += option + ",";
                }
            }

            SetDisplayContent(displayList);
        }

        public void RemoveFilterFromThisStepInteger(string filterKey)
        {
            filterStepStatus = FilterStepStatus.FilterAdded;
            filterSubOptionsSelectedInteger.Remove(filterKey);

            string displayList = "";
            foreach (string filterOptionKey in filterSubOptionsSelectedInteger.Keys)
            {
                displayList += filterOptionKey + " : ";
                foreach (double option in filterSubOptionsSelectedInteger[filterOptionKey])
                {
                    displayList += option.ToString() + ",";
                }
            }

            SetDisplayContent(displayList);
        }

        public void OnClickFilterButton()
        {
            filterStepStatus = FilterStepStatus.Filtered;

            if (filterStepStatus == FilterStepStatus.FilterAdded)
            {
                List<string> filterOptions = new List<string>();
                List<List<string>> filterOptionsValues = new List<List<string>>();

                foreach (string key in filterSubOptionsSelected.Keys)
                {
                    filterOptions.Add(key);
                    filterOptionsValues.Add(filterSubOptionsSelected[key]);
                }

                List<string> filterOptionsInteger = new List<string>();
                List<List<double>> filterOptionsValuesInteger = new List<List<double>>();

                foreach (string key in filterSubOptionsSelectedInteger.Keys)
                {
                    filterOptionsInteger.Add(key);
                    filterOptionsValuesInteger.Add(filterSubOptionsSelectedInteger[key]);
                }

                List<string> filteredPatientIdsFromString = filterAndGroupManager.manager.dataManager.GetFilteredPatientIds(filterOptions, filterOptionsValues);
                List<string> filteredPatientIdsFromInteger = filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsInteger(filterOptions, filterOptionsValuesInteger);

                List<string> filteredPatientIds = new List<string>(filteredPatientIdsFromString.Count + filteredPatientIdsFromInteger.Count);
                filteredPatientIds.AddRange(filteredPatientIdsFromString);
                filteredPatientIds.AddRange(filteredPatientIdsFromInteger);

                filterAndGroupManager.manager.stepManager.IncreaseSteps(filteredPatientIds);

                filterAndGroupManager.filterStepManager.ActivateStep(stepNumber);
                filterAndGroupManager.filterStepManager.DeActivateStep(stepNumber-1);
            }
            else if(filterStepStatus == FilterStepStatus.Filtered)
            {
                List<string> filterOptions = new List<string>();
                List<List<string>> filterOptionsValues = new List<List<string>>();

                foreach (string key in filterSubOptionsSelected.Keys)
                {
                    filterOptions.Add(key);
                    filterOptionsValues.Add(filterSubOptionsSelected[key]);
                }

                List<string> filterOptionsInteger = new List<string>();
                List<List<double>> filterOptionsValuesInteger = new List<List<double>>();

                foreach (string key in filterSubOptionsSelectedInteger.Keys)
                {
                    filterOptionsInteger.Add(key);
                    filterOptionsValuesInteger.Add(filterSubOptionsSelectedInteger[key]);
                }

                List<string> filteredPatientIdsFromString = filterAndGroupManager.manager.dataManager.GetFilteredPatientIds(filterOptions, filterOptionsValues);
                List<string> filteredPatientIdsFromInteger = filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsInteger(filterOptions, filterOptionsValuesInteger);

                List<string> filteredPatientIds = new List<string>(filteredPatientIdsFromString.Count + filteredPatientIdsFromInteger.Count);
                filteredPatientIds.AddRange(filteredPatientIdsFromString);
                filteredPatientIds.AddRange(filteredPatientIdsFromInteger);         

                filterAndGroupManager.manager.stepManager.RefreshStep(filteredPatientIds, stepNumber);
            }
        }

        public void OnClickRemoveAllButton()
        {
            if (filterStepStatus == FilterStepStatus.Filtered)
            {
                filterAndGroupManager.manager.stepManager.DecreaseSteps();
                filterAndGroupManager.filterStepManager.ActivateStep(stepNumber);
                filterAndGroupManager.filterStepManager.DeActivateStep(stepNumber + 1);
            }
        }

        public void SetDisplayContent(string displayList)
        {
            GameObject displayListGameObject = Instantiate(filterAndGroupManager.filterStepManager.displayListPrefab, filterDisplayListParent);
            displayListGameObject.GetComponent<CAS_EachDisplayList>().SetDisplayContent(displayList); 
        }

        public void ActivateThisStep()
        {
            filterButton.GetComponent<Button>().interactable = true;
            removeAllButton.GetComponent<Button>().interactable = true; 
        }

        public void DeActivateThisStep()
        {
            filterButton.GetComponent<Button>().interactable = false;
            removeAllButton.GetComponent<Button>().interactable = false;
        }
    }
}

