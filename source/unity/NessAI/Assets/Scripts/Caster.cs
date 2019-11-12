using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster : MonoBehaviour
{
    public Material SelectedMaterial;
    Camera camera;
    public LayerMask ChessLayer;
    string previousHit;
    GameObject previousObject;
    Material previousMaterial;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;   
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, ChessLayer))
        {
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.name.Equals(previousHit))
            {

            } else
            {
                if (previousObject != null)
                { 
                    previousObject.GetComponent<Renderer>().material = previousMaterial;
                }
                previousHit = objectHit.name;
                previousMaterial = objectHit.GetComponent<Renderer>().material;
                objectHit.GetComponent<Renderer>().material = SelectedMaterial;
                previousObject = objectHit;
            }

            if (Input.GetMouseButtonDown(0)) {
                Debug.Log(objectHit.name);
            }
        } else
        {
            if (previousObject != null)
            {
                previousObject.GetComponent<Renderer>().material = previousMaterial;
                previousHit = null;
                previousObject = null;
                previousMaterial = null;
            }
        }
    }
}
