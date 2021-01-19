using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace CAS
{
    public class CAS_FilterSubOption : MonoBehaviour
    {
        public CAS_FilterUI filterUI;
        public CAS_FilterSubOptionUI filterSubOptionUI; 

        public TextMeshProUGUI filterSubOptionLabel;
        // Start is called before the first frame update
        void Start()
        {
            filterSubOptionUI = GetComponentInParent<CAS_FilterSubOptionUI>();
            filterUI = filterSubOptionUI.filterUI; 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetFitlerSubOptionLabel(string value)
        {
            filterSubOptionLabel.text = value;
        }

        public void OnClickFilterSubOptionButton(bool value)
        {
            filterSubOptionUI.ToggledThisFilterSubOption(gameObject.name, value);
        }
    }
}

