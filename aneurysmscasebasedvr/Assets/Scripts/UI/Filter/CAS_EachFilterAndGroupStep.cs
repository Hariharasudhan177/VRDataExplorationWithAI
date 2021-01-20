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
        GameObject displayListGameObject;


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

        public void SetFilterDisplayContent(List<CAS_FilterAndGroupOptionKeyValuesClass> filtersApplied)
        {
            string filterOptionHeadingText = "";
            string filterOptionValuesText = "";

            CAS_FilterAndGroupOptionKeyValuesClass filterKeyValuesClass = filtersApplied[filtersApplied.Count-1];
            filterOptionHeadingText += filterKeyValuesClass.GetFilterName();

            if (filterKeyValuesClass.GetIsString())
            {
                foreach (string filterValue in filterKeyValuesClass.GetStringValues())
                {
                    filterOptionValuesText += filterValue + "\n";
                }
            }
            else
            {
                int index = 0;
                foreach (double filterValue in filterKeyValuesClass.GetDoubleValues())
                {
                    if (index == 0)
                    {
                        if(filterKeyValuesClass.GetFilterName() == "age")
                        {
                            filterOptionValuesText += "Between " + ((int)Mathf.Ceil((float)filterValue)).ToString() + " and ";
                        }
                        else
                        {
                            filterOptionValuesText += "Between " + filterValue + " and ";
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
            }

            if (!displayListGameObject)
            {
                displayListGameObject = Instantiate(filterAndGroupUIManager.stepUI.displayListPrefab, filterDisplayListParent);
            }
 
            displayListGameObject.GetComponent<CAS_FilterDisplayList>().SetDisplayContent(filterOptionHeadingText, filterOptionValuesText); 
        }
    }
}

