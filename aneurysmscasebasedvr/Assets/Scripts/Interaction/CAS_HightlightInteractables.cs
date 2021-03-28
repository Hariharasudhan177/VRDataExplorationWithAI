using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace CAS
{
    /// <summary>
    /// Highlight interacting interactables on hover 
    /// </summary>
    public class CAS_HightlightInteractables : MonoBehaviour
    {
        public XRNode leftOrRightHand;
        InputDevice inputXRDevice;

        XRBaseControllerInteractor interactor;

        //Hightlight 
        [SerializeField]
        [Tooltip("Default Colour")]
        Color defaultColor = Color.blue;
        [SerializeField]
        [Tooltip("Highlight colour on hover")]
        Color highlightedColor = Color.magenta;

        //ShowData
        public CAS_Manager manager;
        public CAS_InteractorControl interactableControl;

        // Start is called before the first frame update
        void Start()
        {
            List<InputDevice> inputXRDevices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(leftOrRightHand, inputXRDevices);
            if (inputXRDevices.Count > 0)
            {
                inputXRDevice = inputXRDevices[0];
            }

            interactor = GetComponent<XRBaseControllerInteractor>();

            interactor.onHoverEnter.AddListener(HighlightOnHoverEnter);
            interactor.onHoverExit.AddListener(DeHighlightOnHoverExit);
        }

        // Update is called once per frame
        void Update()
        {

        }

        //Better method is to write another ray interactor script to hover only when trigger touched 
        //For now just removing the highlighting 

        void HighlightOnHoverEnter(XRBaseInteractable interactable)
        {
            if (interactor.GetType() == typeof(XRDirectInteractor))
            {
                interactableControl.SetOtherInteractorInActive();
            }

            if ((interactor.GetType() == typeof(XRRayInteractor)))
            {
                float triggerTouched;
                inputXRDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerTouched);
                if (!(triggerTouched > 0.065f))
                {
                    return;
                }
                else
                {
                    interactableControl.SetOtherInteractorInActive();
                }
            }

            if (interactable.GetComponent<CAS_PrepareModels>())
            {
                //interactable.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", highlightedColor);
                if(interactable.transform.GetComponent<CAS_ContolModel>())
                    interactable.transform.GetComponent<CAS_ContolModel>().Highlight(highlightedColor);
                manager.stepManager.DisplayThisModelData(interactable.transform.GetChild(0).GetComponentInParent<CAS_GrabInteractable>().gameObject.name); 
            }
        //meshRenderer.material.SetColor("_Color", highlightedColor);
        }

        void DeHighlightOnHoverExit(XRBaseInteractable interactable)
        {
            interactableControl.SetOtherInteractorActive();

            if (interactable == null) return; 
            //manager.displayPatientDetailsUIManager.showDataUI.UnPopulateData();
            if (interactable.GetComponent<CAS_PrepareModels>())
            {
                //interactable.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);
                if(interactable.transform.GetComponent<CAS_ContolModel>())
                    interactable.transform.GetComponent<CAS_ContolModel>().DeHighlight();
            }
            //meshRenderer.material.SetColor("_Color", defaultColor);
        }
    }
}

