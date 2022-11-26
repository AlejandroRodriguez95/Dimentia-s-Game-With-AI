using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    string playerName;
    double coins;
    E_PlayerType playerType;

    public Player(E_PlayerType type)
    {
        playerType = type;
    }
}
