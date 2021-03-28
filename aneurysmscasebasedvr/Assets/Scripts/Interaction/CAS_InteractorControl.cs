using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.XR.Interaction.Toolkit;

namespace CAS
{
    public class CAS_InteractorControl : MonoBehaviour
    {
        public XRBaseInteractor otherInteractor;

        public bool otherInteractorActiveStatus = false; 

        public void SetOtherInteractorInActive()
        {
            otherInteractor.InteractionLayerMask = LayerMask.GetMask("NotInteractables");

            otherInteractorActiveStatus = true; 
        }

        public void SetOtherInteractorActive()
        {
            otherInteractor.InteractionLayerMask = LayerMask.GetMask("ModelInteractables");

            otherInteractorActiveStatus = false; 
        }
    }
}