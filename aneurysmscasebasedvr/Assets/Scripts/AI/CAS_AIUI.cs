using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace CAS
{
    public class CAS_AIUI : MonoBehaviour
    {
        public CAS_AIManager aiManager; 
        public CAS_ObjectOfInterestUI objectOfInterestUI;
        public CAS_SimilarityUI similarityUI;
        public CAS_ClassificationUI classificationUI;
        public GameObject aiUIParent; 

        bool aiUIVisibilityStatus = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenClose()
        {
            aiUIVisibilityStatus = !aiUIVisibilityStatus;

            objectOfInterestUI.ActivatePanel(aiUIVisibilityStatus);

            if (aiUIVisibilityStatus)
            {
                aiUIParent.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                aiUIParent.GetComponent<CanvasGroup>().alpha = 0;
            }
            aiUIParent.GetComponent<CanvasGroup>().interactable = aiUIVisibilityStatus;
            aiUIParent.GetComponent<TrackedDeviceGraphicRaycaster>().enabled = aiUIVisibilityStatus;
        }
    }
}