using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_FilterStepManager : MonoBehaviour
    {
        public CAS_FilterAndGroupManager filterAndGroupManager; 

        CAS_TabGroup tabGroup;

        public CAS_TabButton[] tabButtons;

        public GameObject displayListPrefab;

        [HideInInspector]
        public List<CAS_FilterKeyValuesClass> filtersApplied;

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
        private void Awake()
        {
            filtersApplied = new List<CAS_FilterKeyValuesClass>();
            eachFilterAndGroupStepsAdded = new List<CAS_EachFilterAndGroupStep>();

            filterLayerPageList = new List<GameObject>();
            filterLayerButtonList = new List<GameObject>();

            filterAndGroupManager = GetComponentInParent<CAS_FilterAndGroupManager>(); 
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

        public void AddFilter(string filterKey, List<string> filterValuesString,  List<double> filterValuesDouble, bool isString)
        {
            CAS_FilterKeyValuesClass filterKeyValuesClass = new CAS_FilterKeyValuesClass(filterKey, filterValuesString, filterValuesDouble, isString);
            filtersApplied.Add(filterKeyValuesClass);

            ApplyFilter(filtersApplied); 
        }

        public void ChangeFilter(string filterKey, List<string> filterValuesString, List<double> filterValuesDouble, bool isString)
        {
            //foreach (CAS_FilterKeyValuesClass eachKeyFilterValues in filtersApplied)
            for(int i = 0; i < filtersApplied.Count; i++)
            {
                CAS_FilterKeyValuesClass eachKeyFilterValues = filtersApplied[i];
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

        public CAS_EachFilterAndGroupStep CreateFilterLayer()
        {
            GameObject filterLayerPage = Instantiate(filterLayerPagePrefab, filterLayerPageParent.transform);
            filterLayerPageList.Add(filterLayerPage);
            tabGroup.SetObjectsToSwap(filterLayerPageList);

            GameObject filterLayerButton = Instantiate(filterLayerButtonPrefab, filterLayerButtonParent.transform);
            filterLayerButton.GetComponent<CAS_TabButton>().tabGroup = tabGroup;
            filterLayerButtonList.Add(filterLayerButton);

            CAS_EachFilterAndGroupStep eachFilterAndGroupStep = filterLayerPage.GetComponent<CAS_EachFilterAndGroupStep>();
            
            return eachFilterAndGroupStep; 
        }

        public void DeleteFilterLayer(int index)
        {
            Destroy(filterLayerPageList[index].gameObject); 
            filterLayerPageList.RemoveAt(index);
            tabGroup.SetObjectsToSwap(filterLayerPageList);

            tabGroup.UnSubcribe(filterLayerButtonList[index].gameObject.GetComponent<CAS_TabButton>()); 
            Destroy(filterLayerButtonList[index].gameObject);
            filterLayerButtonList.RemoveAt(index);

        }

        public void OpenClose(bool status)
        {
            if (status)
            {
                GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0; 
            }

            GetComponent<CanvasGroup>().interactable = status;
            GetComponent<TrackedDeviceGraphicRaycaster>().enabled = status;
        }

        public void ApplyFilter(List<CAS_FilterKeyValuesClass> filterKeyValues)
        {
            List<CAS_FilterKeyValuesClass> incrementalStepFilterKeyValuesList = new List<CAS_FilterKeyValuesClass>();
            List<List<string>> modeldsForThisStep = new List<List<string>>();

            foreach (CAS_FilterKeyValuesClass eachStepFilterKeyValues in filterKeyValues)
            {
                incrementalStepFilterKeyValuesList.Add(eachStepFilterKeyValues);
                modeldsForThisStep.Add(GetFilteredPatiendIds(incrementalStepFilterKeyValuesList));
            }

            filterAndGroupManager.manager.stepManager.SetFilteredModelsToEditLayers(modeldsForThisStep);

            CreateAndEditFilterSteps(); 
        }


        public void CreateAndEditFilterSteps()
        {
            int index = -1;

            List<CAS_FilterKeyValuesClass> filtersAppliedAtEachStep = new List<CAS_FilterKeyValuesClass>(); 
            foreach (CAS_FilterKeyValuesClass filterKeyValuesClass in filtersApplied)
            {
                index++; 
                filtersAppliedAtEachStep.Add(filterKeyValuesClass); 

                if (index >= eachFilterAndGroupStepsAdded.Count)
                {
                    CAS_EachFilterAndGroupStep eachFilterAndGroupStep = CreateFilterLayer(); 
                    eachFilterAndGroupStepsAdded.Add(eachFilterAndGroupStep);
                    eachFilterAndGroupStep.SetDisplayContent(filtersAppliedAtEachStep);
                }
                else
                {
                    eachFilterAndGroupStepsAdded[index].SetDisplayContent(filtersAppliedAtEachStep);
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

        public List<string> GetFilteredPatiendIds(List<CAS_FilterKeyValuesClass> filterKeyValues)
        {
            List<string> stringFilterKeys = new List<string>();
            List<List<string>> stringFilterValues = new List<List<string>>();
            List<string> doubleFilterKeys = new List<string>();
            List<List<double>> doubleFilterValues = new List<List<double>>();

            foreach (CAS_FilterKeyValuesClass filterKeyValue in filterKeyValues)
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

            Debug.Log(stringFilterKeys.Count);
            Debug.Log(doubleFilterKeys.Count);

            return filterAndGroupManager.manager.dataManager.GetFilteredPatientIdsStringAndInteger(stringFilterKeys, stringFilterValues, doubleFilterKeys, doubleFilterValues);
        }
    }
}

