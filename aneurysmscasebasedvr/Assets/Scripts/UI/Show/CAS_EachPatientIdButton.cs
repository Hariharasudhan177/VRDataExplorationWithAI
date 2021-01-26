using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace CAS
{
    public class CAS_EachPatientIdButton : MonoBehaviour
    {
        CAS_PatientIdListUI patientIdListUI; 

        public TextMeshProUGUI patientIdButtonText; 

        // Start is called before the first frame update
        void Start()
        {
            patientIdListUI = GetComponentInParent<CAS_PatientIdListUI>(); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetPatientIdButtonContent(string patientId)
        {
            patientIdButtonText.text = patientId; 
        }

        public void OnClickPatientIdButton()
        {
            patientIdListUI.OnClickPatientIdButton(gameObject.name); 
        }
    }
}

