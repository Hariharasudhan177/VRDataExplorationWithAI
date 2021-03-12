using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;

namespace CAS
{
    public class CAS_EachFilterAndGroupSubOptionInteger : MonoBehaviour
    {
        public CAS_EachFilterAndGroupSubOptionPanel subOptionPanel;

        public TextMeshProUGUI fromValueDisplayText;
        public TextMeshProUGUI toValueDisplayText;

        float originalMinimum;
        float originalMaximum;

        bool settingValue = false;

        int subOptionIntegerIndex;

        public TextMeshProUGUI conditionNumber;

        public RangeSlider rangeSlider;

        // Start is called before the first frame update
        void Start()
        {
            subOptionPanel = GetComponentInParent<CAS_EachFilterAndGroupSubOptionPanel>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetEachFilterAndGroupSubOptionContent(double min, double max, int index)
        {
            subOptionIntegerIndex = index;

            if (index > 0)
            {
                conditionNumber.gameObject.SetActive(true);
                conditionNumber.text = "Condition " + (subOptionIntegerIndex + 1);
            }


            settingValue = true;
            rangeSlider.MinValue = (float)min;
            rangeSlider.MaxValue = (float)max;
            rangeSlider.LowValue = (float)min;
            rangeSlider.HighValue = (float)max;
            //toSlider.value = (float)max; //Reversed to slider - not needed 
            settingValue = false;

            originalMinimum = (float)min;
            originalMaximum = (float)max;

            fromValueDisplayText.text = originalMinimum.ToString();
            toValueDisplayText.text = originalMaximum.ToString();

        }

        public void SetOriginalValues()
        {
            rangeSlider.LowValue = originalMinimum;
            rangeSlider.HighValue = originalMaximum;
        }

        public void ChangeFromToSlider(float from, float to)
        {
            if (!settingValue)
            {
                if (subOptionPanel == null) subOptionPanel = GetComponentInParent<CAS_EachFilterAndGroupSubOptionPanel>();

                subOptionPanel.SetFromToSliderValue(0, from, subOptionIntegerIndex);
                subOptionPanel.SetFromToSliderValue(1, to, subOptionIntegerIndex);
                fromValueDisplayText.text = from.ToString();
                toValueDisplayText.text = to.ToString();
            }
        }
    }
}