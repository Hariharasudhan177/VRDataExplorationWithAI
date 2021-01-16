using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_FilterAndGroupManager : MonoBehaviour
    {
        public CAS_Manager manager;

        public CAS_FilterStepManager filterStepManager; 
        public CAS_FilterAndGroupOptions filterAndGroupOptions;
        public CAS_FilterAndGroupSubOptions filterAndGroupSubOptions;

        bool filterAndGroupUiVisibilityStatus = false;

        Dictionary<string, List<string>> optionsUnderSpecificTypes;

        void Awake()
        {
            optionsUnderSpecificTypes = new Dictionary<string, List<string>>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenClose()
        {
            filterAndGroupUiVisibilityStatus = !filterAndGroupUiVisibilityStatus;

            filterStepManager.OpenClose(filterAndGroupUiVisibilityStatus);
            filterAndGroupOptions.OpenClose(filterAndGroupUiVisibilityStatus);
            filterAndGroupSubOptions.OpenClose(filterAndGroupUiVisibilityStatus);
        }

        public void PopulateFilterOptions()
        {
            filterAndGroupOptions.PopulateFilterOptions();
            filterAndGroupSubOptions.PopulateFilterOptions();
        }

        public void SetOptionsUnderSpecificTypes(Dictionary<string, List<string>> value)
        {
            optionsUnderSpecificTypes = value; 
        }

        public Dictionary<string, List<string>> GetOptionsUnderSpecificTypes()
        {
            return optionsUnderSpecificTypes;
        }

        public void AddGroupByToActiveStepInteger(string filterKey)
        {
            //filterStepManager.AddGroupByToActiveStepInteger(filterKey);
        }

        public void SetFilterOptionSelected(string key)
        {
            filterAndGroupSubOptions.SetfilterSubOptionPanelsSelected(key); 
        }


        public void AddFilter(string filterKey, List<string> filterValuesString, List<double> filterValuesDouble, bool isString)
        {
            filterStepManager.AddFilter(filterKey, filterValuesString, filterValuesDouble, isString); 
        }

        public void ChangeFilter(string filterKey, List<string> filterValuesString, List<double> filterValuesDouble, bool isString)
        {
            filterStepManager.ChangeFilter(filterKey, filterValuesString, filterValuesDouble, isString);
        }
    }
}