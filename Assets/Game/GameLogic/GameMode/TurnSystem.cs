using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem
{
    Player P1;
    Player P2;
    public TurnSystem(Player p1, Player p2)
    {
        P1 = p1;
        P2 = p2;
    }



    public bool Play(Board board, (int, int) selectedSlot, ref E_TurnStages currentStage, ref Player currentPlayer)
    {
        switch (currentStage)
        {
            case E_TurnStages.TurnStart:
                if (board.SelectPlayer(currentPlayer))
                {
                    currentStage = E_TurnStages.MovePlayer;
                    Debug.Log(currentPlayer.PlayerName);
                    return true;
                }
                return false;

            case E_TurnStages.MovePlayer:
                if (board.MovePlayer(ref currentPlayer, selectedSlot))
                {
                    currentStage = E_TurnStages.GameOverCheck1;
                    board.DeselectPiece();
                    Play(board, selectedSlot, ref currentStage, ref currentPlayer);
                    return true;
                }
                return false;
            
            case E_TurnStages.GameOverCheck1:
                // perform check
                if (board.ScanPiecesInRangeOfPlayer(currentPlayer))
                {
                    currentStage = E_TurnStages.SelectPiece;
                }
                else
                {
                    currentStage = E_TurnStages.GameOver;
                }
                return true;

            case E_TurnStages.SelectPiece:
                if (board.SelectPiece(selectedSlot, true))
                {
                    currentStage = E_TurnStages.MovePiece;
                    return true;
                }
                return false;

            case E_TurnStages.MovePiece:
                if (board.MovePiece(selectedSlot))
                {
                    currentStage = E_TurnStages.GameOverCheck2;
                    board.DeselectPiece();
                    Play(board, selectedSlot, ref currentStage, ref currentPlayer);
                    return true;
                }
                else
                {
                    board.DeselectPiece();
                    currentStage = E_TurnStages.SelectPiece;
                }
                    return false;

            case E_TurnStages.GameOverCheck2:
                //perform check
                if(currentPlayer == P1)
                {
                    currentPlayer = P2;
                }

                else
                {
                    currentPlayer = P1;
                }

                currentStage = E_TurnStages.TurnStart;
                Debug.Log(currentStage.ToString());
                return true;

            case E_TurnStages.GameOver:
                return true;

        }
        return true;
    }
}
