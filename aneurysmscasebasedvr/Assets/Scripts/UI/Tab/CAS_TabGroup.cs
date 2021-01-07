using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CAS
{
    public class CAS_TabGroup : MonoBehaviour
    {
        private List<CAS_TabButton> tabButtons;
        public Sprite tabIdle;
        public Sprite tabHover;
        public Sprite tabActive;
        public CAS_TabButton selectedTab;
        public List<GameObject> ObjectsToSwap;

        public void Subcribe(CAS_TabButton button)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<CAS_TabButton>();
            }

            tabButtons.Add(button);
        }

        public void OnTabEnter(CAS_TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
            {
                button.background.sprite = tabHover;
            }
        }

        public void OnTabExit(CAS_TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(CAS_TabButton button)
        {
            selectedTab = button;
            ResetTabs();
            button.background.sprite = tabActive;
            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < ObjectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    ObjectsToSwap[i].SetActive(true);
                }
                else
                {
                    ObjectsToSwap[i].SetActive(false);
                }
            }
        }

        public void ResetTabs()
        {
            foreach (CAS_TabButton button in tabButtons)
            {
                if (selectedTab != null && button == selectedTab)
                {
                    continue;
                }
                button.background.sprite = tabIdle;
            }
        }
    }
}

