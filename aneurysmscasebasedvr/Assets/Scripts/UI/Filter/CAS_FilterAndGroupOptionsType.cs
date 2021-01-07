using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CAS_FilterAndGroupOptionsType : MonoBehaviour
{
    public Transform buttonParent; 
    List<string> filterOptionsOfThisType; 

    // Start is called before the first frame update
    void Start()
    {
        filterOptionsOfThisType = new List<string>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOptionButtons(GameObject prefab, List<string> options)
    {
        Debug.Log(options.Count);
        foreach(string option in options)
        {
            GameObject filterOptionButton = Instantiate(prefab, buttonParent);
            filterOptionButton.name = option;
            filterOptionButton.GetComponent<CAS_EachFilterAndGroupOptions>().SetEachFilterAndGroupOptionContent(option); 
        }

        filterOptionsOfThisType = options;
    }
}
