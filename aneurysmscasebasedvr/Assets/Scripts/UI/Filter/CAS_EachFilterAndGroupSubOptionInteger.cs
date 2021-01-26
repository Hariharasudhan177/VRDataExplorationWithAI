using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace CAS
{
    public class CAS_EachFilterAndGroupSubOptionInteger : MonoBehaviour
    {
        public CAS_EachFilterAndGroupSubOptionPanel subOptionPanel;

        public Slider fromSlider;
        public Slider toSlider;
        public TextMeshProUGUI fromValueDisplayText;
        public TextMeshProUGUI toValueDisplayText;

        float originalMinimum;
        float originalMaximum; 

        bool settingValue = false;

        int subOptionIntegerIndex;

        public TextMeshProUGUI conditionNumber; 

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

            if(index > 0)
            {
                conditionNumber.gameObject.SetActive(true); 
                conditionNumber.text = "Condition " + (subOptionIntegerIndex+1);
            }


            settingValue = true; 
            fromSlider.minValue = (float)min;
            fromSlider.maxValue = (float)max;
            fromSlider.gameObject.SetActive(true); 

            toSlider.minValue = (float)min;
            toSlider.maxValue = (float)max;
            toSlider.gameObject.SetActive(true);
            //toSlider.value = (float)max; //Reversed to slider - not needed 
            settingValue = false;

            originalMinimum = (float) min;
            originalMaximum = (float) max;

            fromValueDisplayText.text = originalMinimum.ToString();
            toValueDisplayText.text = originalMaximum.ToString(); 
        }

        public void ChageFromSliderValue(float value)
        {
            float convertedToValueSinceReversed = originalMaximum - (toSlider.value - originalMinimum);

            if (!settingValue)
            {
                if (value > convertedToValueSinceReversed)
                {
                    fromSlider.value = convertedToValueSinceReversed;
                    return;
                }

                subOptionPanel.SetFromToSliderValue(0, value, subOptionIntegerIndex);
            }

            fromValueDisplayText.text = fromSlider.value.ToString(); 
        }

        public void ChageToSliderValue(float value)
        {
            float convertedToValueSinceReversed = originalMaximum - (value - originalMinimum); 

            if (!settingValue)
            {
                if(convertedToValueSinceReversed < fromSlider.value)
                {
                    toSlider.value = originalMaximum + originalMinimum - fromSlider.value; 
                    return; 
                }

                subOptionPanel.SetFromToSliderValue(1, convertedToValueSinceReversed, subOptionIntegerIndex);
            }

            toValueDisplayText.text = convertedToValueSinceReversed.ToString();
        }

        public void SetOriginalValues()
        {
            fromSlider.value = originalMinimum;
            //toSlider.value = originalMaximum; because of conversion
            toSlider.value = originalMinimum;
        }
    }
}