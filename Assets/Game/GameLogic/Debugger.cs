using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    Board board;
    GameMode gamemode;

    Player p1;
    Player p2;

    TurnSystem turnSystem;
    void Start()
    {
        board = new Board();
        gamemode = new PlayerVsPlayer(board);



        turnSystem = new TurnSystem();

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.F1))
        {
            board.AddPieceToSlot((2, 2), new PlayerPiece());
            p1 = new Player(E_PlayerType.Player, "Alejandro");
            p1.PlayerPosOnBoard = (2, 2);
            board.SelectPlayer(p1);

        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            board.PrintLegalMovesForPlayer(p1);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            board.MovePlayer(p1, (1, 1));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            board.AddPieceToSlot((1, 1), new Tower());
            board.AddPieceToSlot((1, 2), new Tower());
            board.AddPieceToSlot((1, 3), new Tower());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            board.ScanPiecesInRangeOfPlayer(p1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            board.PrintAllPieces();
        }

    }
}
