using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DimBoxes;

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

        Color initialOriginalColour;
        Color currentOriginalColor;

        int layerIndex;

        bool pushingToOriginalPosition = false; 
        bool moving = false;

        //string colourInMaterialName = "_Edgecolor"; 
        string colourInMaterialName = "_Color"; 

        // Start is called before the first frame update
        void Start()
        {
            initialOriginalColour = transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor(colourInMaterialName);
            currentOriginalColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor(colourInMaterialName);
            
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

        public void SetPosition(Vector3 position)
        {
            toBeWorldPosition = position;
            initialWorldPosition = toBeWorldPosition;
            moving = true;
        }

        public void ChangeLayer()
        {
            stepManager.SetTrueScale();
            //should use something like backward step index 
            layerIndex = GetComponentInParent<CAS_EachStepManager>().stepIndex;
            toBeWorldPosition = LerpWithoutClamp(Vector3.zero, initialWorldPosition, 1f + (layerIndex * 1f));
            moving = true;
            stepManager.SetOriginalScale();
        }

        public void PushToOriginalPosition()
        {
            pushingToOriginalPosition = true; 
            transform.localRotation = originalRotation;
        }

        public void UnSetGroupByColour()
        {
            currentOriginalColor = initialOriginalColour;
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, currentOriginalColor);
        }

        public void SetGroupByColour(Color groupByColour)
        {
            currentOriginalColor = groupByColour;
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, currentOriginalColor);
        }

        public void Highlight(Color highlightedColor)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, highlightedColor);
        }

        public void DeHighlight()
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, currentOriginalColor);
        }

        public void HighlightModelSinceSelected(bool selected)
        {
            transform.GetChild(0).GetComponent<BoundBox>().enabled = selected; 
        }

        Vector3 LerpWithoutClamp(Vector3 originPosition, Vector3 currentPosition, float factor)
        {
            return originPosition + (currentPosition - originPosition) * factor;
        }
    }
}
