using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CAS_TabButtonSwitch : MonoBehaviour
{
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void ChangeImage(Sprite idleImage)
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
        image.sprite = idleImage; 
    }
}
