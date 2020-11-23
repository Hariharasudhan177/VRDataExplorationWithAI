using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CAS
{
    public class CAS_EachFieldOfData : MonoBehaviour
    {
        public TextMeshProUGUI columnNameText;
        public TextMeshProUGUI fieldDataText;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetColumnNameAndFieldData(string columnName, string fieldData)
        {
            columnNameText.text = columnName;
            fieldDataText.text = fieldData;
        }
    }
}

