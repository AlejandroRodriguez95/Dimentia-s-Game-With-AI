using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerVsPlayer : GameMode
{
    //protected Player currentPlayer;
    //protected E_TurnStages currentTurnStage = E_TurnStages.MovePlayer;
    //protected bool waitingForInput;
    //protected TurnSystem turnSystem;


    public PlayerVsPlayer(Board board, Player p1, Player p2) : base(board, p1, p2)
    {

        currentPlayer = P1; // starting player, must be randomized later
        waitingForInput = true;
    }

    public override void Play((int,int) slot)
    {
        turnSystem.Play(board, slot, ref currentTurnStage, ref currentPlayer);
    }


}
