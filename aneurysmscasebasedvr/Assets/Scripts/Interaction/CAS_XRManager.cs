using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CAS_XRManager : MonoBehaviour
{
    public GameObject viveControllers;
    public GameObject oculusControllers; 
    // Start is called before the first frame update
    void Start()
    {
        if(XRSettings.loadedDeviceName == "OpenVR")
        {
            viveControllers.SetActive(true);
        }
        else if(XRSettings.loadedDeviceName == "Oculus")
        {
            oculusControllers.SetActive(true); 
        }else
        {
            //Debug.LogError("Unknown VR input system: " + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
