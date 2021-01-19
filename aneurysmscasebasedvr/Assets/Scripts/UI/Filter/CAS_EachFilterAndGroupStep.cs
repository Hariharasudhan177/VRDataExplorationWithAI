using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 

namespace CAS
{
    public class CAS_EachFilterAndGroupStep : MonoBehaviour
    {
        CAS_FilterAndGroupUIManager filterAndGroupUIManager;

        public Transform filterDisplayListParent;
 
        GameObject displayListGameObject;

        void Awake()
        {
            filterAndGroupUIManager = GetComponentInParent<CAS_FilterAndGroupUIManager>(); 
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDisplayContent(List<CAS_FilterAndGroupOptionKeyValuesClass> filtersApplied)
        {
            string filterOptionHeadingText = "";
            string filterOptionValuesText = "";

            CAS_FilterAndGroupOptionKeyValuesClass filterKeyValuesClass = filtersApplied[filtersApplied.Count-1];
            filterOptionHeadingText += filterKeyValuesClass.GetFilterName();

            if (filterKeyValuesClass.GetIsString())
            {
                foreach (string filterValue in filterKeyValuesClass.GetStringValues())
                {
                    filterOptionValuesText += filterValue + "\n";
                }
            }
            else
            {
                int index = 0;
                foreach (double filterValue in filterKeyValuesClass.GetDoubleValues())
                {
                    if (index == 0)
                    {
                        if(filterKeyValuesClass.GetFilterName() == "age")
                        {
                            filterOptionValuesText += "Between " + ((int)Mathf.Ceil((float)filterValue)).ToString() + " and ";
                        }
                        else
                        {
                            filterOptionValuesText += "Between " + filterValue + " and ";
                        }
                    }
                    else
                    {
                        if (filterKeyValuesClass.GetFilterName() == "age")
                        {
                            filterOptionValuesText += ((int)Mathf.Ceil((float)filterValue)).ToString();
                        }
                        else
                        {
                            filterOptionValuesText += filterValue;
                        }
                    }
                    index++;
                }
            }

            if (!displayListGameObject)
            {
                displayListGameObject = Instantiate(filterAndGroupUIManager.stepUI.displayListPrefab, filterDisplayListParent);
            }
 
            displayListGameObject.GetComponent<CAS_FilterDisplayList>().SetDisplayContent(filterOptionHeadingText, filterOptionValuesText); 
        }

        public void ApplyGroupbyThisStep(string filterKey)
        {
            /*stepGrouped = false; 

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

            filterAndGroupManager.manager.stepManager.GroupByStep(filteredPatientIdsGroupBy, stepNumber - 1);*/
        }
    }
}

