using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR; 

namespace CAS
{
    /// <summary>
    /// Push gesture
    /// </summary>
    public class CAS_PushGesture : MonoBehaviour
    {
        //Push Gesture 
        public XRNode leftOrRightHand; 
        InputDevice inputXRDevice;

        bool pushHappening = false;
        bool checkForPush = false; 

        // Start is called before the first frame update
        void Start()
        {
            List<UnityEngine.XR.InputDevice> inputXRDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(leftOrRightHand, inputXRDevices);
            if (inputXRDevices.Count > 0)
            {
                inputXRDevice = inputXRDevices[0];
            }
        }

        // Update is called once per frame
        //CAS Implementation - Device connected and disconnected check future may be turn into events 
        public void Update()
        {
            //For now commenting out that this could be possible without touchpad touch and stop checking for push when grip is pressed 
            /*if (inputXRDevice != null)
            {
                bool primary2DAxisTouched = false;
                if (inputXRDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisTouch, out primary2DAxisTouched) && primary2DAxisTouched)
                {
                    CheckForPush(inputXRDevice);
                    checkForPush = true;
                }
                else
                {
                    checkForPush = false; 
                }
            }*/

            if (inputXRDevice != null)
            {
                bool gripPressed = false;
                if (inputXRDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out gripPressed) && gripPressed)
                {
                    checkForPush = false;
                }
                else
                {
                    CheckForPush(inputXRDevice);
                    checkForPush = true;
                }
            }
        }

        void CheckForPush(InputDevice inputDevice)
        {
            Vector3 angularVelocity;
            inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceAngularVelocity, out angularVelocity);

            if (angularVelocity.x > 5 || angularVelocity.y > 5 || angularVelocity.z > 5)
            {
                Debug.Log("Trying to push");

                //RayPush
                Vector3 devicePosition;
                inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out devicePosition);

                Quaternion deviceRotation;
                inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out deviceRotation);
                Vector3 forwardVector = deviceRotation * Vector3.forward;

                //Debug.DrawRay(devicePosition, forwardVector, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(devicePosition, forwardVector, out hit, Mathf.Infinity))
                {
                    //Debug.DrawRay(devicePosition, forwardVector * hit.distance, Color.yellow);
                    //Debug.Log(hit.transform.gameObject.name + " is pushed");
                    hit.transform.gameObject.GetComponent<CAS_ContolModel>().PushToOriginalPosition(); 
                }

                //DirectPush
                pushHappening = true;
            }
            else
            {
                pushHappening = false; 
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log(other.gameObject + "" + checkForPush  + pushHappening);
            if (checkForPush && pushHappening)
            {
                //Debug.Log(other.transform.parent.gameObject.name + " is pushed"); 
                other.transform.parent.gameObject.GetComponent<CAS_ContolModel>().PushToOriginalPosition();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            //Debug.Log(other.gameObject + "" + checkForPush + pushHappening);
            if (checkForPush && pushHappening)
            {
                //Debug.Log(other.transform.parent.gameObject.name + " is pushed");
                other.transform.parent.gameObject.GetComponent<CAS_ContolModel>().PushToOriginalPosition();
            }
        }

    }
}

