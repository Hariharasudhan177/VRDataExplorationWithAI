using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_FilterOption : MonoBehaviour
    {
        public CAS_FilterUI filterUI; 

        public TextMeshProUGUI filterOptionLabel;
        public GameObject filterSubOption; 

        // Start is called before the first frame update
        void Start()
        {
            filterUI = GetComponentInParent<CAS_FilterUI>();

            //Dynamically adding event as this button is added dynamically 
            GetComponent<Button>().onClick.AddListener(OnClickFilterOptionButton); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetFitlerOptionLabel(string value)
        {
            filterOptionLabel.text = value; 
        }

        public void SetFitlerSubOption(GameObject value)
        {
            filterSubOption = value;
        }

        public void OnClickFilterOptionButton()
        {
            filterUI.ClickedThisFilterOption(gameObject.name); 
        }
    }
}
