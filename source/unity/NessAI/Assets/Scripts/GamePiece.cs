using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessDotNet;
using UnityEditor;

public class GamePiece : MonoBehaviour
{ 

    public ChessDotNet.Piece piece;
    public ChessDotNet.Position position;
    public Cell Tile;


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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveTo(Cell newCell)
    {
        if (newCell.gamePiece != null)
        {
            Destroy(newCell.gamePiece.gameObject);
        }
        newCell.gamePiece = this;
        Tile.gamePiece = null;
        Tile = newCell;

        UpdatePosition();
    }
    public void UpdatePosition()
    {

        transform.position = Tile.transform.position;
        transform.parent = null;
        transform.position = new Vector3(transform.position.x, Tile.transform.position.y + Tile.transform.lossyScale.y / 2, transform.position.z);
        


    }
}
