using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
using TMPro;
using UnityEngine.UI; 

namespace CAS
{
    public class CAS_AIUI : MonoBehaviour
    {
        public CAS_AIManager aiManager; 
        public CAS_ObjectOfInterestUI objectOfInterestUI;
        public CAS_SimilarityUI similarityUI;
        public CAS_ClassificationUI classificationUI;
        public GameObject aiUIParent;
        public TextMeshProUGUI visualisationButtonText;
        bool aiUIVisibilityStatus = true;

        public GameObject similartiyVisualisation;
        public GameObject similartiyContent;
        public Camera cameraForRadialChart;
        bool similarityVisualisationStatus = false;  
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

        public void OnClickVisualisationOfSimilarity()
        {
            if(aiManager.GetIndexOfInterest() == -1)
            {
                return; 
            }

            similarityVisualisationStatus = !similarityVisualisationStatus; 

            if (similarityVisualisationStatus)
            {
                ActivateSimilarityVisualisation();
            }
            else
            {
                DeactivateSimilarityVisualisation(); 
            }
        }

        public void DeactivateSimilarityVisualisation()
        {
            visualisationButtonText.text = "Activate Visualisation";
            similartiyVisualisation.SetActive(false);
            similartiyContent.SetActive(true);
            cameraForRadialChart.gameObject.SetActive(false);
            aiManager.DeActivateSimilarityVisualisation();
        }

        public void ActivateSimilarityVisualisation()
        {
            visualisationButtonText.text = "Deactivate Visualisation";
            similartiyVisualisation.SetActive(true);
            similartiyContent.SetActive(false);
            cameraForRadialChart.gameObject.SetActive(true);
            aiManager.ActivateSimilartiyVisualisation();
        }

        public void SimilarityVisualisationButton(bool value)
        {
            visualisationButtonText.transform.parent.GetComponent<Button>().interactable = value; 
        }
    }
}