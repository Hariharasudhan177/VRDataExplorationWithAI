using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;
using TMPro; 

namespace CAS
{
    public class CAS_FilterAndGroupUIStep : MonoBehaviour
    {
        public CAS_FilterAndGroupUIManager filterAndGroupUIManager; 

        CAS_TabGroup tabGroup;
        public CAS_TabButton[] tabButtons;

        public GameObject displayListPrefab;

        [HideInInspector]
        public List<CAS_FilterAndGroupOptionKeyValuesClass> filtersApplied;

        [HideInInspector]
        public List<List<string>> modelsForAllSteps; 

        [HideInInspector]
        public List<CAS_EachFilterAndGroupStep> eachFilterAndGroupStepsAdded;

        [HideInInspector]
        public int currentStep = -1;

        public GameObject filterLayerPagePrefab;
        public GameObject filterLayerButtonPrefab;
        public GameObject filterLayerPageParent;
        public GameObject filterLayerButtonParent;
        public List<GameObject> filterLayerPageList;
        public List<GameObject> filterLayerButtonList;

        public TextMeshProUGUI groupingOptionName;
        public GameObject eachGroupedSetAndDetailsPrefab;
        public GameObject parentContentOfEachGroupedSetAndDetailsPrefab;

        string groupedByKey = "";
        int clustersCount =  4; 
        bool groupingWithFilter = true;

        string sortByKey = "";

        bool filterAndGroupUIVisibilityStatus = true; 

        private void Awake()
        {
            filtersApplied = new List<CAS_FilterAndGroupOptionKeyValuesClass>();
            eachFilterAndGroupStepsAdded = new List<CAS_EachFilterAndGroupStep>();

            filterLayerPageList = new List<GameObject>();
            filterLayerButtonList = new List<GameObject>();

            filterAndGroupUIManager = GetComponentInParent<CAS_FilterAndGroupUIManager>(); 
        }

        // Start is called before the first frame update
        void Start()
        {
            //tabButtons = GetComponentsInChildren<CAS_TabButton>();
            tabGroup = GetComponentInChildren<CAS_TabGroup>();         
        }

        // Update is called once per frame
        void Update()
        {
       
        }

        public int GetCurrentStep()
        {
            return currentStep; 
        }

        public void AddFilter(string filterKey, List<string> filterValuesString,  List<List<double>> filterValuesDouble, bool isString)
        {
            if(filterValuesString.Count > 0 || filterValuesDouble.Count > 0){
                CAS_FilterAndGroupOptionKeyValuesClass filterKeyValuesClass = new CAS_FilterAndGroupOptionKeyValuesClass(filterKey, filterValuesString, filterValuesDouble, isString);
                filtersApplied.Add(filterKeyValuesClass);
            }

            ApplyFilter(filtersApplied); 
        }

        public void ChangeFilter(string filterKey, List<string> filterValuesString, List<List<double>> filterValuesDouble, bool isString)
        {
            //foreach (CAS_FilterKeyValuesClass eachKeyFilterValues in filtersApplied)
            for(int i = 0; i < filtersApplied.Count; i++)
            {
                CAS_FilterAndGroupOptionKeyValuesClass eachKeyFilterValues = filtersApplied[i];
                if (eachKeyFilterValues.GetFilterName() == filterKey)
                {
                    if (isString)   
                    {
                        if (filterValuesString.Count > 0)
                        {
                            eachKeyFilterValues.SetStringValues(filterValuesString);
                        }
                        else
                        {
                            filtersApplied.Remove(eachKeyFilterValues);
                        }
                    }
                    else
                    {
                        if(filterValuesDouble.Count > 0)
                        {
                            eachKeyFilterValues.SetDoubleValues(filterValuesDouble);
                        }
                        else
                        {
                            filtersApplied.Remove(eachKeyFilterValues);
                        }
                    }
                }
            }

            ApplyFilter(filtersApplied); 
        }

