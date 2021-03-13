using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_CompareObject : MonoBehaviour
    {
        //Camera compareCamera;

        GameObject comparedModel;
        public CAS_CompareManager compareManager;
        // Start is called before the first frame update
        void Start()
        {
            //compareCamera = GetComponentInChildren<Camera>(); 
        }

        public void ActivateCompareObject(GameObject model)
        {
            if (comparedModel != null)
            {
                Destroy(comparedModel);
            }

            comparedModel = Instantiate(model, transform);

            foreach (var comp in comparedModel.GetComponents<Component>())
            {
                if (!(comp is Transform))
                {
                    if (!(comp is Rigidbody))
                    {
                        Destroy(comp);
                    }
                }
            }
            Destroy(comparedModel.GetComponent<Rigidbody>());

            comparedModel.transform.localPosition = Vector3.zero;
            comparedModel.transform.localScale = new Vector3(10f, 10f, 10f);
            comparedModel.AddComponent<CAS_Rotate>();
            GameObject meshObject = comparedModel.transform.GetChild(0).gameObject;
            Destroy(meshObject.GetComponent<BoxCollider>());
            meshObject.AddComponent<BoxCollider>();
            meshObject.layer = 11;
            comparedModel.GetComponentInChildren<MeshRenderer>().material = compareManager.manager.stepManager.defaultMaterial;

            //if (compareCamera == null) compareCamera = GetComponentInChildren<Camera>();
            //Debug.Log(GetComponentInChildren<BoxCollider>().bounds.center);
            //compareCamera.transform.position = GetComponentInChildren<BoxCollider>().bounds.center + new Vector3(0f, 0f, -8f);
        }

        public void DeActivateCompareObject()
        {
            if (comparedModel != null)
            {
                Destroy(comparedModel);
            }
        }
    }
}