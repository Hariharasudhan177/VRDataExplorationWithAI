using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_ClassificationUI : MonoBehaviour
    {
        public CAS_AIUI aiUI;

        public GameObject similarityValuePrefab;

        public Transform parentContent;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulateData(Dictionary<string, string> similarityValues, string predicted)
        {
            foreach (Transform child in parentContent)
            {
                Destroy(child.gameObject);
            }

            foreach (string patientId in similarityValues.Keys)
            {
                GameObject eachFieldOfDataObject = Instantiate(similarityValuePrefab, parentContent);
                eachFieldOfDataObject.GetComponent<CAS_EachFieldOfData>().SetColumnNameAndFieldData(patientId, similarityValues[patientId]);
            }

            GameObject eachFieldOfDataObjectFinal = Instantiate(similarityValuePrefab, parentContent);
            eachFieldOfDataObjectFinal.GetComponent<CAS_EachFieldOfData>().SetColumnNameAndFieldData("Prediction", predicted);
        }

        public void UnPopulateData()
        {
            foreach (Transform child in parentContent)
            {
                Destroy(child.gameObject);
            }
        }
    }

}