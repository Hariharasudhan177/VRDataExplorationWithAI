using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAS_Rotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
    }
}
