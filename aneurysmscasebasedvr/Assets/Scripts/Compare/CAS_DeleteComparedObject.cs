using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_DeleteComparedObject : MonoBehaviour
    {
        string patientId;

        CAS_CompareManager compareManager; 
        
        // Start is called before the first frame update
        void Start()
        {
            compareManager = GetComponentInParent<CAS_CompareManager>(); 
        }

        public void OnClickDeleteButton()
        {
            compareManager.RemoveIdFromList(patientId); 
        }

        public void SetComparedPatientId(string compared)
        {
            patientId = compared;
        }
    }
}