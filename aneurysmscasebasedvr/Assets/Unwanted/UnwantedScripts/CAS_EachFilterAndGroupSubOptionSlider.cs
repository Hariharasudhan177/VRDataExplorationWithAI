using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CAS_EachFilterAndGroupSubOptionSlider : MonoBehaviour
{
    public Slider slider; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEachFilterAndGroupSubOptionSliderContent(double min, double max)
    {
        slider.minValue = (float) min;
        slider.maxValue = (float) max; 
    }
}
