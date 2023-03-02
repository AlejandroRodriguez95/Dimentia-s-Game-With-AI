using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ICloneable
{
    string playerName;
    double coins;
    E_PlayerType playerType;

    private AI aI;
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

    public AI AI
    {
        get { return aI; }
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
        switch (type)
        {
            case E_PlayerType.Player:
                //Do nothing
                break;
            case E_PlayerType.AI_Random:
                //initiate AI player
                aI = new RandomMoves(GameManager.board, this);
                break;
            case E_PlayerType.AI_Smart:
                //initiate AI player
                aI = new Smart(GameManager.board, this);
                break;
        }
    }

    public object Clone()
    {
        Player clone = new Player(playerType, playerName, goal);
        clone.coins = coins;
        clone.PlayerPosOnBoard = PlayerPosOnBoard;
        return clone;
    }

}
