using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI
{
    protected Board boardReference;
    protected List<(int, int)> legalMoves;
    protected Player player;

    public AI(Board boardReference, Player player)
    {
        this.player = player;
        this.boardReference = boardReference;
        legalMoves = new List<(int, int)>();
    }

    public abstract bool GenerateMove(ref (int,int) AImove, E_TurnStages turnstage);
}