        public void ApplyFilter(List<CAS_FilterAndGroupOptionKeyValuesClass> filterKeyValues)
        {
            List<CAS_FilterAndGroupOptionKeyValuesClass> incrementalStepFilterKeyValuesList = new List<CAS_FilterAndGroupOptionKeyValuesClass>();
            modelsForAllSteps = new List<List<string>>();

            foreach (CAS_FilterAndGroupOptionKeyValuesClass eachStepFilterKeyValues in filterKeyValues)
            {
                incrementalStepFilterKeyValuesList.Add(eachStepFilterKeyValues);
                modelsForAllSteps.Add(GetFilteredPatiendIds(incrementalStepFilterKeyValuesList));
            }

            filterAndGroupUIManager.manager.stepManager.SetFilteredModelsToEditLayers(modelsForAllSteps);

            CreateAndEditFilterSteps();

            if (groupedByKey != "")
            {
                ApplyGrouping(groupedByKey, clustersCount);
            }

            if (sortByKey != "")
            {
                ApplySorting(sortByKey);
            }

            filterAndGroupUIManager.manager.aiManager.RefreshAfterFilter();
        }

        public void CreateAndEditFilterSteps()
        {
            int index = -1;

            List<CAS_FilterAndGroupOptionKeyValuesClass> filtersAppliedAtEachStep = new List<CAS_FilterAndGroupOptionKeyValuesClass>();
            foreach (CAS_FilterAndGroupOptionKeyValuesClass filterKeyValuesClass in filtersApplied)
            {
                index++;
                filtersAppliedAtEachStep.Add(filterKeyValuesClass);

                if (index >= eachFilterAndGroupStepsAdded.Count)
                {
                    CAS_EachFilterAndGroupStep eachFilterAndGroupStep = CreateFilterLayer();
                    eachFilterAndGroupStepsAdded.Add(eachFilterAndGroupStep);
                    eachFilterAndGroupStep.SetFilterDisplayContent(filtersAppliedAtEachStep, modelsForAllSteps);
                }
                else
                {
                    eachFilterAndGroupStepsAdded[index].SetFilterDisplayContent(filtersAppliedAtEachStep, modelsForAllSteps);
                }
            }

            //Remove unnecessary steps 
            if (eachFilterAndGroupStepsAdded.Count - index > 1)
            {
                int stepsToDelete = eachFilterAndGroupStepsAdded.Count - index - 1;

                int lastStep = eachFilterAndGroupStepsAdded.Count - 1;


                for (int i = lastStep; i > lastStep - stepsToDelete; i--)
                {
                    DeleteFilterLayer(i);
                }
            }
        }

        public CAS_EachFilterAndGroupStep CreateFilterLayer()
        {
            GameObject filterLayerButton = Instantiate(filterLayerButtonPrefab, filterLayerButtonParent.transform);
            filterLayerButton.name = "Filter Layer Button" + (filterLayerButtonList.Count + 1);
            filterLayerButton.GetComponent<CAS_TabButton>().tabGroup = tabGroup;
            filterLayerButton.GetComponent<CAS_TabButton>().SetButtonText("" + (filterLayerButtonList.Count + 1));
            filterLayerButtonList.Add(filterLayerButton);

            GameObject filterLayerPage = Instantiate(filterLayerPagePrefab, filterLayerPageParent.transform);
            filterLayerPage.name = "Filter Layer Page" + (filterLayerPageList.Count + 1); 
            filterLayerPageList.Add(filterLayerPage);
            tabGroup.SetObjectsToSwap(filterLayerPageList);

            CAS_EachFilterAndGroupStep eachFilterAndGroupStep = filterLayerPage.GetComponent<CAS_EachFilterAndGroupStep>();
            
            return eachFilterAndGroupStep; 
        }

        public void DeleteFilterLayer(int index)
        {
            eachFilterAndGroupStepsAdded.RemoveAt(index); 

            Destroy(filterLayerPageList[index].gameObject); 
            filterLayerPageList.RemoveAt(index);
            tabGroup.SetObjectsToSwap(filterLayerPageList);

            tabGroup.UnSubcribe(filterLayerButtonList[index].gameObject.GetComponent<CAS_TabButton>()); 
            Destroy(filterLayerButtonList[index].gameObject);
            filterLayerButtonList.RemoveAt(index);
        }

