using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_FirstModel : MonoBehaviour
    {
        CAS_StepManager stepManager;
        CAS_ContolModel contolModel;

        bool initialized = false;
        bool placed = false; 

        public void Update()
        {
            if(initialized == false)
            {
                stepManager = GetComponentInParent<CAS_StepManager>(); 
                contolModel = GetComponent<CAS_ContolModel>(); 
                if(contolModel != null)
                {
                    initialized = true; 
                }
            }

            if(initialized == true)
            {
                if (!contolModel.GetModelMovingStatus())
                {
                    if (!placed)
                    {
                        placed = true;
                        stepManager.manager.compareManager.compareTriggerLeft.SetModelsPlacedTrue();
                        stepManager.manager.compareManager.compareTriggerRight.SetModelsPlacedTrue();
                        stepManager.manager.displayPatientDetailsUIManager.showDataUI.Initialize(gameObject.name);  
                    }
                }
            }
        }
    }

}