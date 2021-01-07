using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

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

            if(filterOptionDataType == System.Type.GetType("System.Double"))
            {
                return;
            }

            List<object> filterSubOptions = filterAndGroupManager.manager.dataManager.GetFilterSubOptions(filterOptionName);
            Debug.Log(filterSubOptions.Count);
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

        public void SetFitlerOptionName(string value)
        {
            filterOptionName = value;
        }
    }
}