        //Grouping 
        public void ApplyGrouping(string filterKey, int count)
        {
            groupedByKey = filterKey;
            sortByKey = ""; 

            clustersCount = count; 
            //filterAndGroupUIManager.manager.stepManager.SetGroupByModelsToEditLayers(GetGroupedByPatiendIds(filtersApplied, filterKey));
            //For now removing the filtering from grouping. Use the above code if needed later
            List<CAS_FilterAndGroupOptionKeyValuesClass> filterForGrouping = new List<CAS_FilterAndGroupOptionKeyValuesClass>();

            if (groupingWithFilter)
            {
                filterForGrouping = filtersApplied; 
            }

            Dictionary<string, List<string>> groupedByPatientIds = GetGroupedByPatiendIds(filterForGrouping, filterKey, clustersCount); 

            //Set group colours to models 
            filterAndGroupUIManager.manager.stepManager.SetGroupByModelsToEditLayers(groupedByPatientIds);
            //Set group details to UI
            SetGroupingDetails(groupedByPatientIds); 

            if(filterAndGroupUIManager.manager.dataManager.GetColumnType(filterKey) == System.Type.GetType("System.String"))
            {
                groupingOptionName.text = "Grouped by " + filterKey;
            }
            else if (filterAndGroupUIManager.manager.dataManager.GetColumnType(filterKey) == System.Type.GetType("System.Double"))
            {
                groupingOptionName.text = "Clustered by " + filterKey;
            }
        }

        public void ApplySorting(string sortKey)
        {
            if (filterAndGroupUIManager.manager.aiManager.visualisationOn) return; 
            sortByKey = sortKey;
            groupedByKey = ""; 

            SortByStructure sorted = filterAndGroupUIManager.manager.dataManager.GetSortedBy(sortByKey);

            //Set group colours to models 
            filterAndGroupUIManager.manager.stepManager.SetSortByModelsToEditLayers(sorted, groupingWithFilter);
            SetSortingDetails(sorted);

            groupingOptionName.text = "Sorted by " + sortKey;
        }

        public void RemoveGrouping()
        {
            groupedByKey = "";
            sortByKey = "";
            filterAndGroupUIManager.manager.stepManager.RemoveGroupByModelsToEditLayers();
            SetGroupingDetails(new Dictionary<string, List<string>>()); 
            groupingOptionName.text = "";

            filterAndGroupUIManager.manager.aiManager.RefreshAfterGroupingRemoved();
        }

        public void OnClickGroupingWithOrWithoutFilterSwitch(float value)
        {
            if(value == 0)
            {
                groupingWithFilter = true; 
            }
            else
            {
                groupingWithFilter = false; 
            }

            if (groupedByKey != "")
            {
                ApplyGrouping(groupedByKey, clustersCount);
            }

            if(sortByKey != "")
            {
                ApplySorting(sortByKey);
            }
        }

        public void SetGroupingDetails(Dictionary<string, List<string>> groupedByPatientIds)
        {
            int index = 0;

            CAS_EachGroupedSetAndDetails[] toDestroy = parentContentOfEachGroupedSetAndDetailsPrefab.GetComponentsInChildren<CAS_EachGroupedSetAndDetails>(); 
            foreach (CAS_EachGroupedSetAndDetails child in toDestroy)
            {
                Destroy(child.gameObject); 
            }

            foreach (string key in groupedByPatientIds.Keys)
            {
                GameObject eachGroupedSetAndDetailsObject = Instantiate(eachGroupedSetAndDetailsPrefab, parentContentOfEachGroupedSetAndDetailsPrefab.transform);
                eachGroupedSetAndDetailsObject.GetComponent<CAS_EachGroupedSetAndDetails>().SetGroupedDetailsContent(filterAndGroupUIManager.manager.dataManager.colorsForGrouping[index], key, groupedByPatientIds[key].Count);
                index++;
            }
        }

