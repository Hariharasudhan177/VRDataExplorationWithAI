using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace CAS
{
    public class CAS_FilterSubOptionStringDetailClass
    {
        string name;

        bool isString;

        CAS_EachFilterAndGroupSubOptionString toggle;
        bool toggleStatus;

        public CAS_FilterSubOptionStringDetailClass(string name, bool isString, CAS_EachFilterAndGroupSubOptionString toggle, bool toggleStatus)
        {
            this.name = name;
            this.isString = isString;
            this.toggle = toggle;
            this.toggleStatus = toggleStatus;
        }

        public string GetSubOptionName()
        {
            return name;
        }

        public void SetSubOptionName(string name)
        {
            this.name = name;
        }

        public bool GetIsString()
        {
            return isString;
        }

        public void SetIsString(bool isString)
        {
            this.isString = isString;
        }

        public CAS_EachFilterAndGroupSubOptionString GetToggle()
        {
            return toggle;
        }

        public void SetToggle(CAS_EachFilterAndGroupSubOptionString toggle)
        {
            this.toggle = toggle;
        }

        public bool GetToggleStatus()
        {
            return toggleStatus;
        }

        public void SetToggleStatus(bool status)
        {
            this.toggleStatus = status;
        }
    }
}