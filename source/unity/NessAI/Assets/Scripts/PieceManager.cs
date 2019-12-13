using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessDotNet;
using System;

public class PieceManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> piecePrefabs;

    List<GamePiece> piecesOnBoard = new List<GamePiece>();
 
    [Space]
    [Tooltip("Materials")]
    public Material Highlighted;
    public Material SelectedMaterial;
    public Material MovePossible;
    public Material MovePossibleHighlighted;
    public Material BadMaterial;
    public Material HoverOnOtherTeam;

    [Space]

    public LayerMask ChessLayer;

    [Space]

    public GamePiece SelectedPiece;

    public List<Cell> PossibleMoves = new List<Cell>();


    public IDictionary<string, GamePiece> Pieces = new Dictionary<string, GamePiece>();
    public void PointerClick(Cell cell, bool log = true)
    {
        if (log) Status.Log("Clicked " + cell.name + " (" + (cell.gamePiece != null ? cell.gamePiece.type : "empty") + ")", Status.Importance.SlightlyImportant);

        

        GamePiece current = cell.gamePiece;

        if (SelectedPiece != null && PossibleMoves.Contains(cell))
        {
            if (ServerImp.connected)
            {
                Move move = new Move(SelectedPiece.position, new Position(cell.name), ChessManager.Instance.current);
                SelectedPiece.Tile.PreventColorChange = false;
                SelectedPiece.Tile.RevertMaterial();
                SelectedPiece = null;
                ResetPossibleMoves();
                ChessManager.MakeMove(move, true);
            } else
            {
                Status.Log("<b><color=red>Not connected to server! can't make move</color></b>", Status.Importance.Critical);

                foreach (Cell tile in ChessManager.Instance.board.transform.GetComponentsInChildren(typeof(Cell), false))
                {
                    StartCoroutine(FlashBad(tile, .4f));
                }
            }


        }
        else
        {
            if (cell.gamePiece != null && cell.gamePiece.black)
            {
                StartCoroutine(FlashBad(cell, .3f));
                Status.Log("<b><color=red>Wrong side</color></b>", Status.Importance.SlightlyImportant);

                return;
            }
            try
            {
                if (current != null)
                {
                    if (SelectedPiece != null)
                    {
                        SelectedPiece.Tile.PreventColorChange = false;
                        SelectedPiece.Tile.RevertMaterial();
                    }
                    SelectedPiece = current;
                    SelectedPiece.Tile.ChangeMaterial(SelectedMaterial);
                    SelectedPiece.Tile.PreventColorChange = true;

                    ResetPossibleMoves();
                    Status.Log("Searching possible moves...", Status.Importance.SomewhatImportant);

                    foreach (Move move in ChessManager.Instance.possibleMoves(SelectedPiece.position))
                    {
                        Status.Log(" - Possible Move: " + move.NewPosition, Status.Importance.SlightlyImportant);
                        Cell Related = PieceManager.getFromPosition(move.NewPosition);
                        PossibleMoves.Add(Related);
                        Related.ChangeMaterial(MovePossible);

                    };

                } else
                {
                    StartCoroutine(FlashBad(cell, .3f));
                }
            }
            catch (Exception e)
            {
                Status.Log(e.Message, Status.Importance.Important);

            }
        }
    }   
                    
    public void PointerClick(GamePiece piece, bool log = true)
    {
        if (log) Status.Log("Clicked piece: " + piece.type + " (" + piece.Tile.pos.ToString() + ")", Status.Importance.SlightlyImportant);
        PointerClick(piece.Tile, false);
    }
    public void PointerEnter(Cell cell, bool log = true)
    {
        if (log) Status.Log("Pointer entered " + cell.name + " (" + (cell.gamePiece != null ? cell.gamePiece.type : "empty") + ")", Status.Importance.NotImportant);
        if (ChessManager.BlackTurn)
        {
            return;
        }
        else if (PossibleMoves.Contains(cell))
        {
            cell.ChangeMaterial(MovePossibleHighlighted);
        }
        else if (cell.gamePiece == null)
        {
            cell.ChangeMaterial(Highlighted);
        }
        else
        {
            if (cell.gamePiece.black)
            {
                cell.ChangeMaterial(Highlighted, HoverOnOtherTeam);
            } else
            {
                cell.ChangeMaterial(Highlighted);
            }
        }

    }
    public void PointerLeave(Cell cell, bool log = true)
    {
        if (log) Status.Log("Pointer left " + cell.name + " (" + (cell.gamePiece != null ? cell.gamePiece.type : "empty") + ")", Status.Importance.NotImportant);

        if (PossibleMoves.Contains(cell))
        {
            cell.ChangeMaterial(MovePossible);
        }
        else
        {
            cell.RevertMaterial();
        }
    }
    public void PointerEnter(GamePiece piece, bool log = true)
    {
        if (log) Status.Log("Pointer entered piece: " + piece.type + " (" + piece.Tile.pos.ToString() + ")", Status.Importance.NotImportant);

        PointerEnter(piece.Tile, false);
    }
    public void PointerLeave(GamePiece piece, bool log = true)
    {
        if (log) Status.Log("Pointer left piece: " + piece.type + " (" + piece.Tile.pos.ToString() + ")", Status.Importance.NotImportant);

        PointerLeave(piece.Tile, false);
    }
    public void Hover(GamePiece piece)
    {

        //        GameObject objectHit = hit.transform.gameObject;
        //        Cell currentCell = objectHit.GetComponent<Cell>();
        //        if (currentCell == null)
        //        {
        //            if (objectHit.GetComponent<GamePiece>() != null)
        //            {
        //                currentCell = objectHit.GetComponent<GamePiece>().Tile;
        //            }

        //        }
        //        if (currentCell == null) return;
        //        {

        //            HoverHistory.Clear();
        //            if (!currentCell.Equals(SelectedPiece))
        //            {
        //                HoverHistory.Add(currentCell);

        //                if (PossibleMoves.Contains(currentCell))
        //                {
        //                    currentCell.ChangeMaterial(MovePossibleHighlighted);

        //                }
        //                else
        //                {
        //                    currentCell.ChangeMaterial(Highlighted);
        //                }
        //            }

        //        }

        
        //                    }
        //            }



        //        }
        //    }
        //    else
        //    {

        //    }
        //}
    }


    public static Cell getFromPosition(Position p)
    {
        return ChessManager.Instance.board.transform.Find(p.ToString()).GetComponent<Cell>();
    }

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
            Status.Log("<b>" + piece.GetFenCharacter() + " added at </b>" + p.ToString(), Status.Importance.SlightlyImportant);
        }
        catch (KeyNotFoundException e)
        {
            Status.Log("<b><color=red>" + piece.GetFenCharacter() + " not added </color></b>", Status.Importance.VeryImportant);
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
    public static bool ClickedOtherTeam(GamePiece clicked)
    {
        if (clicked.black)
        {
            if (ChessManager.BlackTurn)
            {
                return false;
            }
            Status.Log("<b><color=red>" + "Wrong team</color></b>", Status.Importance.SlightlyImportant);
            return true;
        }
        if (ChessManager.BlackTurn)
        {
            Status.Log("<b><color=red>" + "Wrong Team</color></b>", Status.Importance.SlightlyImportant);
            return true;
        }
        return false;
    }
    void ResetPossibleMoves()
    {
        foreach (Cell unused in PossibleMoves)
        {
            unused.RevertMaterial();
        }
        PossibleMoves.Clear();
    }
    IEnumerator FlashBad(Cell tile, float time)
    {
        tile.RevertMaterial();
        tile.ChangeMaterial(BadMaterial);
        tile.PreventColorChange = true;

        yield return new WaitForSeconds(time);
        tile.PreventColorChange = false;
        tile.RevertMaterial();
    }
}
