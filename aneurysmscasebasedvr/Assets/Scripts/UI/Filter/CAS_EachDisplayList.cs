using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace CAS
{
    public class CAS_EachDisplayList : MonoBehaviour
    {
        public TextMeshProUGUI displayContent; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDisplayContent(string content)
        {
            displayContent.text = content; 
        }
    }
}
