using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro; 

namespace CAS
{
    public class CAS_EachFilterAndGroupStep : MonoBehaviour
    {
        CAS_FilterAndGroupUIManager filterAndGroupUIManager;

        public Transform filterDisplayListParent;

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

        public void SetFilterDisplayContent(List<CAS_FilterAndGroupOptionKeyValuesClass> filtersApplied, List<List<string>> modelsForAllSteps)
        {
            foreach (Transform child in filterDisplayListParent)
            {
                Destroy(child.gameObject); 
            }

            int filtersAppliedCount = 0; 

            foreach (CAS_FilterAndGroupOptionKeyValuesClass filterAndGroupOptionKeyValueClass in filtersApplied)
            {

                string filterOptionHeadingText = "";
                string filterOptionValuesText = "";
                int filterOptionsSize = 0;

                CAS_FilterAndGroupOptionKeyValuesClass filterKeyValuesClass = filterAndGroupOptionKeyValueClass;
                filterOptionHeadingText += filterKeyValuesClass.GetFilterName() + " - " + modelsForAllSteps[filtersAppliedCount].Count + " selected";

                if (filterKeyValuesClass.GetIsString())
                {
                    foreach (string filterValue in filterKeyValuesClass.GetStringValues())
                    {
                        filterOptionValuesText += filterValue + "\n";
                        filterOptionsSize++; 
                    }
                }
                else
                {
                    int index = 0;
                    foreach (double filterValue in filterKeyValuesClass.GetDoubleValues())
                    {
                        if (index == 0)
                        {
                            if (filterKeyValuesClass.GetFilterName() == "age")
                            {
                                filterOptionValuesText += "from " + "\n" + ((int)Mathf.Ceil((float)filterValue)).ToString() + "\n" + " to " + "\n";
                            }
                            else
                            {
                                filterOptionValuesText += "from " + "\n" + filterValue + "\n" + "to " + "\n";
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
                    filterOptionsSize = 4; 
                }

                GameObject displayListGameObject = Instantiate(filterAndGroupUIManager.stepUI.displayListPrefab, filterDisplayListParent);
                displayListGameObject.GetComponent<CAS_FilterDisplayList>().SetDisplayContent(filterOptionHeadingText, filterOptionValuesText, filterOptionsSize);

                filtersAppliedCount++; 
            }
        }
    }
}

