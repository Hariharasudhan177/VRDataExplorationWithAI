using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using CAS;
using UnityEditor; 

namespace CAS
{
    [ExecuteInEditMode]
    public class CAS_PrepareModels : MonoBehaviour
    {
        public void Prepare(GameObject model, Vector3 position, Material material)
        {
            //Assigning Material 
            MeshRenderer meshRenderer = model.GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = material;

            //Attach XR grabbable
            model.AddComponent<Rigidbody>();
            model.GetComponent<Rigidbody>().useGravity = false;
            model.GetComponent<Rigidbody>().isKinematic = true;


            model.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
            //model.gameObject.GetComponent<MeshCollider>().sharedMesh = model.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh; 

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
            if(Vector3.Distance(transform.position, Camera.main.transform.position) < 2f)
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

