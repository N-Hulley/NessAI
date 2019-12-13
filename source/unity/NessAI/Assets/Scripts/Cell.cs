using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessDotNet;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    Renderer r;
    Material InitialMaterial;
    public GamePiece gamePiece;
    public bool PreventColorChange = false;
    public Position pos;
    private void Awake()
    {
        //transform.localRotation = Quaternion.Euler(transform.localRotation.x, Random.Range(-180, 180), transform.localRotation.z);
        //transform.localScale *= Random.Range(.2f, 2.5f);
        r = GetComponent<Renderer>();
        InitialMaterial = r.material;
        pos = new Position(name);
    }
    private void Start()
    {
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { ChessManager.Instance.PManager.PointerEnter(this); });
            GetComponent<EventTrigger>().triggers.Add(entry);
        }
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((eventData) => { ChessManager.Instance.PManager.PointerLeave(this); });
            GetComponent<EventTrigger>().triggers.Add(entry);
        }
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { ChessManager.Instance.PManager.PointerClick(this); });
            GetComponent<EventTrigger>().triggers.Add(entry);
        }
    }
    public void ChangeMaterial(Material newMaterial, Material pieceMaterial = null)
    {
        if (!PreventColorChange)
        {
            if (pieceMaterial == null) pieceMaterial = newMaterial;

            r.material = newMaterial;
            if (gamePiece != null) gamePiece.GetComponent<Renderer>().material = pieceMaterial;
        }
    }
    public void RevertMaterial()
    {
        if (!PreventColorChange)
        {
            r.material = InitialMaterial;
            if (gamePiece != null) gamePiece.GetComponent<Renderer>().material = gamePiece.InitialMaterial;

        }
    }
    
}
