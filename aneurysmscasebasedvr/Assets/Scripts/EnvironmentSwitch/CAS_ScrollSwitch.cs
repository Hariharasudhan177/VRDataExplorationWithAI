using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CAS
{
    public class CAS_ScrollSwitch : MonoBehaviour
    {
        private Image scrollToSwitch;
        private void Start()
        {
            scrollToSwitch = GetComponent<Image>();
        }
        public void ChangeColor(Color color)
        {
            if(scrollToSwitch == null)
            {
                scrollToSwitch = GetComponent<Image>();
            }
            scrollToSwitch.color = color;
        }
    }
}