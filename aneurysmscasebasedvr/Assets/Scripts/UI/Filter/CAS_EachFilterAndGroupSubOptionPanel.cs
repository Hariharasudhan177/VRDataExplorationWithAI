using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro; 

namespace CAS
{
    public class CAS_EachFilterAndGroupSubOptionPanel : MonoBehaviour
    {
        public CAS_FilterAndGroupUIManager filterAndGroupManager;

        [HideInInspector]
        public string filterOptionName;

        bool filterApplied = false;

        //For string filters 
        [HideInInspector]
        public Dictionary<string, CAS_FilterSubOptionStringDetailClass> filterSubOptionsStatus;

        //For double filters 
        List<double> fromToValueOriginal;
        List<double> fromToValue;
        CAS_EachFilterAndGroupSubOptionInteger filterAndGroupSubOptionInteger; 

        public GameObject parentContentOfSubOption;

        public GameObject applyFilterButton;
        public GameObject changeFilterButton;

        [HideInInspector]
        public bool initialStatus;

        public TextMeshProUGUI filterOptionNameTextBox;
        public Image filterOptionStatus;

        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupManager = GetComponentInParent<CAS_FilterAndGroupUIManager>();
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
            filterOptionNameTextBox.text = filterOptionName; 

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

                GameObject filterSubOptionInteger = Instantiate(filterAndGroupManager.subOptionsUI.filterSubOptionSliderPrefab, parentContentOfSubOption.transform);
                filterSubOptionInteger.GetComponent<CAS_EachFilterAndGroupSubOptionInteger>().SetEachFilterAndGroupSubOptionContent(minValue, maxValue);
                filterAndGroupSubOptionInteger = filterSubOptionInteger.GetComponent<CAS_EachFilterAndGroupSubOptionInteger>(); 
            }
            else if(filterOptionDataType == System.Type.GetType("System.String"))
            {
                List<object> filterSubOptions = filterAndGroupManager.manager.dataManager.GetFilterSubOptions(filterOptionName);
                filterSubOptionsStatus = new Dictionary<string, CAS_FilterSubOptionStringDetailClass>();

                foreach (var filterSubOption in filterSubOptions)
                {
                    GameObject filterSubOptionInstantiatedToggle = Instantiate(filterAndGroupManager.subOptionsUI.filterSubOptionTogglePrefab, parentContentOfSubOption.transform);
                    filterSubOptionInstantiatedToggle.name = filterSubOption.ToString();
                    CAS_EachFilterAndGroupSubOptionString filterAndGroupSubOptionString = filterSubOptionInstantiatedToggle.GetComponent<CAS_EachFilterAndGroupSubOptionString>();
                    filterAndGroupSubOptionString.SetEachFilterAndGroupSubOptionContent(filterSubOption.ToString());
                    CAS_FilterSubOptionStringDetailClass filterSubOptionDetailClass = new CAS_FilterSubOptionStringDetailClass(filterSubOption.ToString(), true, filterAndGroupSubOptionString, false); 
                    filterSubOptionsStatus.Add(filterSubOption.ToString(), filterSubOptionDetailClass);
                }
            }
        }

        public void SetFitlerOptionName(string value)
        {
            filterOptionName = value;
        }

        public void ToggledThisFilterSubOption(string name, bool value)
        {
            filterSubOptionsStatus[name].SetToggleStatus(value); 
        }

        public void SetFromToSliderValue(int index, float value)
        {
            fromToValue[index] = (double)value; 
        }

        //OnClick filter button - take all the filteroptionstatus values and send query 
        public void ApplyThisFilter()
        {
            bool blankFilter = false; 

            if (filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.Double"))
            {
                List<double> fromToValueSelected = new List<double>(); 

                if ((fromToValue[0] != fromToValueOriginal[0]) || (fromToValue[1] != fromToValueOriginal[1]))
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
                    if (filterSubOptionsStatus[key].GetToggleStatus())
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
                applyFilterButton.SetActive(true);
                changeFilterButton.SetActive(false);
            }
            else
            {
                applyFilterButton.SetActive(false);
                changeFilterButton.SetActive(true);
                filterApplied = true;
            }

            if (filterApplied)
            {
                filterOptionStatus.color = Color.green; 
            }
            else
            {
                filterOptionStatus.color = Color.red;
            }
        }

        public void RemoveFilter()
        {
            if (filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.Double"))
            {
                fromToValue[0] = fromToValueOriginal[0];
                fromToValue[1] = fromToValueOriginal[1];
                filterAndGroupSubOptionInteger.SetOriginalValues(); 
            }
            else if (filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.String"))
            {
                foreach (string key in filterSubOptionsStatus.Keys)
                {
                    filterSubOptionsStatus[key].SetToggleStatus(false); 
                    filterSubOptionsStatus[key].GetToggle().UncheckToggle();
                }
            }

            ApplyThisFilter();
        }

        public void ApplyGrouping()
        {
            //To Remove existing grouping 
            RemoveGrouping(); 

            filterAndGroupManager.ApplyGrouping(gameObject.name);
        }

        public void RemoveGrouping()
        {
            filterAndGroupManager.RemoveGrouping();
        }


    }
}