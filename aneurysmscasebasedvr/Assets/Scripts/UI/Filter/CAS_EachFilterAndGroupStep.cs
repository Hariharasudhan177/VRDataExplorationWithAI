using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; 

namespace CAS
{
    public class CAS_EachFilterAndGroupStep : MonoBehaviour
    {
        CAS_FilterAndGroupManager filterAndGroupManager;

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
            filterAndGroupManager = GetComponentInParent<CAS_FilterAndGroupManager>(); 
        }
        // Start is called before the first frame update
        void Start()
        {
            //filterAndGroupManager = GetComponentInParent<CAS_FilterAndGroupManager>(); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDisplayContent(List<CAS_FilterKeyValuesClass> filtersApplied)
        {
            string displayList = "";
            foreach(CAS_FilterKeyValuesClass filterKeyValuesClass in filtersApplied)
            {
                displayList += filterKeyValuesClass.GetFilterName(); 
            }

            if(!displayListGameObject)
            {
                displayListGameObject = Instantiate(filterAndGroupManager.filterStepManager.displayListPrefab, filterDisplayListParent);
            }
 
            displayListGameObject.GetComponent<CAS_EachDisplayList>().SetDisplayContent(displayList); 
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

