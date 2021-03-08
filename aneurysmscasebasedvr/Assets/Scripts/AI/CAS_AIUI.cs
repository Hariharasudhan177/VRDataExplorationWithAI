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

        public GameObject similartiyVisualisation;
        public GameObject similartiyContent;
        public Camera cameraForRadialChart; 
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

        public void OnClickVisualisationOfSimilarity(float value)
        {
            if (value == 0)
            {
                similartiyVisualisation.SetActive(false);
                similartiyContent.SetActive(true);
                cameraForRadialChart.gameObject.SetActive(false); 
                aiManager.DeActivateSimilarityVisualisation();
            }
            else
            {
                similartiyVisualisation.SetActive(true);
                similartiyContent.SetActive(false);
                cameraForRadialChart.gameObject.SetActive(true);
                aiManager.ActivateSimilartiyVisualisation();
            }
        }
    }
}