        public void SetSortingDetails(CAS_FilterAndGroupUIStep.SortByStructure sorted)
        {
            if (!sorted.isString) return; 


            CAS_EachGroupedSetAndDetails[] toDestroy = parentContentOfEachGroupedSetAndDetailsPrefab.GetComponentsInChildren<CAS_EachGroupedSetAndDetails>();
            foreach (CAS_EachGroupedSetAndDetails child in toDestroy)
            {
                Destroy(child.gameObject);
            }

            string previousKey = sorted.stringValues[0];
            string currentKey = sorted.stringValues[0];

            int index = 0;
            int stringIndex = 0; 
            int keyIndex = 0; 
            foreach (string patientId in sorted.patientIds)
            {
                Debug.Log(sorted.stringValues[index]);
                Debug.Log(stringIndex); 

                //if (sorted.isString)
                //{
                    previousKey = currentKey;
                    currentKey = sorted.stringValues[index];

                    if (previousKey != currentKey)
                    {
                        GameObject eachGroupedSetAndDetailsObject = Instantiate(eachGroupedSetAndDetailsPrefab, parentContentOfEachGroupedSetAndDetailsPrefab.transform);
                        eachGroupedSetAndDetailsObject.GetComponent<CAS_EachGroupedSetAndDetails>().SetGroupedDetailsContent(filterAndGroupUIManager.manager.dataManager.colorsForGrouping[keyIndex], previousKey, stringIndex);
                        stringIndex = 0; 
                        keyIndex++;
                    }
                //}

                stringIndex++;
                index++;

                if (index == sorted.patientIds.Count)
                {
                    if (previousKey == currentKey)
                    {
                        GameObject eachGroupedSetAndDetailsObject = Instantiate(eachGroupedSetAndDetailsPrefab, parentContentOfEachGroupedSetAndDetailsPrefab.transform);
                        eachGroupedSetAndDetailsObject.GetComponent<CAS_EachGroupedSetAndDetails>().SetGroupedDetailsContent(filterAndGroupUIManager.manager.dataManager.colorsForGrouping[keyIndex], previousKey, stringIndex);
                    }
                }
            }
        }

        public void OpenClose()
        {
            filterAndGroupUIVisibilityStatus = !filterAndGroupUIVisibilityStatus; 

            if (filterAndGroupUIVisibilityStatus)
            {
                GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0; 
            }

            GetComponent<CanvasGroup>().interactable = filterAndGroupUIVisibilityStatus;
            GetComponent<TrackedDeviceGraphicRaycaster>().enabled = filterAndGroupUIVisibilityStatus;
        }

        public List<string> GetFilteredPatiendIds(List<CAS_FilterAndGroupOptionKeyValuesClass> filterKeyValues)
        {
            var keyValuePairs = GetKeyValuePairs(filterKeyValues);

            return filterAndGroupUIManager.manager.dataManager.GetFilteredPatientIdsStringAndInteger(keyValuePairs.Item1, keyValuePairs.Item2, keyValuePairs.Item3, keyValuePairs.Item4);
        }

        public Dictionary<string, List<string>> GetGroupedByPatiendIds(List<CAS_FilterAndGroupOptionKeyValuesClass> filterKeyValues, string filterKey, int clustersCount)
        {
            var keyValuePairs = GetKeyValuePairs(filterKeyValues); 

            return filterAndGroupUIManager.manager.dataManager.GetPatientIdsGroupBy(keyValuePairs.Item1, keyValuePairs.Item2, keyValuePairs.Item3, keyValuePairs.Item4, filterKey, clustersCount);
        }

        public (List<string>, List<List<string>>, List<string>, List<List<List<double>>>) GetKeyValuePairs(List<CAS_FilterAndGroupOptionKeyValuesClass> filterKeyValues)
        {
            List<string> stringFilterKeys = new List<string>();
            List<List<string>> stringFilterValues = new List<List<string>>();
            List<string> doubleFilterKeys = new List<string>();
            List<List<List<double>>> doubleFilterValues = new List<List<List<double>>>();

            foreach (CAS_FilterAndGroupOptionKeyValuesClass filterKeyValue in filterKeyValues)
            {
                if (filterKeyValue.GetIsString())
                {
                    stringFilterKeys.Add(filterKeyValue.GetFilterName());
                    stringFilterValues.Add(filterKeyValue.GetStringValues());
                }
                else
                {
                    doubleFilterKeys.Add(filterKeyValue.GetFilterName());
                    doubleFilterValues.Add(filterKeyValue.GetDoubleValues());
                }
            }

            return (stringFilterKeys, stringFilterValues, doubleFilterKeys, doubleFilterValues); 
        }

        public struct SortByStructure
        {
            public bool isString;
            public List<string> patientIds;
            public List<string> stringValues;
            public List<double> doubleValues;
            public double min;
            public double max; 
        }
    }
}

