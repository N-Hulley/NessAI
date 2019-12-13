using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessDotNet;
using UnityEditor;
using UnityEngine.EventSystems;

public class GamePiece : MonoBehaviour
{ 

    public ChessDotNet.Piece piece;
    public ChessDotNet.Position position;
    public Cell Tile;
    [HideInInspector]
    public Material InitialMaterial;

    static Vector3 topLeft = new Vector3(5.97731f, -2.202016f, 2.97286f);
    static float xChange = 1;
    static float yChange = 1; 

    private void Awake()
    {
        
    }

    public bool black = false;
    public string type = "p";

    // Start is called before the first frame update
    void Start()
    {
        InitialMaterial = GetComponent<Renderer>().material;
        
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveTo(Position p)
    {
        Cell newCell = PieceManager.getFromPosition(p);
        if (newCell.gamePiece != null)
        {
            Destroy(newCell.gamePiece.gameObject);
        }
        newCell.gamePiece = this;
        Tile.gamePiece = null;
        Tile = newCell;
        position = Tile.pos;
        UpdatePosition();
    }
    public void UpdatePosition()
    {

        transform.position = Tile.transform.position;
        transform.parent = null;
        transform.position = new Vector3(transform.position.x, Tile.transform.position.y + Tile.transform.lossyScale.y / 2, transform.position.z);
        

    }
}
