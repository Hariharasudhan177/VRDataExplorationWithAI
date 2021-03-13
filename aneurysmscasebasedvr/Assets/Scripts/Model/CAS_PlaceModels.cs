using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    /// <summary>
    /// Class which loads and places models at the positions 
    /// Models are placed on vertices of a semi-spherical wireframe 
    /// Semi-Sphere is cut through by several planes provides many small circles 
    /// Models are placed along the circumference of these small circles  
    /// All the models are at a equal distance from the origin  
    /// </summary>
    [ExecuteInEditMode]
    public class CAS_PlaceModels : MonoBehaviour
    {
        /// <summary>
        /// Total angle - 360 Degree for sphere 
        /// Small circles along the semi-sphere have total angle (Vertical direction)  
        /// </summary>
        public float totalAngleOfSphere = 180f;

        /// <summary>
        /// Adjustment angle - to shape the sphere
        /// Zero for semi sphere
        /// </summary>
        public float adjustmentAngle = 10f;

        /// <summary>
        /// Radius of the sphere which the distance each model is from the origin 
        /// Hidden as checking the la
        /// </summary>
        [Tooltip("Radius of the sphere")]
        public float radiusOfTheSphere;

        /// <summary>
        /// Number of models at the first small circle 
        /// </summary>
        [Tooltip("Number of models to be placed at the first small circle")]
        public float numberOfModelsAtFirstCircle;

        /// <summary>
        /// Distance between each small circles 
        /// </summary>
        [Tooltip("Distance at which each small circles origin is to be placed")]
        public float distanceBetweenSmallCircles;

        /// <summary>
        /// Adjustment Distance - to shape the sphere
        /// Zero for sphere
        /// </summary>
        public float adjustmentDistance = 1f;

        public Material material;
        float limitSize;

        /// <summary>
        /// Picks every child models and places them in spherical wireframe
        /// </summary>
        public void PickAndPlace()
        {
            transform.GetComponentInParent<CAS_StepManager>().SetTrueScale(); 

            material = GetComponentInParent<CAS_StepManager>().defaultMaterial;
            limitSize = GetComponentInParent<CAS_StepManager>().limitSize;
            Dictionary<string, Vector3> positionOfTheModels = new Dictionary<string, Vector3>(); 

            //Vector3 originalScale = transform.parent.localScale; 
            //transform.parent.localScale = new Vector3(1f, 1f, 1f); 
            //Validate values provided 
            bool validation = ValidateValues();
            if (!validation) return;

            //As each small circle radius would decrease so would the distance
            //So the number of models will be reduced at each small circle 
            float numberOfModelsAtEachCircle = numberOfModelsAtFirstCircle;

            //Distance will be increased as we progress through the small circles  
            float currentDistanceBetweenEachSmallCircle = distanceBetweenSmallCircles;

            //Radius of the inner circle calculated using the pythogoras theorem 
            float radiusOfSmallCircle = PythagoreanTheorem(radiusOfTheSphere, currentDistanceBetweenEachSmallCircle);

            float eachAngle = totalAngleOfSphere / (numberOfModelsAtEachCircle);
            float angleRequired = 0f;
            int index = 0;
            int circleIndex = 0;

            //Looping through each child attached to the parent model object 
            foreach (Transform child in transform)
            {
                //Prepare model 
                if (!child.GetComponent<CAS_PrepareModels>())
                {
                    child.gameObject.AddComponent<CAS_PrepareModels>();
                    child.gameObject.GetComponent<CAS_PrepareModels>().Prepare(child.gameObject, material, limitSize);
                }

                if (!child.GetComponent<CAS_ContolModel>())
                {
                    child.gameObject.AddComponent<CAS_ContolModel>();
                }

                //Centre of the small circle when z-value is the distance between small circles 
                Vector3 centreOfSmallCircle = new Vector3(0f, 0.05f, currentDistanceBetweenEachSmallCircle);

                //Position where model is to be placed 
                Vector3 toBePosition = PointOnCircle(radiusOfSmallCircle, angleRequired, centreOfSmallCircle);
                
                /* Mesh is formed with vertices and triangles. So need to find the centre to position the mesh according to our needs
                 The centre is found using Mesh bounds */
                //Mesh bounds 
                Bounds meshBounds = child.GetComponentInChildren<MeshRenderer>().bounds;

                //child.transform.position = toBePosition;
                child.GetComponent<CAS_ContolModel>().SetPosition(toBePosition); 

                //Not sure of this line - not useful for cubes but needed for meshes as the mesh is away from centre will be adjusted in the parent 
                child.GetChild(0).transform.localPosition = Vector3.zero - meshBounds.center;

                //child.gameObject.GetComponent<CAS_ContolModel>().SetStepOriginalPositionMoving(toBePosition, layerChangeType); 

                index++;
                angleRequired = (circleIndex * adjustmentAngle) + (eachAngle * index);

                //First Circle completed 
                if (index > numberOfModelsAtEachCircle)
                {
                    circleIndex++;
                    index = 0;
                    numberOfModelsAtEachCircle--;
                    eachAngle = totalAngleOfSphere / numberOfModelsAtEachCircle;
                    angleRequired = (circleIndex * adjustmentAngle);
                    currentDistanceBetweenEachSmallCircle += (distanceBetweenSmallCircles - (circleIndex * adjustmentDistance));
                    //Debug.Log(currentDistanceBetweenEachSmallCircle + " " + distanceBetweenSmallCircles + " " + circleIndex + " " + adjustmentDistance + " " + transform.GetComponentsInChildren<CAS_PrepareModels>().Length); 
                    if (currentDistanceBetweenEachSmallCircle > radiusOfTheSphere)
                    {
                        //Infuture we can provide the values required by calculating the remainging 
                        Debug.LogError("Not enough radius to accomodate the required number of circles at the given distance between each small circles. Increase radius or decrease distance");
                        return;
                    }

                    radiusOfSmallCircle = PythagoreanTheorem(radiusOfTheSphere, currentDistanceBetweenEachSmallCircle);
                    //Debug.Log(radiusOfSmallCircle); 
                }
            }
            transform.GetComponentInParent<CAS_StepManager>().SetOriginalScale();
        }

        /// <summary>
        /// Gives the point on the sphere by providing the radius and horizontal angle. Vertical angle is zero
        /// </summary>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="horizontalAngle">Horizontal angle</param>
        /// <param name="origin">Origin of the sphere</param>
        /// <returns>Points on the sphere</returns>
        public Vector3 PointOnCircle(float radius, float horizontalAngle, Vector3 origin)
        {
            float x = (float)(radius * Mathf.Cos(ConvertToRadians(horizontalAngle))) + origin.x;

            float y = (float)(radius * Mathf.Sin(ConvertToRadians(horizontalAngle))) + origin.y;

            float z = 0f + origin.z;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Converts from degrees to radians via multiplication by PI/180 
        /// </summary>
        /// <param name="degree">Angle in degree</param>
        /// <returns>Converted radian value from degree</returns>
        private float ConvertToRadians(float degree)
        {
            return degree * (Mathf.PI / 180f);
        }

        /// <summary>
        /// Pythagorean Theorem
        /// </summary>
        /// <param name="radiusOfSphere"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private float PythagoreanTheorem(float radius, float distance)
        {
            return Mathf.Sqrt((radius * radius) - (distance * distance));
        }

        /// <summary>
        /// Only numberOfModelsAtFirstCircle is validated for now 
        /// </summary>
        /// <returns></returns>
        public bool ValidateValues()
        {
            int numberOfChild = transform.childCount;
            float nn = numberOfModelsAtFirstCircle + 1; 
            //Total models that could be accumulated from the number of models at first circle provided 
            float numberOfModelsAccomodatable = (nn * (nn + 1)) / 2;
            if (numberOfModelsAccomodatable < numberOfChild)
            {
                Debug.LogError("Models cannot be accomodated with the provided number of models at first circle. Value should be increased");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Temporary update to avoid deleting objects and placing it again with required changes 
        /// </summary>
        public void TempUpdate()
        {
            //Looping through each child attached to the parent model object 
            /*foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<CAS_ContolModel>(); 
            }*/
        }
    }
}
