using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which loads and places models at the positions 
/// Models are places on vertices of a spherical wireframe (quarter)
/// Vertices are calcualted by placing the centre of the sphere at origin 
/// Distance as radius and angles between radius 
/// </summary>
[ExecuteInEditMode]
public class PlaceModelsClinder : MonoBehaviour
{

    /// <summary>
    /// Total angle - 180 Degree for semi sphere 
    /// </summary>
    private float totalAngle = 180f;

    /// <summary>
    /// Radius of the sphere which the distance each model is from the origin 
    /// </summary>
    [Range(0.0f, 100.0f)]
    public float radius;

    /// <summary>
    /// Number of rows - decided based on number of colomns 
    /// </summary>
    private float numberOfRows;

    /// <summary>
    /// Number of coloumns  
    /// </summary>
    [Range(0.0f, 100.0f)]
    public float numberOfColoumns;

    /// <summary>
    /// Decides the height between each row 
    /// </summary>
    [Range(10f, 180.0f)]
    public float verticalAngle;

    /// <summary>
    /// Distance between each row 
    /// </summary>
    public float height;

    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Picks every child models and places them in spherical wireframe
    /// </summary>
    public void PickAndPlace()
    {
        int horizontalIndex = 0;
        int verticalIndex = 0;

        // Angle which determines the distance between each model horizontally
        float eachAngleHorizontal = 90f;
        if (numberOfColoumns > 1)
        {
            eachAngleHorizontal = totalAngle / (numberOfColoumns - 1);
        }

        int totalNumberOfModels = transform.childCount;
        numberOfRows = Mathf.Ceil(totalNumberOfModels / numberOfColoumns);

        //Looping through each child attached to the parent model object 
        foreach (Transform child in transform)
        {
            float requiredAngleHorizontal = horizontalIndex * eachAngleHorizontal;

            //Position where model is to be placed 
            Vector3 toBePosition = PointOnCylinder(radius, requiredAngleHorizontal, new Vector3(0f, 0f, 0f), verticalIndex * height);

            /* Mesh is formed with vertices and triangles. So need to find the centre to position the mesh according to our needs
             The centre is found using Mesh bounds */
            //Mesh bounds 
            Bounds meshBounds = child.GetComponentInChildren<MeshFilter>().sharedMesh.bounds;
            //Offset by which the model is to be placed correctly 
            child.transform.position = toBePosition - meshBounds.center;

            if (horizontalIndex < numberOfColoumns - 1)
            {
                horizontalIndex++;
            }
            else
            {
                horizontalIndex = 0;
                verticalIndex++;
            }
        }
    }

    public static Vector3 PointOnCylinder(float radius, float horizontalAngle, Vector3 origin, float height)
    {
        return PointOnCircle(radius, horizontalAngle, new Vector3(0f, height, 0f));
    }

    /// <summary>
    /// Gives the point on the sphere by providing the radius and horizontal angle. Vertical angle is zero
    /// </summary>
    /// <param name="radius">Radius of the sphere</param>
    /// <param name="horizontalAngle">Horizontal angle</param>
    /// <param name="origin">Origin of the sphere</param>
    /// <returns>Points on the sphere</returns>
    public static Vector3 PointOnCircle(float radius, float horizontalAngle, Vector3 origin)
    {
        float x = (float)(radius * Mathf.Cos(ConvertToRadians(horizontalAngle))) + origin.x;

        float y = 0f + origin.y;

        float z = (float)(radius * Mathf.Sin(ConvertToRadians(horizontalAngle))) + origin.z;

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Converts from degrees to radians via multiplication by PI/180 
    /// </summary>
    /// <param name="degree">Angle in degree</param>
    /// <returns>Converted radian value from degree</returns>
    private static float ConvertToRadians(float degree)
    {
        return degree * (Mathf.PI / 180f);
    }
}
