using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_EachFilterAndGroupSubOptionInteger : MonoBehaviour
    {
        public CAS_FilterAndGroupSubOptionPanel filterAndGroupSubOptionPanel;

        public Slider fromSlider;
        public Slider toSlider;

        bool settingValue = false; 

        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupSubOptionPanel = GetComponentInParent<CAS_FilterAndGroupSubOptionPanel>();
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

                filterAndGroupSubOptionPanel.SetFromToSliderValue(0, value);
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

                filterAndGroupSubOptionPanel.SetFromToSliderValue(1, value);
            }
        }
    }

}