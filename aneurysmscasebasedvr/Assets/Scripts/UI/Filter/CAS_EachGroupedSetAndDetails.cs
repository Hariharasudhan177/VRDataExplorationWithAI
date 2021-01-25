using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace CAS
{
    public class CAS_EachGroupedSetAndDetails : MonoBehaviour
    {
        public Image groupedColor;
        public TextMeshProUGUI groupedDetails; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetGroupedDetailsContent(Color colour, string subKeyName, int count)
        {
            groupedColor.color = colour;
            groupedDetails.text = subKeyName + " - " + count; 
        }
    }

}