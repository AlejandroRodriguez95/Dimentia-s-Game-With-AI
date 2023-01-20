using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerVsPlayer : GameMode
{
    //protected Player currentPlayer;
    //protected E_TurnStages currentTurnStage = E_TurnStages.MovePlayer;
    //protected bool waitingForInput;
    //protected TurnSystem turnSystem;


    public PlayerVsPlayer(Board board) : base(board)
    {
        P1 = new Player(E_PlayerType.Player, "Player 1");
        P2 = new Player(E_PlayerType.Player, "Player 2");

        currentPlayer = P1; // starting player, must be randomized later
        waitingForInput = true;
    }

    public void Play(Board board, (int,int) slot)
    {
        turnSystem.Play(board, slot, currentTurnStage, currentPlayer);
    }



}
