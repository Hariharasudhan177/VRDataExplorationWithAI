using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

namespace CAS
{
    public class CAS_EachFilterAndGroupOption : MonoBehaviour
    {
        CAS_EachFilterAndGroupOptionType filterAndGroupOptionsType;

        public TextMeshProUGUI buttonTextName;
        // Start is called before the first frame update
        void Start()
        {
            filterAndGroupOptionsType = GetComponentInParent<CAS_EachFilterAndGroupOptionType>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetEachFilterAndGroupOptionContent(string name)
        {
            buttonTextName.text = name;
        }

        public void OnClickThisFilterAndGroupOptionButton()
        {
            filterAndGroupOptionsType.SetFilterOptionSelected(gameObject.name);
        }

    }
}