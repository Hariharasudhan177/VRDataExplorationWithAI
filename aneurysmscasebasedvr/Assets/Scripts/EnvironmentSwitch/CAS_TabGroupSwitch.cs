using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_TabGroupSwitch : MonoBehaviour
    {
        private CAS_TabGroup tabGroup;
        private void Start()
        {
            tabGroup = GetComponent<CAS_TabGroup>();
        }
        public void ChangeImages(Sprite idleImage, Sprite activeImage)
        {
            if(tabGroup == null)
            {
                tabGroup = GetComponent<CAS_TabGroup>();
            }
            tabGroup.tabHover = activeImage;
            tabGroup.tabActive = activeImage;
            tabGroup.tabIdle = idleImage;

            if(tabGroup.selectedTab != null)
            {
                tabGroup.selectedTab.background.sprite = tabGroup.tabActive; 
            }
        }
    }
}