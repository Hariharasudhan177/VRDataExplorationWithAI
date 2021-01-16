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

        bool filterApplied = false;
        //int stepNumberToWhichThisFilterIsApplied = -1; 

        public GameObject addFilterButton;
        public GameObject changeFilterButton;

        List<double> fromToValueOriginal;
        List<double> fromToValue;

        [HideInInspector]
        public bool initialStatus; 


        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupManager = GetComponentInParent<CAS_FilterAndGroupManager>();
            PopulateFilterOptions();

            if (!initialStatus)
            {
                gameObject.SetActive(false); 
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulateFilterOptions()
        {
            Type filterOptionDataType = filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName);

            if (filterOptionDataType == System.Type.GetType("System.Double"))
            {
                fromToValue = new List<double>();
                fromToValueOriginal = new List<double>();

                List<object> filterSubOptions = filterAndGroupManager.manager.dataManager.GetFilterSubOptions(filterOptionName);
                List<double> filterSubOptionsDouble = new List<double>();
                foreach (var filterSubOption in filterSubOptions) {
                    if ((double) filterSubOption > 0)
                    {
                        filterSubOptionsDouble.Add((double)filterSubOption);
                    }
                }

                double minValue = filterSubOptionsDouble.Min(); 
                double maxValue = filterSubOptionsDouble.Max();
                fromToValue.Add(minValue);
                fromToValue.Add(maxValue);

                fromToValueOriginal.Add(minValue);
                fromToValueOriginal.Add(maxValue);

                GameObject filterSubOptionInteger = Instantiate(filterAndGroupManager.filterAndGroupSubOptions.filterSubOptionSliderPrefab, parentContentOfToggle.transform);
                filterSubOptionInteger.GetComponent<CAS_EachFilterAndGroupSubOptionInteger>().SetEachFilterAndGroupSubOptionContent(minValue, maxValue); 
            }
            else if(filterOptionDataType == System.Type.GetType("System.String"))
            {
                List<object> filterSubOptions = filterAndGroupManager.manager.dataManager.GetFilterSubOptions(filterOptionName);
                filterSubOptionsStatus = new Dictionary<string, bool>();

                foreach (var filterSubOption in filterSubOptions)
                {
                    GameObject filterSubOptionInstantiatedToggle = Instantiate(filterAndGroupManager.filterAndGroupSubOptions.filterSubOptionTogglePrefab, parentContentOfToggle.transform);
                    filterSubOptionInstantiatedToggle.name = filterSubOption.ToString();
                    filterSubOptionInstantiatedToggle.GetComponent<CAS_EachFilterAndGroupSubOption>().SetEachFilterAndGroupSubOptionContent(filterSubOption.ToString());
                    filterSubOptionsStatus.Add(filterSubOption.ToString(), false);
                }
            }
        }

        public void SetFitlerOptionName(string value)
        {
            filterOptionName = value;
        }

        public void ToggledThisFilterSubOption(string name, bool value)
        {
            filterSubOptionsStatus[name] = value;
        }

        public void SetFromToSliderValue(int index, float value)
        {
            fromToValue[index] = (double)value; 
        }

        //OnClick filter button - take all the filteroptionstatus values and send query 
        public void OnClickApplyThisFilterButton()
        {
            bool blankFilter = false; 

            if (filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.Double"))
            {
                List<double> fromToValueSelected = new List<double>(); 

                if ((fromToValue[0] != fromToValueOriginal[0]) && (fromToValue[1] != fromToValueOriginal[1]))
                {
                    fromToValueSelected = fromToValue; 
                }
                else 
                {
                    blankFilter = true; 
                }

                Debug.Log(fromToValueSelected.Count); 
                if (filterApplied)
                {
                    filterAndGroupManager.ChangeFilter(gameObject.name, new List<string>(), fromToValueSelected, false);
                }
                else
                {
                    filterAndGroupManager.AddFilter(gameObject.name, new List<string>(), fromToValueSelected, false);
                }
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

                if(filterSubOptionsSelected.Count == 0)
                {
                    blankFilter = true;
                }

                if (filterApplied)
                {
                    filterAndGroupManager.ChangeFilter(gameObject.name, filterSubOptionsSelected, new List<double>(), true);
                }
                else
                {
                    filterAndGroupManager.AddFilter(gameObject.name, filterSubOptionsSelected, new List<double>(), true);
                }
            }

            if (blankFilter)
            {
                filterApplied = false;
                addFilterButton.SetActive(true);
                changeFilterButton.SetActive(false);
            }
            else
            {
                addFilterButton.SetActive(false);
                changeFilterButton.SetActive(true);
                filterApplied = true;
            }
        }

        public void OnClickApplyGrouping()
        {
            filterAndGroupManager.AddGroupByToActiveStepInteger(gameObject.name);
        }
    }
}