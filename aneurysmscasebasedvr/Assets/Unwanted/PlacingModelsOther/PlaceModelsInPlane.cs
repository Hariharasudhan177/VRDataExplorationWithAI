using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceModelsInPlane : MonoBehaviour
{
    GameObject[] gameObjects;
    public Material material;
    public Transform parent;
    public Transform firstPosition;
    public int numberOfColumns = 5; 

    // Start is called before the first frame update
    void Start()
    {
        gameObjects = Resources.LoadAll<GameObject>("100Datasets");

        int indexVertical = 0;
        int indexHorizontal = 0; 
        foreach (var t in gameObjects)
        {
            //Instantiating the GameObject 
            GameObject model = Instantiate(t, parent);

            //Assigning position 
            model.transform.localPosition = firstPosition.localPosition 
                + (indexHorizontal * new Vector3(100f, 0f, 0f))
                + (indexVertical * new Vector3(0f, -100f, 0f)); 

            //Assigning Material 
            MeshRenderer meshRenderer = model.GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = material;

            //Finding bounds and centre point 
            Bounds meshBounds = model.GetComponentInChildren<MeshFilter>().mesh.bounds;
            Vector3 toBePosition = new Vector3(0f, 0f, 0f);

            //Using localPosition is okay for now 
            //Vector3 offset = model.transform.position - model.transform.TransformPoint(meshBounds.center);
            Vector3 offset = model.transform.localPosition - meshBounds.center;
            model.transform.position = toBePosition + offset;

            //Setting rows and Columns
            indexHorizontal++;
            if(indexHorizontal > numberOfColumns)
            {
                indexHorizontal = 0;
                indexVertical++; 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
