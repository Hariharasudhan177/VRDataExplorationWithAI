using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_TextToSwitch : MonoBehaviour
    {
        private TextMeshProUGUI textToSwitch;
        private void Start()
        {
            textToSwitch = GetComponent<TextMeshProUGUI>(); 
        }
        public void ChangeText(Color color)
        {
            textToSwitch.color = color;
        }
    }
}