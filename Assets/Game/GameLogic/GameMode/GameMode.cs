using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode
{
    protected BoardSlot[,] boardSlots;
    protected Board board;
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


    public GameMode(Board board, Player p1, Player p2)
    {
        this.boardSlots = board.GetBoard();
        this.board = board;
        currentTurnStage = E_TurnStages.TurnStart;
        P1 = p1;
        P2 = p2;
        P1.PlayerPosOnBoard = (0, 0);
        P2.PlayerPosOnBoard = (3, 5);
        turnSystem = new TurnSystem(p1, p2);
    }

    public abstract void Play((int, int) slot);
}
