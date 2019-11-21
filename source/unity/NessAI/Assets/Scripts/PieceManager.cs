using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessDotNet;
public class PieceManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> piecePrefabs;

    List<GamePiece> piecesOnBoard = new List<GamePiece>();
    public bool boardChanged;


    public IDictionary<string, GamePiece> Pieces = new Dictionary<string, GamePiece>();

    public void AddPieceToBoard(ChessDotNet.Piece piece, Position p)
    {

        if (piece == null) return;

        try
        {
            GamePiece newPiece = 
                Instantiate<GameObject>(
                    Pieces[piece.GetFenCharacter().ToString()].gameObject, 
                    ChessManager.Instance.board.transform.Find(p.ToString()))
                .GetComponent<GamePiece>();

            newPiece.piece = piece;
            newPiece.position = p;

            newPiece.Tile = ChessManager.Instance.board.transform.Find(p.ToString()).GetComponent<Cell>();
            newPiece.Tile.gamePiece = newPiece;
        
            if (piece.GetFenCharacter().Equals(piece.GetFenCharacter().ToString().ToUpper()[0])) transform.Rotate(Vector3.up, 180);

            newPiece.UpdatePosition();
            piecesOnBoard.Add(newPiece.GetComponent<GamePiece>());
            Debug.Log("<b>" + piece.GetFenCharacter() + " added at </b>" + p.ToString());
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError(piece.GetFenCharacter() + " not added");
        }

    }

    private void Awake()
    {
        for (int i = 0; i < piecePrefabs.Count; i++)
        {
            GamePiece p = piecePrefabs[i].GetComponent<GamePiece>();
            Pieces.Add(p.black ? p.type.ToUpper() : p.type.ToLower(), p);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (boardChanged)
        {

        }
    }
}
