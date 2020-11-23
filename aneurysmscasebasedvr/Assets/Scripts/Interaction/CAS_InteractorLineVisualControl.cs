using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit; 

/// <summary>
/// To control XRInteractorLineVisual
/// </summary>
public class CAS_InteractorLineVisualControl : MonoBehaviour
{
    public XRNode leftOrRightHand;
    InputDevice inputXRDevice; 

    //Use Input helpers later 
    public enum CAS_XRButtonTypes
    {
        triggerTouch, 
        touchPadTouch
    }

    public CAS_XRButtonTypes activateLineButton; 

    private void Start()
    {
        List<InputDevice> inputXRDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(leftOrRightHand, inputXRDevices); 
        if(inputXRDevices.Count > 0)
        {
            inputXRDevice = inputXRDevices[0]; 
        }
    }

    private void Update()
    {
        //To activate line visual only when trigger button is pressed 
        if(inputXRDevice != null)
        {
            if(activateLineButton == CAS_XRButtonTypes.touchPadTouch)
            {
                bool touchButtonTouched;
                if (inputXRDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out touchButtonTouched) && touchButtonTouched)
                {
                    gameObject.GetComponent<XRInteractorLineVisual>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
                }
            }
            else if(activateLineButton == CAS_XRButtonTypes.triggerTouch)
            {
                float triggerButtonTouched;
                if (inputXRDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerButtonTouched) && triggerButtonTouched > 0.065f)
                {
                    gameObject.GetComponent<XRInteractorLineVisual>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
                }
            }
        }
    }
}
