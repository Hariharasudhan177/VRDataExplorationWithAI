using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace CAS
{
    public class CAS_FilterAndGroupOptionsType : MonoBehaviour
    {
        CAS_FilterAndGroupOptions filterAndGroupOptions;

        public Transform buttonParent;

        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupOptions = GetComponentInParent<CAS_FilterAndGroupOptions>(); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetFilterOptionSelected(string key)
        {
            filterAndGroupOptions.SetFilterOptionSelected(key); 
        }

        public void SetOptionButtons(GameObject prefab, List<string> options)
        {
            foreach (string option in options)
            {
                GameObject filterOptionButton = Instantiate(prefab, buttonParent);
                filterOptionButton.name = option;
                filterOptionButton.GetComponent<CAS_EachFilterAndGroupOptions>().SetEachFilterAndGroupOptionContent(option);
            }
        }
    }
}