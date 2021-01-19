using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ToggleTest : MonoBehaviour
{
    public Toggle toggle; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Hari");
            SetToggle();
        }
    }

    public void OnClickToggle(bool toggleValue)
    {
        Debug.Log(toggleValue); 
    }

    public void SetToggle()
    {
        toggle.isOn = false;
        Debug.Log(toggle.isOn); 
    }
}
