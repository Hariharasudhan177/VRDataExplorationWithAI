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
            fromSlider.minValue = (float)min;
            fromSlider.maxValue = (float)max;

            toSlider.minValue = (float)min;
            toSlider.maxValue = (float)max;
        }

        public void ChageFromSliderValue(float value)
        {
            filterAndGroupSubOptionPanel.SetFromToSliderValue(0, value);
        }

        public void ChageToSliderValue(float value)
        {
            filterAndGroupSubOptionPanel.SetFromToSliderValue(1, value);
        }
    }

}