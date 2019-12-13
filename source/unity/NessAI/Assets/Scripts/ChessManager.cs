using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using ChessDotNet;
using System;

public class ChessManager : MonoBehaviour
{
    public Player current;
    public Player player1;
    public Player player2;
    public GameObject board;
    public static ChessManager Instance;
    public static bool BlackTurn = false;
    public ServerImp server;
    public Transform PlayerRig;
    private void Awake()
    {
        Instance = this;
        current = Game.WhoseTurn;
    }
    public ChessGame Game = new ChessGame();
    public PieceManager PManager;
    private void Start()
    {
        BlackTurn = false;
        Piece[][] Board = Game.GetBoard();
        for (int row = 0; row < Board.Length; row++)
        {
            for (int col = 0; col < Board[row].Length; col++)
            {
                PManager.AddPieceToBoard(Board[row][col], new Position((File)col, row + 1));
            }
        }
        Status.Log("<color=green><b>Finished generating pieces</b></color>");
        server = new ServerImp();
        server.ResponseRecieved += Server_ResponseRecieved;
        //Position 
        //Move move = new Move();
        //Game.MakeMove();
    }

    private void Server_ResponseRecieved(object sender, ResponseEventArgs e)
    {
        if (e.RequestValues.ContainsKey("test"))
        {
            return;
        }
        Status.Log("<color=\"green\">Recieved  '" + e.Message + "' from server</color>");
        Position from = new Position(e.Message[0].ToString() + e.Message[1].ToString());
        Position to = new Position(e.Message[2].ToString() + e.Message[3].ToString());
        Move m = new Move(from, to, current);
        MakeMove(m, false);


    }

    public List<Move> possibleMoves(Position pos)
    {
        List<Move> moves = new List<Move>();
        moves = new List<Move>(Game.GetValidMoves(pos));
        return moves;
    }

    public static void MakeMove(Move move, bool player)
    {
        Status.Log("<b> " + (player? "Player ":"AI") + "Moving " + move.OriginalPosition.ToString() + " to " + move.NewPosition.ToString() + "</b>");

        Instance.Game.MakeMove(move, true);
        Instance.current = Instance.Game.WhoseTurn;
        BlackTurn = !BlackTurn;
        
        if (player)
        {
            Status.Log("<color=\"blue\">Sending </color>'<b>" + Instance.Game.GetFen() + "</b>'");

            Dictionary<string, string> values = new Dictionary<string, string>
            {
            { "board", Instance.Game.GetFen() },
            };

            Instance.server.Send(values);
        }
        PieceManager.getFromPosition(move.OriginalPosition).gamePiece.MoveTo(move.NewPosition);

        

    }
    public static string BoardToString()
    {
        string output = "\n<mspace=1.25em>";
        bool black = false;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                black = !black;
                //output += black ? "<mark=#000000cc>" : "<mark=#FFFFFFcc>";
                try
                {
                    string p = Instance.Game.GetBoard()[i][j].GetFenCharacter().ToString();
                    string add = "<b>" + (p == p.ToUpper() ? "<color=red>" : "<color=blue>") + p + "</color></b>";
                    output += add;
                }
                catch (Exception)
                {
                    output += "X";
                }
                output += " </mark>";
            }
            output += "\n";
        }
        return output + "</mspace>\n" + ChessManager.Instance.Game.GetPGN();
    }
    
}