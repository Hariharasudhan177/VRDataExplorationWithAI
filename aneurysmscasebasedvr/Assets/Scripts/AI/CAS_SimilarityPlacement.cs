using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_SimilarityPlacement : MonoBehaviour
    {
        public CAS_SimilarityObject[] objectsToBePlaced;

        // Start is called before the first frame update
        void Start()
        {
            objectsToBePlaced = GetComponentsInChildren<CAS_SimilarityObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PlaceObjects3();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                FindDistance();
            }
        }

        public void PlaceObjects()
        {
            int totalNumberofObjects = objectsToBePlaced.Length - 1;

            float angleRequired = 180 / totalNumberofObjects;
            float radius = 1f;

            int index = 0;


            foreach (CAS_SimilarityObject objectToBePlaced in objectsToBePlaced)
            {
                Debug.Log(Mathf.Cos(angleRequired * index));
                Debug.Log(Mathf.Sin(angleRequired * index));

                //objectToBePlaced.PlaceObject(new Vector3((radius * (objectToBePlaced.similarity * 10f)) * Mathf.Cos((angleRequired * index) * (Mathf.PI / 180)), 0f, (radius * (objectToBePlaced.similarity * 10f)) * Mathf.Sin((angleRequired * index) * (Mathf.PI / 180))));
                index++;
            }
        }

        public void PlaceObjects2()
        {
            int totalNumberofObjects = objectsToBePlaced.Length;

            float angleRequired = 90 / (totalNumberofObjects / 2);

            float radius = 1f;

            int index = 0;
            int leftIndex = 0;
            int rightIndex = 0;

            bool toIncreaseLeft = false;

            foreach (CAS_SimilarityObject objectToBePlaced in objectsToBePlaced)
            {
                float currentAngle = 90f;

                if (toIncreaseLeft)
                {
                    currentAngle -= (leftIndex * angleRequired);
                }
                else
                {
                    currentAngle += (rightIndex * angleRequired);
                }

                Debug.Log(currentAngle);

                //objectToBePlaced.PlaceObject(new Vector3((radius * (objectToBePlaced.similarity * 10f)) * Mathf.Cos((currentAngle) * (Mathf.PI / 180)), 0f, (radius * (objectToBePlaced.similarity * 10f)) * Mathf.Sin((currentAngle) * (Mathf.PI / 180))));
                index++;

                if (toIncreaseLeft)
                {
                    toIncreaseLeft = false;
                    rightIndex++;
                }
                else
                {
                    toIncreaseLeft = true;
                    leftIndex++;
                }
            }
        }

        public void PlaceObjects3()
        {
            int totalNumberofObjects = objectsToBePlaced.Length;

            float angleRequired = 90f / (totalNumberofObjects / 2);

            float radius = 1f;

            int index = 0;
            int leftIndex = 0;
            int rightIndex = 0;

            bool toIncreaseLeft = false;

            foreach (CAS_SimilarityObject objectToBePlaced in objectsToBePlaced)
            {
                float currentAngle = 90f;

                if (toIncreaseLeft)
                {
                    currentAngle -= (leftIndex * angleRequired);
                }
                else
                {
                    currentAngle += (rightIndex * angleRequired);
                }

                float currentVerticalAngle = 360f;
                currentVerticalAngle -= index * (90f / totalNumberofObjects);

                Debug.Log(currentAngle);
                //Debug.Log(currentVerticalAngle);

                float offset;

                if (objectToBePlaced.similarity < 0.25f)
                {
                    offset = 20f;
                }
                else if (objectToBePlaced.similarity < 0.1f)
                {
                    offset = 10f;
                }
                else if (objectToBePlaced.similarity < 2f)
                {
                    offset = 10f;
                }
                else
                {
                    offset = 10f;
                }

                objectToBePlaced.PlaceObject(new Vector3(
                    radius * (objectToBePlaced.similarity * offset) * Mathf.Cos(currentAngle * (Mathf.PI / 180f)),
                    0f,
                    (radius * (objectToBePlaced.similarity * offset)) * Mathf.Sin(currentAngle * (Mathf.PI / 180f))
                ), InterpolateScaleColour(objectToBePlaced.similarity));

                index++;

                if (toIncreaseLeft)
                {
                    toIncreaseLeft = false;
                    rightIndex++;
                }
                else
                {
                    toIncreaseLeft = true;
                    leftIndex++;
                }
            }
        }

        public void FindDistance()
        {
            foreach (CAS_SimilarityObject objectToBePlaced in objectsToBePlaced)
            {
                Debug.Log(Vector3.Distance(objectToBePlaced.transform.position, Vector3.zero));
            }
        }

        public Color InterpolateScaleColour(float similartiy)
        {
            float r = 1f;
            float a = 1f;

            float gMax = 0.95f;
            float gMin = 0.6f;

            float bMax = 0.90f;
            float bMin = 0f;

            float similartiyMax = 2f;
            float similartiyMin = 0f;

            float g = gMax - (((similartiyMax - similartiy) / (similartiyMax - similartiyMin)) * (gMax - gMin));
            float b = bMax - (((similartiyMax - similartiy) / (similartiyMax - similartiyMin)) * (bMax - bMin));

            return new Color(r, g, b, a);

        }
    }
}