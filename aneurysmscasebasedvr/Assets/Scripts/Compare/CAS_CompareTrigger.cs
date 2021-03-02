using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_CompareTrigger : MonoBehaviour
    {
        public CAS_Manager manager; 

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.GetComponent<CAS_ContolModel>())
            {
               manager.compareManager.AddIdToList(manager.stepManager.allModelsInformationGameobjectRecordName[collision.collider.transform.parent.gameObject.name][0]); 
            }
        }
    }
}