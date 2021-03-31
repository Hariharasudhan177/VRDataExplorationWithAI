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
        float speedMoveInitial = 10f;
        float speedMoveSorting = 2f;
        float speedMoveToLayer = 6f;
        float speedPush = 10f;

        Vector3 originalLocalPosition;
        Vector3 toBeWorldPosition;
        Vector3 initialWorldPosition;

        Quaternion originalRotation;

        Color initialOriginalColour;
        Color currentOriginalColor;

        int layerIndex;

        bool pushingToOriginalPosition = false;
        bool movingInitial = false;
        bool movingSorting = false;
        bool moving = false;

        //string colourInMaterialName = "_Edgecolor"; 
        string colourInMaterialName = "_Color";

        bool moveForSimilartiy = false;
        double similartiy = 0f;
        Vector3 similartiyPosition;
        //1 - mostInteresting; 2 - lessInteresting; 3 - notInteresting;
        int interest = 2;

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

                if (Vector3.Distance(transform.localPosition, originalLocalPosition) < 0.01f && Vector3.Distance(transform.localPosition, originalLocalPosition) > (-0.01f)) pushingToOriginalPosition = false;
            }

            //MovingInitial 
            float stepInitialMove = speedMoveInitial * Time.deltaTime;
            if (movingInitial)
            {
                //stepManager.SetTrueScale();
                if (Vector3.Distance(transform.position, toBeWorldPosition) < 0.01f && Vector3.Distance(transform.position, toBeWorldPosition) > (-0.01f))
                {
                    movingInitial = false;
                    GetComponent<CAS_GrabInteractable>().enabled = true;
                    GetComponent<CAS_PrepareModels>().SetInitialMovement(false);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, toBeWorldPosition, stepInitialMove);
                    originalLocalPosition = transform.localPosition;
                }
                //stepManager.SetOriginalScale(); 
            }

            //MovingSorting 
            float stepSortingMove = speedMoveSorting * Time.deltaTime;
            if (movingSorting)
            {
                //stepManager.SetTrueScale();
                if (Vector3.Distance(transform.position, toBeWorldPosition) < 0.01f && Vector3.Distance(transform.position, toBeWorldPosition) > (-0.01f))
                {
                    movingSorting = false;
                    GetComponent<CAS_GrabInteractable>().enabled = true;
                    GetComponent<CAS_PrepareModels>().SetInitialMovement(false);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, toBeWorldPosition, stepSortingMove);
                    originalLocalPosition = transform.localPosition;
                }
                //stepManager.SetOriginalScale(); 
            }

            //Moving 
            float stepMoveToLayer = speedMoveToLayer * Time.deltaTime;
            if (moving)
            {
                //stepManager.SetTrueScale();
                if (Vector3.Distance(transform.position, toBeWorldPosition) < 0.01f && Vector3.Distance(transform.position, toBeWorldPosition) > (-0.01f))
                {
                    moving = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, toBeWorldPosition, stepMoveToLayer);
                    originalLocalPosition = transform.localPosition;
                }
                //stepManager.SetOriginalScale(); 
            }

            if (moveForSimilartiy)
            {
                //stepManager.SetTrueScale();
                if (Vector3.Distance(transform.position, similartiyPosition) < 0.01f && Vector3.Distance(transform.position, similartiyPosition) > (-0.01f))
                {
                    moveForSimilartiy = false;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, similartiyPosition, stepMoveToLayer);
                    originalLocalPosition = transform.localPosition;
                }
                //stepManager.SetOriginalScale();
            }
        }

        public void SetPosition(Vector3 position)
        {
            toBeWorldPosition = position;
            initialWorldPosition = toBeWorldPosition;
            movingInitial = true;
            GetComponent<CAS_PrepareModels>().SetInitialMovement(true);
        }

        public void SetPositionSorting(Vector3 position)
        {
            toBeWorldPosition = position;
            initialWorldPosition = toBeWorldPosition;
            movingSorting = true;
            GetComponent<CAS_PrepareModels>().SetInitialMovement(true);
        }

        public Vector3 GetPosition()
        {
            return initialWorldPosition;
        }

        public void ChangeLayer()
        {
            //stepManager.SetTrueScale();
            //should use something like backward step index 
            layerIndex = GetComponentInParent<CAS_EachStepManager>().stepIndex;
            toBeWorldPosition = LerpWithoutClamp(Vector3.zero, initialWorldPosition, 1f + (layerIndex * 1f));
            moving = true;
            //stepManager.SetOriginalScale();
        }

        public void PushToOriginalPosition()
        {
            pushingToOriginalPosition = true;
            //transform.localRotation = originalRotation;
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

        public void ChangeMaterialForSwitch(Material material, string materialColourName)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
            colourInMaterialName = materialColourName;
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, currentOriginalColor);
        }

        public void SetSimilarity(double value, int interestStatus)
        {
            similartiy = value;
            interest = interestStatus;
        }

        public double GetSimilarity()
        {
            return similartiy;
        }

        //Similarity values already set. So now active this setting by having a visualise button in the tab. Switch on off in the smilartis script written today 
        public void ActivateSimilartiySettings(Vector3 position, Color similartiyColor)
        {
            moving = false;
            similartiyPosition = position;
            moveForSimilartiy = true;
            currentOriginalColor = similartiyColor;
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, currentOriginalColor);
        }

        public void DeActivateSimilartiySettings()
        {
            similartiyPosition = toBeWorldPosition;
            moveForSimilartiy = false;
            moving = true;
            currentOriginalColor = initialOriginalColour;
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, currentOriginalColor);
        }

        public bool GetModelMovingStatus()
        {
            return moving;
        }
    }
}
