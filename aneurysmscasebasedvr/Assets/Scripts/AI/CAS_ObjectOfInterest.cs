using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DimBoxes;

namespace CAS
{
    public class CAS_ObjectOfInterest : MonoBehaviour
    {
        CAS_AIManager aIManager;
        // Start is called before the first frame update

        //string colourInMaterialName = "_Edgecolor"; 
        string colourInMaterialName = "_Color";

        Color initialOriginalColour;
        Color currentOriginalColor;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Initialize()
        {
            if (aIManager == null) aIManager = GetComponentInParent<CAS_AIManager>();

            gameObject.AddComponent<CAS_PrepareModels>();
            GetComponent<CAS_PrepareModels>().Prepare(gameObject, aIManager.manager.stepManager.defaultMaterial, aIManager.manager.stepManager.limitSize, aIManager.manager.stepManager.boundingBoxLineMaterial);

            transform.position = aIManager.objectOfInterestToBeTransform.position;

            GetComponentInChildren<BoundBox>().enabled = true;
            GetComponentInChildren<BoundBox>().wireColor = Color.red;

            Bounds meshBounds = transform.GetComponentInChildren<MeshRenderer>().bounds;
            transform.GetChild(0).transform.localPosition = transform.localPosition - meshBounds.center;

            initialOriginalColour = transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor(colourInMaterialName);
            currentOriginalColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor(colourInMaterialName);
        }

        public void Highlight(Color highlightedColor)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, highlightedColor);
        }

        public void DeHighlight()
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, currentOriginalColor);
        }

        public void ChangeMaterialForSwitch(Material material, string materialColourName)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
            colourInMaterialName = materialColourName;
            transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(colourInMaterialName, currentOriginalColor);
        }
    }
}