using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    string playerName;
    double coins;
    E_PlayerType playerType;
    (int, int) playerPosOnBoard;
    (int, int) goal;

    public (int,int) PlayerPosOnBoard
    {
        get { return playerPosOnBoard; }
        set { playerPosOnBoard = value; }
    }

    public string PlayerName
    {
        get { return playerName; }
    }

    public bool PlayerHasNotReachedGoal()
    {
        if (goal == PlayerPosOnBoard)
            return false;

        return true;
    }

    public Player(E_PlayerType type, string playerName, (int,int) goal)
    {
        playerType = type;
        this.playerName = playerName;
        this.goal = goal;
    }
}
