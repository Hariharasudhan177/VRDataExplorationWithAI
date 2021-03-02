﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_MainPanelToSwitch : MonoBehaviour
    {
        private Image imageToSwitch;

        private void Start()
        {
            imageToSwitch = GetComponent<Image>(); 
        }
        public void ChangeImage(Sprite sprite)
        {
            imageToSwitch.sprite = sprite;
            //imageToSwitch.type = Image.Type.Sliced; 

        }
    }

}