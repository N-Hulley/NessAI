using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using ChessDotNet;

public class Caster : MonoBehaviour
{
    public Material SelectedMaterial;
    public Material Highlighted;
    public Material MovePossible;
    public Material MovePossibleHighlighted;
    public Material BadMaterial;

    public static Caster Instance;
    Camera camera;

    public LayerMask ChessLayer;


    public TextMeshProUGUI status;

    public GamePiece SelectedPiece;
    public List<Cell> HoverHistory = new List<Cell>();
    public List<Cell> PossibleMoves = new List<Cell>();

    public Cell Hovering;


    private void Awake()
    {
        Instance = this;
    }
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
        foreach (Cell cell in HoverHistory)
        {
            if (PossibleMoves.Contains(cell))
            {
                cell.ChangeMaterial(MovePossible);
            }
            else
            {

                cell.RevertMaterial();
            }
        }
        if (Physics.Raycast(ray, out hit, ChessLayer))
        {
            GameObject objectHit = hit.transform.gameObject;
            Cell currentCell = objectHit.GetComponent<Cell>();
            if (currentCell == null) return;    
            {

                HoverHistory.Clear();
                if (!currentCell.Equals(SelectedPiece))
                {
                    HoverHistory.Add(currentCell);

                    if (PossibleMoves.Contains(currentCell))
                    {
                        currentCell.ChangeMaterial(MovePossibleHighlighted);

                    }
                    else
                    {
                        currentCell.ChangeMaterial(Highlighted);
                    }
                }

            }

            //if (objectHit.name.Equals(previousHit))
            //{

            //} else
            //{
            //if (previousObject != null)
            //{ 
            //    if (!previousObject.GetComponent<Renderer>().material.name.Equals(SelectedMaterial.name)) previousObject.GetComponent<Renderer>().material = previousMaterial;
            //}
            //previousHit = objectHit.name;
            //previousMaterial = objectHit.GetComponent<Renderer>().material;
            //objectHit.GetComponent<Renderer>().material = Highlighted;
            //previousObject = objectHit;
            //}

            if (Input.GetMouseButtonDown(0))
            {
                GamePiece current = currentCell.gamePiece;
                Log("Clicked on " + currentCell.name + " (" + (current != null ? current.type : "empty") + ")");

                if (SelectedPiece != null && PossibleMoves.Contains(currentCell))
                {
                    Log("<b>Moving " + SelectedPiece.position.ToString() + " to " + currentCell.name + "</b>");
                    Move move = new Move(SelectedPiece.position, new Position(currentCell.name), ChessManager.Instance.current);
                    ChessManager.MakeMove(move);
                    SelectedPiece.Tile.RevertMaterial();
                    SelectedPiece.MoveTo(currentCell);
                    SelectedPiece = null;
                    ResetPossibleMoves();

                }
                else
                {
                    if (currentCell.gamePiece != null && ClickedOtherTeam(currentCell.gamePiece)) {
                        StartCoroutine(FlashBad(currentCell, .2f));
                    }

                    else
                        try
                        {
                            if (current != null)
                            {
                                if (SelectedPiece != null) SelectedPiece.Tile.RevertMaterial();
                                SelectedPiece = current;
                                SelectedPiece.Tile.ChangeMaterial(SelectedMaterial);
                                ResetPossibleMoves();
                                Log("Searching possible moves...");

                                foreach (Move move in ChessManager.Instance.possibleMoves(SelectedPiece.position))
                                {
                                    Log(" - Possible Move: " + move.NewPosition);
                                    Cell Related = ChessManager.Instance.board.transform.Find(move.NewPosition.ToString()).GetComponent<Cell>();
                                    PossibleMoves.Add(Related);
                                    Related.ChangeMaterial(MovePossible);

                                };

                            }
                        }
                        catch (Exception e)
                        {
                            Log(e.Message);
                        }
                }



            }
        }
        else
        {

        }
    }
    bool ClickedOtherTeam(GamePiece clicked)
    {
        if (clicked.black)
        {
            if (ChessManager.BlackTurn)
            {
                return false;
            }
            Log("Wrong team");
            return true;
        }
        if (ChessManager.BlackTurn)
        {
            Log("Wrong Team");
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
    public static void Log(string message)
    {
        Instance.status.text += "\n<color=#FFFFFF55>" + System.DateTime.Now.ToString("H:mm:ss.f") + "</color>   " + message;
    }
    public void logBoard()
    {
        if (SelectedPiece != null) Log("\nSelected:  " + SelectedPiece.position.ToString());

        Log(ChessManager.BoardToString());

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


/*
 * 
                
                Log("<color=\"blue\">Sending '<b>" + objectHit.name + "</b>' to server</color>");
                Dictionary<string, string> values = new Dictionary<string, string>
                {
                { "message", objectHit.name },
                };
                
                server.Send(values);
                
                objectHit.GetComponent<Renderer>().material = SelectedMaterial;
                void DisplayResponse(object sender, ResponseEventArgs e)
    {
       Log("<color=\"green\">Recieved  '" + e.Message + "' from server</color>");

    }
*/
