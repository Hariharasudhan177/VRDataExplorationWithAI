using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace CAS
{
    public class CAS_EachFilterAndGroupSubOption : MonoBehaviour
    {
        public CAS_FilterAndGroupSubOptionPanel filterAndGroupSubOptionPanel;

        public TextMeshProUGUI label;

        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupSubOptionPanel = GetComponentInParent<CAS_FilterAndGroupSubOptionPanel>(); 
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
            filterAndGroupSubOptionPanel.ToggledThisFilterSubOption(gameObject.name, value); 
        }
    }
}

