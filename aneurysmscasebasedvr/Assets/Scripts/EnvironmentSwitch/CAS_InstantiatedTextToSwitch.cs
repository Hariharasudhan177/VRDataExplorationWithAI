using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace CAS
{
    public class CAS_InstantiatedTextToSwitch : MonoBehaviour
    {

        private TextMeshProUGUI textToSwitch;
        public CAS_EnvironmentSwitch environmentSwitch; 
        private void Start()
        {
            textToSwitch = GetComponent<TextMeshProUGUI>();
            environmentSwitch = FindObjectOfType<CAS_EnvironmentSwitch>();
            environmentSwitch.AddToInstantiatedTextToSwitch(this); 
        }
        public void ChangeText(Color color)
        {
            if (textToSwitch == null)
            {
                textToSwitch = GetComponent<TextMeshProUGUI>();
            }
            textToSwitch.color = color;
        }

        private void OnDestroy()
        {
            environmentSwitch.RemoveFromInstantiatedTextToSwitch(this);
        }
    }
}
