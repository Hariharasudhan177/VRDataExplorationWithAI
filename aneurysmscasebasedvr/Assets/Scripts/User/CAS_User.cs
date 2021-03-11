using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CAS_User : MonoBehaviour
{
    //Parent of the Camera
    public Transform VRCamHolder; 

    public float WaitBeforeSetting = 0.5f;

    // Use this for initialization
    void Start()
    {
        
    }

    public void SetInitialUserPositionAndRotation()
    {
        
        VRCamHolder.position = transform.position;

        Transform cameraCurrent = VRCamHolder.Find("Main Camera").transform;

        float yRotation = VRCamHolder.transform.eulerAngles.y - cameraCurrent.eulerAngles.y;

        VRCamHolder.transform.eulerAngles = new Vector3(VRCamHolder.transform.eulerAngles.x, yRotation, VRCamHolder.transform.eulerAngles.z); 
    }
}