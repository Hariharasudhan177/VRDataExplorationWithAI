using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 

namespace CAS
{
    public enum FilterStepStatus
    {
        Inactive, 
        Active, 
        FilterAdded, 
        Filtered, 
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

        GameObject displayListGameObject;

        public bool stepGrouped = false; 

        void Awake()
        {
            filterSubOptionsSelected = new Dictionary<string, List<string>>();
            filterSubOptionsSelectedInteger = new Dictionary<string, List<double>>();
        }
        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupManager = GetComponentInParent<CAS_FilterAndGroupManager>(); 
        }

        // Update is called once per frame
        void Update()
        {
            if(filterStepStatus == FilterStepStatus.Active)
            {
                filterAndGroupManager.filterStepManager.ActivateStepButton(stepNumber - 2);

                filterAndGroupManager.filterStepManager.DeActivateStepButton(stepNumber);
            }
            else if(filterStepStatus == FilterStepStatus.FilterAdded)
            {
                filterAndGroupManager.filterStepManager.DeActivateStepButton(stepNumber - 2);

                filterAndGroupManager.filterStepManager.DeActivateStepButton(stepNumber);
            }
            else if(filterStepStatus == FilterStepStatus.Filtered)
            {
                filterAndGroupManager.filterStepManager.DeActivateStepButton(stepNumber-2);

                //Activating Next Step
                filterAndGroupManager.filterStepManager.ActivateStepButton(stepNumber);
            }
            else if(filterStepStatus == FilterStepStatus.Inactive)
            {

            }
        }

        public void AddFilterToThisStep(string filterKey, List<string> filterOptions)
        {          
            if (filterStepStatus == FilterStepStatus.Active)
            {            
                filterStepStatus = FilterStepStatus.FilterAdded;
                filterAndGroupManager.filterStepManager.ActivateStep(stepNumber-1);
            }

            if(filterOptions.Count > 0)
            {
                filterSubOptionsSelected.Add(filterKey, filterOptions);
            }
            else
            {
                if (filterSubOptionsSelected.ContainsKey(filterKey))
                {
                    filterSubOptionsSelected.Remove(filterKey);
                }

                if((!(filterSubOptionsSelected.Count > 0)) && (filterStepStatus != FilterStepStatus.Filtered))
                {
                    filterStepStatus = FilterStepStatus.Active;
                }
            }

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
            if (filterStepStatus == FilterStepStatus.Active)
            {
                filterStepStatus = FilterStepStatus.FilterAdded;
                filterAndGroupManager.filterStepManager.ActivateStep(stepNumber-1);
            }

            if(filterOptions.Count > 0)
            {
                filterSubOptionsSelectedInteger.Add(filterKey, filterOptions);
            }
            else
            {
                if (filterSubOptionsSelectedInteger.ContainsKey(filterKey))
                {
                    filterSubOptionsSelectedInteger.Remove(filterKey);
                }

                if ((!(filterSubOptionsSelectedInteger.Count > 0)) && (filterStepStatus != FilterStepStatus.Filtered))
                {
                    filterStepStatus = FilterStepStatus.Active;
                }
            }

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
            if (filterStepStatus == FilterStepStatus.FilterAdded)
            {
                List<string> filterOptions = new List<string>();
                List<List<string>> filterOptionsValues = new List<List<string>>();

                for (int i = 0; i <= filterAndGroupManager.filterStepManager.activeAndCurrentStep; i++)
                {
                    foreach (string key in filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelected.Keys)
                    {
                        filterOptions.Add(key);
                        filterOptionsValues.Add(filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelected[key]);
                    }
                }

                List<string> filterOptionsInteger = new List<string>();
                List<List<double>> filterOptionsValuesInteger = new List<List<double>>();

                for(int i=0; i<= filterAndGroupManager.filterStepManager.activeAndCurrentStep; i++)
                {
                    foreach (string key in filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelectedInteger.Keys)
                    {
                        filterOptionsInteger.Add(key);
                        filterOptionsValuesInteger.Add(filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelectedInteger[key]);
                    }
                }

                //List<string> filteredPatientIdsFromString = filterAndGroupManager.manager.dataManager.GetFilteredPatientIds(filterOptions, filterOptionsValues);
                //List<string> filteredPatientIdsFromInteger = filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsInteger(filterOptionsInteger, filterOptionsValuesInteger);

                //List<string> filteredPatientIds = new List<string>(filteredPatientIdsFromString.Count + filteredPatientIdsFromInteger.Count);
                //filteredPatientIds.AddRange(filteredPatientIdsFromString);
                //filteredPatientIds.AddRange(filteredPatientIdsFromInteger);

                List<string> filteredPatientIds = filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsStringAndInteger(filterOptions, filterOptionsValues, filterOptionsInteger, filterOptionsValuesInteger);

                filterAndGroupManager.manager.stepManager.IncreaseSteps(filteredPatientIds);
                filterStepStatus = FilterStepStatus.Filtered;
            }
            else if(filterStepStatus == FilterStepStatus.Filtered)
            {
                List<string> filterOptions = new List<string>();
                List<List<string>> filterOptionsValues = new List<List<string>>();

                for (int i = 0; i <= filterAndGroupManager.filterStepManager.activeAndCurrentStep; i++)
                {
                    foreach (string key in filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelected.Keys)
                    {
                        filterOptions.Add(key);
                        filterOptionsValues.Add(filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelected[key]);
                    }
                }

                List<string> filterOptionsInteger = new List<string>();
                List<List<double>> filterOptionsValuesInteger = new List<List<double>>();

                for (int i = 0; i <= filterAndGroupManager.filterStepManager.activeAndCurrentStep; i++)
                {
                    foreach (string key in filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelectedInteger.Keys)
                    {
                        filterOptionsInteger.Add(key);
                        filterOptionsValuesInteger.Add(filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelectedInteger[key]);
                    }
                }

                //List<string> filteredPatientIdsFromString = filterAndGroupManager.manager.dataManager.GetFilteredPatientIds(filterOptions, filterOptionsValues);
                //List<string> filteredPatientIdsFromInteger = filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsInteger(filterOptionsInteger, filterOptionsValuesInteger);

                //List<string> filteredPatientIds = new List<string>(filteredPatientIdsFromString.Count + filteredPatientIdsFromInteger.Count);
                //filteredPatientIds.AddRange(filteredPatientIdsFromString);
                //filteredPatientIds.AddRange(filteredPatientIdsFromInteger);

                List<string> filteredPatientIds = filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsStringAndInteger(filterOptions, filterOptionsValues, filterOptionsInteger, filterOptionsValuesInteger);

                filterAndGroupManager.manager.stepManager.RefreshStep(filteredPatientIds, stepNumber);

                if(!(filterSubOptionsSelected.Count > 0))
                {
                    filterStepStatus = FilterStepStatus.Active; 
                }
            }
        }

