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

        public void OpenCloseFilterAndGroupUI()
        {
            filterAndGroupUiVisibilityStatus = !filterAndGroupUiVisibilityStatus;

            if (filterAndGroupUiVisibilityStatus)
            {
                filterStepManager.GetComponent<CanvasGroup>().alpha = 1;
                filterAndGroupOptions.GetComponent<CanvasGroup>().alpha = 1;
                filterAndGroupSubOptions.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                filterStepManager.GetComponent<CanvasGroup>().alpha = 0;
                filterAndGroupOptions.GetComponent<CanvasGroup>().alpha = 0;
                filterAndGroupSubOptions.GetComponent<CanvasGroup>().alpha = 0;
            }

            filterStepManager.GetComponent<CanvasGroup>().interactable = filterAndGroupUiVisibilityStatus;
            filterAndGroupOptions.GetComponent<CanvasGroup>().interactable = filterAndGroupUiVisibilityStatus;
            filterAndGroupSubOptions.GetComponent<CanvasGroup>().interactable = filterAndGroupUiVisibilityStatus;

            filterStepManager.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = filterAndGroupUiVisibilityStatus;
            filterAndGroupOptions.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = filterAndGroupUiVisibilityStatus;
            filterAndGroupSubOptions.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = filterAndGroupUiVisibilityStatus;

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

        public void AddFilterToActiveStep(string filterKey, List<string> filterOptionsSelected)
        {
            filterStepManager.AddFilterToActiveStep(filterKey, filterOptionsSelected);
        }

        public void AddFilterToActiveStepInteger(string filterKey, List<double> filterOptionsSelectedInteger)
        {
            filterStepManager.AddFilterToActiveStepInteger(filterKey, filterOptionsSelectedInteger);
        }

        public void RemoveFilterFromActiveStep(string filterKey)
        {
            filterStepManager.RemoveFilterFromActiveStep(filterKey);
        }

        public void RemoveFilterFromActiveStepInteger(string filterKey)
        {
            filterStepManager.RemoveFilterFromActiveStepInteger(filterKey);
        }
    }
}