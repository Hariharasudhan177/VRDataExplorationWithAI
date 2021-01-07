using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CAS_EachFilterAndGroupOptions : MonoBehaviour
{
    public TextMeshProUGUI buttonTextName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {

    }

    public void SetEachFilterAndGroupOptionContent(string name)
    {
        buttonTextName.text = name; 
    }
}
