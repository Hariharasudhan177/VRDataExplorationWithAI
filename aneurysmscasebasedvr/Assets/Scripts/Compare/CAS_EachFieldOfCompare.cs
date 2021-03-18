using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CAS
{
    public class CAS_EachFieldOfCompare : MonoBehaviour
    {

        public TextMeshProUGUI content; 

        public void SetContent(string value)
        {
            if(value != "-1")
            {
                content.text = value;
            }
            else
            {
                content.text = "N/A"; 
            }
        }
    }
}