        public void SetDisplayContent(string displayList)
        {
            if(!displayListGameObject)
            {
                displayListGameObject = Instantiate(filterAndGroupManager.filterStepManager.displayListPrefab, filterDisplayListParent);
            }
 
            displayListGameObject.GetComponent<CAS_EachDisplayList>().SetDisplayContent(displayList); 
        }

        public void ActivateThisStep()
        {
            filterButton.GetComponent<Button>().interactable = true;
            removeAllButton.GetComponent<Button>().interactable = true;
        }

        public void ApplyGroupbyThisStep(string filterKey)
        {
            stepGrouped = false; 

            List<string> filterOptions = new List<string>();
            List<List<string>> filterOptionsValues = new List<List<string>>();

            for (int i = 0; i <= filterAndGroupManager.filterStepManager.activeAndCurrentStep; i++)
            {
                foreach (string key in filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelected.Keys)
                {
                    filterOptions.Add(key);
                    filterOptionsValues.Add(filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelected[key]);
                }
            }

            List<string> filterOptionsInteger = new List<string>();
            List<List<double>> filterOptionsValuesInteger = new List<List<double>>();

            for (int i = 0; i <= filterAndGroupManager.filterStepManager.activeAndCurrentStep; i++)
            {
                foreach (string key in filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelectedInteger.Keys)
                {
                    filterOptionsInteger.Add(key);
                    filterOptionsValuesInteger.Add(filterAndGroupManager.filterStepManager.eachFilterAndGroupSteps[i].filterSubOptionsSelectedInteger[key]);
                }
            }

            //List<string> filteredPatientIdsFromString = filterAndGroupManager.manager.dataManager.GetFilteredPatientIds(filterOptions, filterOptionsValues);
            //List<string> filteredPatientIdsFromInteger = filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsInteger(filterOptionsInteger, filterOptionsValuesInteger);

            //List<string> filteredPatientIds = new List<string>(filteredPatientIdsFromString.Count + filteredPatientIdsFromInteger.Count);
            //filteredPatientIds.AddRange(filteredPatientIdsFromString);
            //filteredPatientIds.AddRange(filteredPatientIdsFromInteger);

            Dictionary<string, List<string>> filteredPatientIdsGroupBy = filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsStringAndIntegerGroupBy(filterOptions, filterOptionsValues, filterOptionsInteger, filterOptionsValuesInteger, filterKey);

            filterAndGroupManager.manager.stepManager.GroupByStep(filteredPatientIdsGroupBy, stepNumber - 1);
        }
    }
}

