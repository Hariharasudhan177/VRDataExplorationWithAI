using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_FilterAndGroupUIManager : MonoBehaviour
    {
        public CAS_Manager manager;

        public CAS_FilterAndGroupUIOptions optionsUI;
        public CAS_FilterAndGroupUISubOptions subOptionsUI;
        public CAS_FilterAndGroupUIStep stepUI;

        bool filterAndGroupUIVisibilityStatus = true;

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
            filterAndGroupUIVisibilityStatus = !filterAndGroupUIVisibilityStatus;

            //stepUI.OpenClose(filterAndGroupUIVisibilityStatus);
            optionsUI.OpenClose(filterAndGroupUIVisibilityStatus);
            subOptionsUI.OpenClose(filterAndGroupUIVisibilityStatus);
        }

        public void PopulateFilterOptions()
        {
            optionsUI.PopulateFilterOptions();
            subOptionsUI.PopulateFilterOptions();
        }

        public void SetOptionsUnderSpecificTypes(Dictionary<string, List<string>> value)
        {
            optionsUnderSpecificTypes = value; 
        }

        public Dictionary<string, List<string>> GetOptionsUnderSpecificTypes()
        {
            return optionsUnderSpecificTypes;
        }

        public void SetFilterOptionSelected(string key)
        {
            subOptionsUI.SetfilterSubOptionPanelsSelected(key); 
        }

        public void AddFilter(string filterKey, List<string> filterValuesString, List<List<double>> filterValuesDouble, bool isString)
        {
            stepUI.AddFilter(filterKey, filterValuesString, filterValuesDouble, isString); 
        }

        public void ChangeFilter(string filterKey, List<string> filterValuesString, List<List<double>> filterValuesDouble, bool isString)
        {
            stepUI.ChangeFilter(filterKey, filterValuesString, filterValuesDouble, isString);
        }

        public void ApplyGrouping(string filterKey)
        {
            stepUI.ApplyGrouping(filterKey);
        }

        public void RemoveGrouping()
        {
            stepUI.RemoveGrouping();
        }
    }
}