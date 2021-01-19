using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_FilterAndGroupOptionKeyValuesClass
    {
        string filterName; 
        List<string> stringValues;
        List<double> doubleValues;
        bool isString;

        public CAS_FilterAndGroupOptionKeyValuesClass(string filterName, List<string> stringValues, List<double> doubleValues, bool isString)
        {
            this.filterName = filterName;
            this.isString = isString; 

            if (isString)
            {
                this.stringValues = stringValues;
            }
            else
            {
                this.doubleValues = doubleValues;
            }
        }

        public string GetFilterName()
        {
            return filterName; 
        }

        public List<string> GetStringValues()
        {
            if (isString)
            {
                return stringValues;
            }
            else
            {
                Debug.LogError("Asking double filter suboptions to string filter option");
                return new List<string>();
            }
        }

        public List<double> GetDoubleValues()
        {
            if (!isString)
            {
                return doubleValues;
            }
            else
            {
                Debug.LogError("Asking string filter suboptions to double filter option");
                return new List<double>();
            }
        }

        public bool GetIsString()
        {
            return isString; 
        }

        public void SetStringValues(List<string> stringValues)
        {
            if (isString)
            {
                this.stringValues = stringValues;
            }
            else
            {
                Debug.LogError("Assigning string filter suboptions to double filter option");
            }
        }

        public void SetDoubleValues(List<double> doubleValues)
        {
            if (!isString)
            {
                this.doubleValues = doubleValues;
            }
            else
            {
                Debug.LogError("Assigning double filter suboptions to string filter option");
            }
        }

        public void SetFilterName(string filtername)
        {
            this.filterName = filtername; 
        }


    }
}

