using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public enum LayerChangeType
    {
        Initial, 
        Forward, 
        Backward
    }

    public class CAS_ContolModel : MonoBehaviour
    {
        CAS_StepManager stepManager; 

        // Adjust the speed for the movement
        public float speed = 1.0f;
        public float speedPush= 10f;

        public Vector3 originalPosition;
        Vector3 currentOriginalPosition; 
        Quaternion originalRotation;

        bool moveToFirstLayer;
        bool moveToOriginalLayer;
        
        bool layerChangingInitial = false;
        bool layerChangingBackward = false;
        bool layerChangingForward = false;
        bool pushingToOriginalPosition = false; 

        public Vector3 originalWorldPosition;

        // Start is called before the first frame update
        void Start()
        {
            stepManager = GetComponentInParent<CAS_StepManager>(); 

            //originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
            //currentOriginalPosition = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            float step = speed * Time.deltaTime;
            float stepPush = speedPush * Time.deltaTime;
            if (moveToFirstLayer)
            {
                if (Vector3.Distance(transform.localPosition, Vector3.zero) <= Vector3.Distance(originalPosition, Vector3.zero) * 0.5f)
                {
                    moveToFirstLayer = false;
                }
                else
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, step);
                    currentOriginalPosition = transform.localPosition;
                }
            }

            if (moveToOriginalLayer)
            {
                if (Vector3.Distance(transform.localPosition, Vector3.zero) >= Vector3.Distance(originalPosition, Vector3.zero))
                {
                    moveToOriginalLayer = false;
                }
                else
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, step);
                    currentOriginalPosition = transform.localPosition;
                }
            }

            //New Layer system

            if (layerChangingForward)
            {
                stepManager.SetTrueScale();

                transform.position = Vector3.MoveTowards(transform.position, originalWorldPosition, step);

                //if (gameObject.name == "ArtificialMCA")
                //    Debug.Log(Vector3.Distance(previousPosition, originalPosition) + " " + step + " " + previousPosition + " " + transform.position);

                if (Vector3.Distance(transform.position, originalWorldPosition) < step && Vector3.Distance(transform.position, originalWorldPosition) > (step * -1f)) layerChangingForward = false; 
                
                stepManager.SetOriginalScale();

                originalPosition = transform.localPosition;
                currentOriginalPosition = transform.localPosition;
            }

            if (layerChangingBackward)
            {
                stepManager.SetTrueScale();

                transform.position = Vector3.MoveTowards(transform.position, originalWorldPosition, step);

                //if (gameObject.name == "ArtificialMCA")
                //    Debug.Log(Vector3.Distance(previousPosition, originalPosition) + " " + step + " " + previousPosition + " " + transform.position);

                if (Vector3.Distance(transform.position, originalWorldPosition) < step && Vector3.Distance(transform.position, originalWorldPosition) > (step * -1f)) layerChangingBackward = false;

                stepManager.SetOriginalScale();

                originalPosition = transform.localPosition;
                currentOriginalPosition = transform.localPosition;
            }

            if (layerChangingInitial)
            {
                stepManager.SetTrueScale();
                
                if (Vector3.Distance(transform.position, Vector3.zero) >= Vector3.Distance(originalWorldPosition, Vector3.zero))
                {
                    layerChangingInitial = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, originalWorldPosition, step);
                }
                stepManager.SetOriginalScale();

                originalPosition = transform.localPosition; 
                currentOriginalPosition = transform.localPosition;
            }

            //Pushing 
            if (pushingToOriginalPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, stepPush);

                if (Vector3.Distance(transform.localPosition, originalPosition) < stepPush && Vector3.Distance(transform.localPosition, originalPosition) > (stepPush * -1f)) pushingToOriginalPosition = false;
            }
        }

        public void MoveModelToFirstLayer()
        {
            
            moveToFirstLayer = true; 
        }

        public void MoveModelToOriginalLayer()
        {
            Debug.Log(gameObject.name);
            moveToOriginalLayer = true;
        }

        public void PushToOriginalPosition()
        {
            pushingToOriginalPosition = true; 

            //transform.localPosition = currentOriginalPosition;
            transform.localRotation = originalRotation;
        }

        public void SetStepOriginalPositionMoving(Vector3 tobePosition, LayerChangeType layerChangeType)
        {
            if(layerChangeType == LayerChangeType.Initial)
            {
                layerChangingInitial = true;
                originalWorldPosition = tobePosition;
            }
            else if(layerChangeType == LayerChangeType.Backward)
            {
                layerChangingBackward = true;
                originalWorldPosition = tobePosition;
            }
            else if(layerChangeType == LayerChangeType.Forward)
            {
                layerChangingForward = true;
                originalWorldPosition = tobePosition;
            }      
        }
    }
}
