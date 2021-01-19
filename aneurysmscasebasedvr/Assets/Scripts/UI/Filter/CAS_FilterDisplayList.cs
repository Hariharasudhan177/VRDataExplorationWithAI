using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace CAS
{
    public class CAS_FilterDisplayList : MonoBehaviour
    {
        public TextMeshProUGUI filterOptionHeading;
        public TextMeshProUGUI filterOptionValues;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDisplayContent(string filterOptionHeadingText, string filterOptionValuesText)
        {
            filterOptionHeading.text = filterOptionHeadingText;
            filterOptionValues.text = filterOptionValuesText;
        }
    }
}
