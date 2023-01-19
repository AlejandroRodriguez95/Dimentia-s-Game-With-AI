using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem
{
    public E_TurnStages Play(Board board, (int, int) selectedSlot, E_TurnStages currentStage, Player currentPlayer)
    {
        switch (currentStage)
        {
            case E_TurnStages.TurnStart:
                if (board.SelectPlayer(currentPlayer))
                    return E_TurnStages.MovePlayer;
                break;

            case E_TurnStages.MovePlayer:
                    if (board.MovePlayer(currentPlayer, selectedSlot))
                        return E_TurnStages.MovePiece;
                break;
            
            case E_TurnStages.GameOverCheck1:
                // perform check
                break;

            case E_TurnStages.SelectPiece:
                if (board.SelectPiece(selectedSlot, true))
                    return E_TurnStages.MovePiece;
                break;

            case E_TurnStages.MovePiece:
                if (board.MovePiece(selectedSlot))
                    return E_TurnStages.GameOverCheck2;
                else
                    board.DeselectPiece();
                break;

            case E_TurnStages.GameOverCheck2:
                //perform check
                break;

            case E_TurnStages.GameOver:
                break;
        }

        return currentStage;
    }
}
