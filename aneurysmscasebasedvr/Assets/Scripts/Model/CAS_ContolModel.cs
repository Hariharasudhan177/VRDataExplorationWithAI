using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_ContolModel : MonoBehaviour
    {
        CAS_StepManager stepManager; 

        // Adjust the speed for the movement - Should be received from step manager 
        float speedMoveToLayer = 0.5f;
        float speedPush= 10f;

        Vector3 originalLocalPosition;
        Vector3 toBeWorldPosition;
        Vector3 initialWorldPosition;

        Quaternion originalRotation;

        Color currentDefaultColor;

        int layerIndex;

        bool pushingToOriginalPosition = false; 
        bool moving = false;
 

        // Start is called before the first frame update
        void Start()
        {
            currentDefaultColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
            
            stepManager = GetComponentInParent<CAS_StepManager>(); 

            originalLocalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
        }

        // Update is called once per frame
        void Update()
        {

            //Pushing 
            float stepPush = speedPush * Time.deltaTime;
            if (pushingToOriginalPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalLocalPosition, stepPush);

                if (Vector3.Distance(transform.localPosition, originalLocalPosition) < stepPush && Vector3.Distance(transform.localPosition, originalLocalPosition) > (stepPush * -1f)) pushingToOriginalPosition = false;
            }

            //Moving 
            float stepMoveToLayer = speedMoveToLayer * Time.deltaTime;
            if (moving)
            {
                stepManager.SetTrueScale();
                if (Vector3.Distance(transform.position, toBeWorldPosition) < stepMoveToLayer && Vector3.Distance(transform.position, toBeWorldPosition) > (stepMoveToLayer * -1f)) 
                {
                    moving = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, toBeWorldPosition, stepMoveToLayer);
                    originalLocalPosition = transform.localPosition;
                }
                stepManager.SetOriginalScale(); 
            }
        }

        public void PushToOriginalPosition()
        {
            pushingToOriginalPosition = true; 
            transform.localRotation = originalRotation;
        }

        public void ChangeLayer()
        {
            stepManager.SetTrueScale(); 
            //should use something like backward step index 
            layerIndex = GetComponentInParent<CAS_EachStepManager>().stepIndex;
            toBeWorldPosition = LerpWithoutClamp(Vector3.zero, initialWorldPosition, 1f + (layerIndex*1f));
            moving = true;
            stepManager.SetOriginalScale();
        }

        public void SetPosition(Vector3 position)
        {
            toBeWorldPosition = position;
            initialWorldPosition = toBeWorldPosition;
            moving = true;
        }

        public void SetDefaultColor(Color _defaultColor)
        {
            currentDefaultColor = _defaultColor;
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", currentDefaultColor);
        }

        public void Highlight(Color currentDefaultColor)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", currentDefaultColor);
        }

        public void DeHighlight()
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", currentDefaultColor);
        }

        Vector3 LerpWithoutClamp(Vector3 originPosition, Vector3 currentPosition, float factor)
        {
            return originPosition + (currentPosition - originPosition) * factor;
        }
    }
}
