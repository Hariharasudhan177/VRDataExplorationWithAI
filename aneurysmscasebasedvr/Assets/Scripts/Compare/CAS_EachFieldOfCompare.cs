using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_EachFieldOfCompare : MonoBehaviour
    {

        public TextMeshProUGUI content; 

        public void SetContent(string value)
        {
            content.text = value; 
        }
    }
}