using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_FilterStepManager : MonoBehaviour
    {
        CAS_TabGroup tabGroup; 
        public CAS_TabButton[] tabButtons;

        Dictionary<int, bool> eachStepStatus;
        public Dictionary<int, CAS_EachFilterAndGroupStep> eachFilterAndGroupSteps;
        public int activeAndCurrentStep = 0;

        //string activeAndCurrentStep;

        public GameObject displayListPrefab;

        private void Awake()
        {
            eachFilterAndGroupSteps = new Dictionary<int, CAS_EachFilterAndGroupStep>();
            eachStepStatus = new Dictionary<int, bool>(); 
        }

        // Start is called before the first frame update
        void Start()
        {
            //tabButtons = GetComponentsInChildren<CAS_TabButton>();
            tabGroup = GetComponentInChildren<CAS_TabGroup>();

            int index = 0; 

            foreach(CAS_TabButton button in tabButtons)
            {
                CAS_EachFilterAndGroupStep eachFilterAndGroupStep = tabGroup.ObjectsToSwap[index].GetComponent<CAS_EachFilterAndGroupStep>();
                eachFilterAndGroupSteps.Add(index, eachFilterAndGroupStep);
                eachFilterAndGroupStep.stepNumber = index+1;

                if (index == 0)
                {
                    eachStepStatus.Add(index, true); 
                }
                else
                {
                    eachStepStatus.Add(index, false);
                }

                index++; 
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            int index = 0; 
            foreach(CAS_TabButton tabButton in tabButtons)
            {
                if (tabGroup.selectedTab.name == tabButton.name)
                {
                    activeAndCurrentStep = index; 
                }
                index++; 
            }            
        }

        public void ActivateStep(int index)
        {
            Debug.Log(index); 
            if(index > -1)
            {
                if(index < eachStepStatus.Count - 1)
                {
                    eachStepStatus[index] = true;
                    eachFilterAndGroupSteps[index].ActivateThisStep();
                }
            }
        }

        public void ActivateStepButton(int index)
        {
            if (index > -1)
            {
                if (index < eachStepStatus.Count - 1)
                {
                    tabButtons[index].GetComponent<Button>().interactable = true;
                }
            }          
        }

        public void DeActivateStepButton(int index)
        {
            if (index > -1)
            {
                if (index < eachStepStatus.Count - 1)
                {
                    tabButtons[index].GetComponent<Button>().interactable = false;
                }
            }
        }

        public void AddFilterToActiveStep(string filterKey, List<string> filterOptionsSelected)
        {
            eachFilterAndGroupSteps[activeAndCurrentStep].AddFilterToThisStep(filterKey, filterOptionsSelected); 
        }

        public void AddFilterToActiveStepInteger(string filterKey, List<double> filterOptionsSelected)
        {
            eachFilterAndGroupSteps[activeAndCurrentStep].AddFilterToThisStepInteger(filterKey, filterOptionsSelected);
        }

        public void RemoveFilterFromActiveStep(string filterKey)
        {
            eachFilterAndGroupSteps[activeAndCurrentStep].RemoveFilterFromThisStep(filterKey);
        }

        public void RemoveFilterFromActiveStepInteger(string filterKey)
        {
            eachFilterAndGroupSteps[activeAndCurrentStep].RemoveFilterFromThisStepInteger(filterKey);
        }

        public void AddGroupByToActiveStepInteger(string filterKey)
        {
            eachFilterAndGroupSteps[activeAndCurrentStep].ApplyGroupbyThisStep(filterKey);
        }

        public int GetCurrentAndActiveStep()
        {
            return activeAndCurrentStep; 
        }

        public void SetCurrentAndActiveStep(int value)
        {
            activeAndCurrentStep = value; 
        }
    }
}

