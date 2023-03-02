using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVsAI : GameMode
{
    private float delayBetweenMoves;
    AI aI;
    private (int, int) aIMove;

    public PlayerVsAI(Board board, Player p1, Player p2, float delayBetweenMoves) : base(board, p1, p2)
    {
        currentPlayer = P1;
        waitingForInput = true;
        this.delayBetweenMoves = delayBetweenMoves;
        aI = p2.AI;
    }

    public (int, int) AIMove
    {
        get { return aIMove; }
    }


    public override bool Play((int, int) slot, ref E_TurnStages turnStageReference)
    {
        if (currentPlayer == P1)
        {
            //Debug.Log($"Play called from {this.GetType()}");
            return turnSystem.Play(board, slot, ref turnStageReference, ref currentPlayer);
        }

        else
        {
            aI.GenerateMove(ref aIMove, turnStageReference);

            //Debug.Log(aIMove);

            return turnSystem.Play(board, aIMove, ref turnStageReference, ref currentPlayer);
        }
    }

}
