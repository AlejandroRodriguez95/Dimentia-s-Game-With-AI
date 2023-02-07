using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoves : AI
{
    //Board boardReference;
    //List<(int, int)> legalMoves;
    //Player player;



    public RandomMoves(Board boardReference, Player player) : base(boardReference, player)
    {
        this.boardReference = boardReference;
        this.player = player;
        this.legalMoves = new List<(int, int)>();
    }




    public override bool GenerateMove(ref (int,int) AImove, E_TurnStages turnstage)
    {
        int radius = 0;
        switch (turnstage)
        {
            case E_TurnStages.MovePlayer:
                radius = 1;
                break;

            case E_TurnStages.MovePiece:
                radius = 2;
                break;
        }

        Debug.Log(boardReference);
        legalMoves = boardReference.ReturnLegalMovesForPlayer(player, radius);

        if (legalMoves.Count == 0)
            return false;

        var random = Random.Range(0, legalMoves.Count);
        AImove = legalMoves[random];


        Debug.Log(AImove);
        return true;
    }
}
