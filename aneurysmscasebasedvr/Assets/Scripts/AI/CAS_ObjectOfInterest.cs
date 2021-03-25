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

        void Start()
        {
            aIManager = GetComponentInParent<CAS_AIManager>();
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
        }
    }
}