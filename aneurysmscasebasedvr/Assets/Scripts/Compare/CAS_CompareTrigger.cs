using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace CAS
{
    public class CAS_CompareTrigger : MonoBehaviour
    {
        public XRNode leftOrRightHand;
        InputDevice inputXRDevice;

        public CAS_CompareManager compareManager;
        bool modelsPlaced = false;

        Material mateiral;
        bool triggerOn = false;

        bool status = true;

        float red = 0f;
        float green = 1f; 

        // Start is called before the first frame update
        void Start()
        {
            List<UnityEngine.XR.InputDevice> inputXRDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(leftOrRightHand, inputXRDevices);
            if (inputXRDevices.Count > 0)
            {
                inputXRDevice = inputXRDevices[0];
            }

            mateiral = GetComponent<MeshRenderer>().material; 
        }

        private void Update()
        {
            if (triggerOn)
            {
                float alpha = mateiral.color.a;

                alpha -= 0.05f;

                if (alpha > 0f)
                {
                    mateiral.color = new Color(red, green, 0f, alpha); 
                }
                else
                {
                    mateiral.color = new Color(0f, 0f, 0f, 0.40f);
                    status = true;
                    red = 0f;
                    green = 1f;
                    triggerOn = false; 
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            bool triggerPressed = false;
            if (inputXRDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
            {
                if (modelsPlaced)
                {
                    if (other.gameObject.GetComponentInParent<CAS_ContolModel>() != null)
                    {
                        bool status = compareManager.manager.compareManager.AddIdToList(compareManager.manager.stepManager.allModelsInformationGameobjectRecordName[other.transform.parent.gameObject.name][0]);

                        if (status)
                        {
                            red = 0f;
                            green = 1f;
                        }
                        else
                        {
                            red = 1f;
                            green = 0f;
                        }

                        mateiral.color = new Color(red, green, 0f, 0.75f);            
                        triggerOn = true; 
                    }
                }
            }
        }

        public void SetModelsPlacedTrue()
        {
            modelsPlaced = true; 
        }
    }
}