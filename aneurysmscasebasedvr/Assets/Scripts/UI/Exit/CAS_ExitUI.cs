using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_ExitUI : MonoBehaviour
    {
        bool visible = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenCloseExitPanel()
        {
            visible = !visible;
            ActivateExitPanel(visible);
        }

        void ActivateExitPanel(bool _visible)
        {
            if (_visible)
            {
                GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                GetComponent<CanvasGroup>().alpha = 0;
            }
            GetComponent<CanvasGroup>().interactable = _visible;
            GetComponent<TrackedDeviceGraphicRaycaster>().enabled = _visible;
        }

        public void OnClickExitYesButton()
        {
            Application.Quit(); 
        }
    }
}

