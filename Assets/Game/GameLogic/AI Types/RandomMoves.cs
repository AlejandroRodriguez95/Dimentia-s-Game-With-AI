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




    public override bool GenerateMove(ref (int,int) AImove, E_TurnStages turnstage) // should generate a move for the Play() method
    {
        int radius = 0;
        int random = 0;

        switch (turnstage)
        {
            case E_TurnStages.TurnStart:
                AImove = player.PlayerPosOnBoard;
                return true;

            case E_TurnStages.MovePlayer:
                radius = 1;
                legalMoves = boardReference.ReturnLegalMovesForPlayer(player, radius);

                if (legalMoves.Count == 0)
                    return false;

                random = Random.Range(0, legalMoves.Count);
                AImove = legalMoves[random];

                return true;

            case E_TurnStages.SelectPiece:
                piecesInRange = boardReference.ReturnPiecesInRangeOfPlayer(player);

                random = Random.Range(0, piecesInRange.Count);
                AImove = piecesInRange[random];
                boardReference.SelectPiece(AImove, true);

                break;

            case E_TurnStages.MovePiece:
                radius = 2;
                legalMoves = boardReference.ReturnLegalMovesForPlayer(player, radius);
                random = Random.Range(0, piecesInRange.Count);

                if (legalMoves.Count == 0)
                    return false;

                random = Random.Range(0, legalMoves.Count);
                AImove = legalMoves[random];

                return true;

            default:
                Debug.Log("Warning! this shouldn't happen");
                break;
        }
        
        return true;
    }
}
