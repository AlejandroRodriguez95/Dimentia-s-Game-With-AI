using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Controller controller;

    Board board;
    GameMode gameMode;

    Player p1;
    Player p2;

    TurnSystem turnSystem;

    private void Start()
    {
        p1 = new Player(E_PlayerType.Player, "Alejandro");
        p2 = new Player(E_PlayerType.Player, "P2");

        board = new Board();
        gameMode = new PlayerVsPlayer(board, p1, p2);

        controller.GameModeRef = gameMode;
        controller.board = board;

        // fill board with test pieces:

        board.AddPieceToSlot((0, 0), new PlayerPiece());
        board.AddPieceToSlot((1, 1), new Tower());
        board.AddPieceToSlot((1, 2), new Tower());
        board.AddPieceToSlot((1, 3), new Tower());

        board.AddPieceToSlot((3, 5), new PlayerPiece());


    }

}
