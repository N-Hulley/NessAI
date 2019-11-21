using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    Renderer r;
    Material InitialMaterial;
    public GamePiece gamePiece;
    public bool PreventColorChange = false;
    private void Awake()
    {
        r = GetComponent<Renderer>();
        InitialMaterial = r.material;
    }
    public void ChangeMaterial(Material newMaterial)
    {
        if (!PreventColorChange)
        r.material = newMaterial;
    }
    public void RevertMaterial()
    {
        if (!PreventColorChange)

            r.material = InitialMaterial;
    }
    
}
