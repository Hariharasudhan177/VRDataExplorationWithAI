using UnityEngine;
using System.Data;
using System.Linq;
using System;

public class CAS_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataTable testTable = new DataTable("testTable");
        Console.WriteLine(testTable.AsEnumerable());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
