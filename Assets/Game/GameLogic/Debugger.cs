using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    Board board;
    GameMode gamemode;
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
            board.RemoveTopPieceFromSlot((0,0));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            board.PrintAllPieces();
        }

    }
}
