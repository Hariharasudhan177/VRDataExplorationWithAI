using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using CAS;
using UnityEditor;
using DimBoxes; 

namespace CAS
{
    [ExecuteInEditMode]
    public class CAS_PrepareModels : MonoBehaviour
    {
        public int dataAvailable = -1; 
        public void Prepare(GameObject model, Material material, float limitSize)
        {
            //Reduce scale to fit to limit size 
            MeshFilter meshFilter = model.GetComponentInChildren<MeshFilter>();
            Vector3 sizeOfModel = meshFilter.sharedMesh.bounds.size;
            float toBeReducedPercentage = 1.0f; 
            if (sizeOfModel.x > limitSize)
            {
                if (sizeOfModel.x > sizeOfModel.y && sizeOfModel.x > sizeOfModel.z)
                {
                    toBeReducedPercentage = limitSize / sizeOfModel.x; 
                }
            } 
            
            if(sizeOfModel.y > limitSize)
            {
                if (sizeOfModel.y > sizeOfModel.x && sizeOfModel.y > sizeOfModel.z)
                {
                    toBeReducedPercentage = limitSize / sizeOfModel.y;
                }
            }
            
            if(sizeOfModel.z > limitSize)
            {
                if (sizeOfModel.z > sizeOfModel.x && sizeOfModel.z > sizeOfModel.y)
                {
                    toBeReducedPercentage = limitSize / sizeOfModel.z;
                }
            }

            if(toBeReducedPercentage != 1.0f)
            {
                model.transform.GetChild(0).localScale = new Vector3(toBeReducedPercentage, toBeReducedPercentage, toBeReducedPercentage);
            }

            //Assigning Material 
            MeshRenderer meshRenderer = model.GetComponentInChildren<MeshRenderer>(); 
            meshRenderer.material = material;

            //Attach XR grabbable
            model.AddComponent<Rigidbody>();
            model.GetComponent<Rigidbody>().useGravity = false;
            model.GetComponent<Rigidbody>().isKinematic = true;

            model.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
            model.transform.GetChild(0).gameObject.AddComponent<BoundBox>();
            model.transform.GetChild(0).gameObject.GetComponent<BoundBox>().lineMaterial = GetComponentInParent<CAS_StepManager>().boundingBoxLineMaterial;
            model.transform.GetChild(0).gameObject.GetComponent<BoundBox>().enabled = false; 
            model.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
            model.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().sharedMesh = model.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            model.transform.GetChild(0).gameObject.GetComponent<MeshCollider>().enabled = false;

            model.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().receiveShadows = false;

            model.gameObject.AddComponent<CAS_GrabInteractable>();
            model.GetComponent<CAS_GrabInteractable>().throwOnDetach = false;
        }

        public void Update()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
                SwitchCollider();
#else
            SwitchCollider();
#endif

        }

        /// <summary>
        /// Switch collider based on distance from the user 
        /// Mesh collider when near 
        /// Box collider when far 
        /// </summary>
        void SwitchCollider()
        {
            if(Vector3.Distance(transform.position, Camera.main.transform.position) < 1f)
            {
                transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
                //Need not change in unity game object layer. Just in the cas_interactable(XR) script. Yes it is simple but it doesn't make sense to me too. 
                //gameObject.layer = 8;
                //transform.GetChild(0).gameObject.layer = 8; 
                gameObject.transform.GetComponent<CAS_GrabInteractable>().interactionLayerMask = LayerMask.GetMask("DirectInteractables"); 
            }
            else
            {
                transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
                transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
                //gameObject.layer = 9;
                //transform.GetChild(0).gameObject.layer = 9;
                gameObject.transform.GetComponent<CAS_GrabInteractable>().interactionLayerMask = LayerMask.GetMask("RayInteractables"); 
            }
        }
    }
}

