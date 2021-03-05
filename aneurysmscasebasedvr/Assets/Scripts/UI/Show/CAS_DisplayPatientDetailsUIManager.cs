using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_DisplayPatientDetailsUIManager : MonoBehaviour
    {
        public CAS_Manager manager;

        public CAS_ShowDataUI showDataUI;
        public CAS_PatientIdListUI patientIdListUI;

        bool displayPatientDetailsUIVisibilityStatus = true; 

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PopulatePatientDisplay()
        {
            patientIdListUI.PopulatePatientIdList(); 
        }

        public void OpenClose()
        {
            displayPatientDetailsUIVisibilityStatus = !displayPatientDetailsUIVisibilityStatus;

            showDataUI.OpenClose(displayPatientDetailsUIVisibilityStatus);
            patientIdListUI.OpenClose(displayPatientDetailsUIVisibilityStatus);
        }
    }
}

