using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_ContolModel : MonoBehaviour
    {
        // Adjust the speed for the movement
        public float speed = 1.0f;

        Vector3 originalPosition;
        Vector3 currentOriginalPosition; 
        Quaternion originalRotation;

        bool moveToFirstLayer;
        bool moveToOriginalLayer;

        // Start is called before the first frame update
        void Start()
        {
            originalPosition = transform.localPosition;
            originalRotation = transform.localRotation;
            currentOriginalPosition = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            float step = speed * Time.deltaTime; 
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
            transform.localPosition = currentOriginalPosition;
            transform.localRotation = originalRotation;
        }
    }

}
