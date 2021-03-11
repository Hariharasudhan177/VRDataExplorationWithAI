using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_CompareTrigger : MonoBehaviour
    {
        public CAS_Manager manager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<CAS_ContolModel>())
            {
                //Debug.Log("Hari");
                manager.compareManager.AddIdToList(manager.stepManager.allModelsInformationGameobjectRecordName[other.transform.parent.gameObject.name][0]);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.GetComponentInParent<CAS_ContolModel>())
            {
               //Debug.Log("Hari");
               manager.compareManager.AddIdToList(manager.stepManager.allModelsInformationGameobjectRecordName[collision.collider.transform.parent.gameObject.name][0]); 
            }
        }
    }
}