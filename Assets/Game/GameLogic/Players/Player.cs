using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    string playerName;
    double coins;
    E_PlayerType playerType;
    (int, int) playerPosOnBoard;

    public (int,int) PlayerPosOnBoard
    {
        get { return playerPosOnBoard; }
        set { playerPosOnBoard = value; }
    }

    public string PlayerName
    {
        get { return playerName; }
    }

    public Player(E_PlayerType type, string playerName)
    {
        playerType = type;
        this.playerName = playerName;
    }
}
