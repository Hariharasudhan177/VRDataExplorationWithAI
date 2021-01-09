using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_FilterAndGroupSubOptionPanel : MonoBehaviour
    {
        public CAS_FilterAndGroupManager filterAndGroupManager;

        [HideInInspector]
        public string filterOptionName;

        //Store names and status for each of the sub obtion to loop through filter button is clicked 
        //replacing with dictionary - delete others if this is working 
        //List<string> filterSubOptionsName; 
        //bool[] filterSubOptionsStatus;  
        [HideInInspector]
        public Dictionary<string, bool> filterSubOptionsStatus;

        public GameObject parentContentOfToggle;

        //bool filterApplied = false;
        int stepNumberToWhichThisFilterIsApplied = -1; 

        public GameObject addFilterButton;
        public GameObject changeFilterButton;

        List<double> fromToValue;

        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupManager = GetComponentInParent<CAS_FilterAndGroupManager>();
            PopulateFilterOptions();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulateFilterOptions()
        {

            Type filterOptionDataType = filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName);
            Debug.Log(filterOptionName + " " + filterOptionDataType);

            if (filterOptionDataType == System.Type.GetType("System.Double"))
            {
                //UseLinq query and directly get the double value here 
                List<object> filterSubOptions = filterAndGroupManager.manager.dataManager.GetFilterSubOptions(filterOptionName);
                List<double> filterSubOptionsDouble = new List<double>(); 
                foreach (var filterSubOption in filterSubOptions) {
                    if((double) filterSubOption > 0)
                    {
                        filterSubOptionsDouble.Add((double)filterSubOption);
                    }
                }

                double minValue = filterSubOptionsDouble.Min(); 
                double maxValue = filterSubOptionsDouble.Max();
                fromToValue.Add(minValue);
                fromToValue.Add(maxValue);
                GameObject filterSubOptionInteger = Instantiate(filterAndGroupManager.filterAndGroupSubOptions.filterSubOptionSliderPrefab, parentContentOfToggle.transform);
                filterSubOptionInteger.GetComponent<CAS_EachFilterAndGroupSubOptionInteger>().SetEachFilterAndGroupSubOptionContent(minValue, maxValue); 
            }
            else if(filterOptionDataType == System.Type.GetType("System.String"))
            {
                List<object> filterSubOptions = filterAndGroupManager.manager.dataManager.GetFilterSubOptions(filterOptionName);
                filterSubOptionsStatus = new Dictionary<string, bool>();
                //filterSubOptionsName = new List<string>();

                foreach (var filterSubOption in filterSubOptions)
                {
                    GameObject filterSubOptionInstantiatedToggle = Instantiate(filterAndGroupManager.filterAndGroupSubOptions.filterSubOptionTogglePrefab, parentContentOfToggle.transform);
                    filterSubOptionInstantiatedToggle.name = filterSubOption.ToString();
                    filterSubOptionInstantiatedToggle.GetComponent<CAS_EachFilterAndGroupSubOption>().SetEachFilterAndGroupSubOptionContent(filterSubOption.ToString());
                    filterSubOptionsStatus.Add(filterSubOption.ToString(), false);
                    //filterSubOptionsName.Add(filterSubOption.ToString()); 
                }

                //filterSubOptionsStatus = new bool[filterSubOptionsName.Count]; 
            }
        }

        public void SetFitlerOptionName(string value)
        {
            filterOptionName = value;
        }

        public void ToggledThisFilterSubOption(string name, bool value)
        {
            Debug.Log(name + " " + value);
            filterSubOptionsStatus[name] = value;
        }

        public void SetFromToSliderValue(int index, float value)
        {
            fromToValue[index] = (double)value; 
        }

        //OnClick filter button - take all the filteroptionstatus values and send query 
        public void OnClickAddThisFilterButton()
        {
            stepNumberToWhichThisFilterIsApplied = filterAndGroupManager.filterStepManager.GetCurrentAndActiveStep();

            if (filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.Double"))
            {
                filterAndGroupManager.AddFilterToActiveStepInteger(gameObject.name, fromToValue);

                addFilterButton.SetActive(false);
                changeFilterButton.SetActive(true);
            }
            else if (filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.String"))
            {
                List<string> filterSubOptionsSelected = new List<string>();

                foreach (string key in filterSubOptionsStatus.Keys)
                {
                    if (filterSubOptionsStatus[key])
                    {
                        filterSubOptionsSelected.Add(key);
                    }
                }

                filterAndGroupManager.AddFilterToActiveStep(gameObject.name, filterSubOptionsSelected);

                addFilterButton.SetActive(false);
                changeFilterButton.SetActive(true);
            }
        }

        //OnClick filter button - take all the filteroptionstatus values and send query 
        public void OnClickChangeThisFilterButton()
        {
            if (filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.Double"))
            {
                filterAndGroupManager.RemoveFilterFromActiveStepInteger(gameObject.name);
                filterAndGroupManager.AddFilterToActiveStepInteger(gameObject.name, fromToValue);
            }
            else if(filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.String"))
            {
                List<string> filterSubOptionsSelected = new List<string>();

                foreach (string key in filterSubOptionsStatus.Keys)
                {
                    if (filterSubOptionsStatus[key])
                    {
                        filterSubOptionsSelected.Add(key);
                    }
                }

                filterAndGroupManager.RemoveFilterFromActiveStep(gameObject.name);
                filterAndGroupManager.AddFilterToActiveStep(gameObject.name, filterSubOptionsSelected);
            }
        }

        public void OnClickRemoveThisFilter()
        {
            stepNumberToWhichThisFilterIsApplied = -1;

            //Should do this only if the step is active. Otherwise the button should be inactive 
            filterAndGroupManager.RemoveFilterFromActiveStep(gameObject.name);

            changeFilterButton.SetActive(false);
            addFilterButton.SetActive(true);

        }

        public void OnClickRemoveThisFilterInteger()
        {
            stepNumberToWhichThisFilterIsApplied = -1;

            //Should do this only if the step is active. Otherwise the button should be inactive 
            filterAndGroupManager.RemoveFilterFromActiveStepInteger(gameObject.name);

            changeFilterButton.SetActive(false);
            addFilterButton.SetActive(true);

        }
    }
}