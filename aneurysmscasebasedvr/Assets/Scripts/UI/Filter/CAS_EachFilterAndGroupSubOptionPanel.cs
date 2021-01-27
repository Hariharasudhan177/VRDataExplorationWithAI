﻿using System.Collections;
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
        List<List<double>> fromToValue;

        public GameObject parentContentOfSubOption;

        public GameObject applyFilterButton;
        public GameObject changeFilterButton;

        [HideInInspector]
        public bool initialStatus;

        public TextMeshProUGUI filterOptionNameTextBox;
        public Image filterOptionStatus;
        public Button addIntegerOptionButton;

        List<CAS_EachFilterAndGroupSubOptionInteger> filterAndGroupSubOptionIntegerList; 

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
                filterAndGroupSubOptionIntegerList = new List<CAS_EachFilterAndGroupSubOptionInteger>();
                fromToValue = new List<List<double>>();
                fromToValue.Add(new List<double>()); 
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
                fromToValue[0].Add(minValue);
                fromToValue[0].Add(maxValue);

                fromToValueOriginal.Add(minValue);
                fromToValueOriginal.Add(maxValue);

                GameObject filterSubOptionInteger = Instantiate(filterAndGroupManager.subOptionsUI.filterSubOptionSliderPrefab, parentContentOfSubOption.transform);
                CAS_EachFilterAndGroupSubOptionInteger filterAndGroupSubOptionInteger = filterSubOptionInteger.GetComponent<CAS_EachFilterAndGroupSubOptionInteger>();
                filterAndGroupSubOptionIntegerList.Add(filterAndGroupSubOptionInteger);
                filterAndGroupSubOptionInteger.SetEachFilterAndGroupSubOptionContent(minValue, maxValue, filterAndGroupSubOptionIntegerList.Count-1);

                addIntegerOptionButton.gameObject.SetActive(true);

                parentContentOfSubOption.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
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

        public void SetFromToSliderValue(int index, float value, int subOptionIntegerIndex)
        {
            fromToValue[subOptionIntegerIndex][index] = (double)value; 
        }

        //OnClick filter button - take all the filteroptionstatus values and send query 
        public void ApplyThisFilter()
        {
            bool blankFilter = true; 

            if (filterAndGroupManager.manager.dataManager.GetColumnType(filterOptionName) == System.Type.GetType("System.Double"))
            {

                List<List<double>> fromToValueSelected = new List<List<double>>(); 

                foreach(List<double> subFromToValue in fromToValue)
                {
                    if ((subFromToValue[0] != fromToValueOriginal[0]) || (subFromToValue[1] != fromToValueOriginal[1]))
                    {
                        fromToValueSelected.Add(subFromToValue);
                        blankFilter = false;
                    }
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
                        blankFilter = false;
                    }
                }

                if (filterApplied)
                {
                    filterAndGroupManager.ChangeFilter(gameObject.name, filterSubOptionsSelected, new List<List<double>>(), true);
                }
                else
                {
                    filterAndGroupManager.AddFilter(gameObject.name, filterSubOptionsSelected, new List<List<double>>(), true);
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
                foreach (List<double> subFromToValue in fromToValue)
                {
                    subFromToValue[0] = fromToValueOriginal[0];
                    subFromToValue[1] = fromToValueOriginal[1];
                }

                foreach (CAS_EachFilterAndGroupSubOptionInteger filterAndGroupSubOptionInteger in filterAndGroupSubOptionIntegerList)
                {
                    filterAndGroupSubOptionInteger.SetOriginalValues();
                }
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

        public void AddAnotherSubOptionInteger()
        {
            GameObject filterSubOptionInteger = Instantiate(filterAndGroupManager.subOptionsUI.filterSubOptionSliderPrefab, parentContentOfSubOption.transform);
            CAS_EachFilterAndGroupSubOptionInteger filterAndGroupSubOptionInteger = filterSubOptionInteger.GetComponent<CAS_EachFilterAndGroupSubOptionInteger>();
            filterAndGroupSubOptionIntegerList.Add(filterAndGroupSubOptionInteger);
            filterAndGroupSubOptionInteger.SetEachFilterAndGroupSubOptionContent(fromToValueOriginal[0], fromToValueOriginal[1], filterAndGroupSubOptionIntegerList.Count-1);

            fromToValue.Add(new List<double>());
            fromToValue[fromToValue.Count - 1].Add(fromToValueOriginal[0]);
            fromToValue[fromToValue.Count - 1].Add(fromToValueOriginal[1]);
        }
    }
}