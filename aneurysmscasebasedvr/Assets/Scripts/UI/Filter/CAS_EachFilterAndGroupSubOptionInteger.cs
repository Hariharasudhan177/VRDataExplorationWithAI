using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_EachFilterAndGroupSubOptionInteger : MonoBehaviour
    {
        public CAS_EachFilterAndGroupSubOptionPanel subOptionPanel;

        public Slider fromSlider;
        public Slider toSlider;

        double originalMinimum;
        double originalMaximum; 

        bool settingValue = false; 

        // Start is called before the first frame update
        void Start()
        {
            subOptionPanel = GetComponentInParent<CAS_EachFilterAndGroupSubOptionPanel>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetEachFilterAndGroupSubOptionContent(double min, double max)
        {
            settingValue = true; 
            fromSlider.minValue = (float)min;
            fromSlider.maxValue = (float)max;
            fromSlider.gameObject.SetActive(true); 

            toSlider.minValue = (float)min;
            toSlider.maxValue = (float)max;
            toSlider.gameObject.SetActive(true);
            toSlider.value = (float)max;
            settingValue = false;

            originalMinimum = min;
            originalMaximum = max; 
        }

        public void ChageFromSliderValue(float value)
        {
            if (!settingValue)
            {
                if (value > toSlider.value)
                {
                    fromSlider.value = toSlider.value;
                    return;
                }

                subOptionPanel.SetFromToSliderValue(0, value);
            }
        }

        public void ChageToSliderValue(float value)
        {
            if (!settingValue)
            {
                if(value < fromSlider.value)
                {
                    toSlider.value = fromSlider.value; 
                    return; 
                }

                subOptionPanel.SetFromToSliderValue(1, value);
            }
        }

        public void SetOriginalValues()
        {
            fromSlider.value = (float)originalMinimum; 
            toSlider.value = (float)originalMaximum;
        }
    }

}