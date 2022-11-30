using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    Board board;
    GameMode gamemode;

    Player p1;
    Player p2;
    void Start()
    {
        board = new Board();
        gamemode = new PlayerVsPlayer(board);



    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            board.AddPieceToSlot((0, 0), new PlayerPiece());
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            board.MovePlayer(p1, (0, 1));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            board.PrintAllPieces();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            board.SelectPiece((0, 0), false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            board.SelectPiece((0, 0), true);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            board.SelectPiece((0, 0), false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log(board.GetBoard()[0, 1].CheckPieceFitsSlot(board.selectedPiece));
        }


    }
}
