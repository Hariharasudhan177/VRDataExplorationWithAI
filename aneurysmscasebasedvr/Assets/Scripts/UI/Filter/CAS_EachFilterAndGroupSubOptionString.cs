using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

namespace CAS
{
    public class CAS_EachFilterAndGroupSubOptionString : MonoBehaviour
    {
        public CAS_EachFilterAndGroupSubOptionPanel eachSubOptionPanel;

        public TextMeshProUGUI label;

        public Toggle toggle; 

        // Start is called before the first frame update
        void Start()
        {
            eachSubOptionPanel = GetComponentInParent<CAS_EachFilterAndGroupSubOptionPanel>(); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetEachFilterAndGroupSubOptionContent(string name)
        {
            label.text = name;
        }

        public void OnClickSubOptionToggle(bool value)
        {
            eachSubOptionPanel.ToggledThisFilterSubOption(gameObject.name, value); 
        }

        public void UncheckToggle()
        {
            toggle.isOn = false; 
        }
    }
}

