using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace CAS
{
    public class CAS_EachFilterAndGroupSubOption : MonoBehaviour
    {
        public TextMeshProUGUI label;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetEachFilterAndGroupSubOptionContent(string name)
        {
            label.text = name;
        }
    }
}

