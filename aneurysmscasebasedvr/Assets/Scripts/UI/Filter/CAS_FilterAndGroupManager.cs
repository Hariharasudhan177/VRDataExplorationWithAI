using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_FilterAndGroupManager : MonoBehaviour
    {
        public CAS_Manager manager;

        public CAS_FilterStepManager filterAndGroupManager; 
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
    }
}