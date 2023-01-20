using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    protected BoardSlot[,] boardSlots;
    protected Player P1;
    protected Player P2;

    protected bool waitingForInput;

    protected Player currentPlayer;
    protected E_TurnStages currentTurnStage;

    protected TurnSystem turnSystem;

    public bool WaitingForInput
    {
        get { return waitingForInput; }
    }


    public GameMode(Board board)
    {
        this.boardSlots = board.GetBoard();
        currentTurnStage = E_TurnStages.MovePlayer;
        turnSystem = new TurnSystem();
    }


}
