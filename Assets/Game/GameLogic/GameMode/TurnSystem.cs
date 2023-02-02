using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem
{
    Player P1;
    Player P2;
    Player opponent;
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
                    //Debug.Log(currentPlayer.PlayerName);
                    return true;
                }
                return false;

            case E_TurnStages.MovePlayer:
                if (board.MovePlayer(ref currentPlayer, selectedSlot))
                {
                    currentStage = E_TurnStages.GameOverCheck1;
                    Play(board, selectedSlot, ref currentStage, ref currentPlayer);
                    board.DeselectPiece();
                    return true;
                }
                return false;

            case E_TurnStages.GameOverCheck1:
                if (currentPlayer == P1)
                {
                    opponent = P2;
                }
                else
                {
                    opponent = P1;
                }
                // perform check

                if (board.ScanPiecesInRangeOfPlayer(currentPlayer) && board.PlayerIsTrapped(opponent) && currentPlayer.PlayerHasNotReachedGoal()) // there is a valid move && opponent is not trapped
                {
                    currentStage = E_TurnStages.SelectPiece;
                }
                else
                {
                    board.MovePlayer(ref currentPlayer, selectedSlot);
                    currentStage = E_TurnStages.GameOver;
                    GameManager.OnGameOver += GameOver;
                }
                return true;

            case E_TurnStages.SelectPiece:
                if (board.SelectPiece(selectedSlot, true))
                {
                    currentStage = E_TurnStages.MovePiece;
                    board.ScanLegalMoves(2, currentPlayer.PlayerPosOnBoard);
                    return true;
                }
                return false;

            case E_TurnStages.MovePiece:
                if (board.MovePiece(selectedSlot))
                {
                    currentStage = E_TurnStages.GameOverCheck2;
                    Play(board, selectedSlot, ref currentStage, ref currentPlayer);
                    board.DeselectPiece();
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
                if (currentPlayer == P1)
                {
                    opponent = P2;
                }
                else
                {
                    opponent = P1;
                }

                if (board.PlayerIsTrapped(opponent)) // there is a valid move && opponent is not trapped
                {
                    currentStage = E_TurnStages.SelectPiece;
                }
                else
                {
                    board.MovePlayer(ref currentPlayer, selectedSlot);
                    currentStage = E_TurnStages.GameOver;
                    GameManager.OnGameOver += GameOver;
                }

                if (currentPlayer == P1)
                {
                    currentPlayer = P2;
                }

                else
                {
                    currentPlayer = P1;
                }

                // perform check

                


                currentStage = E_TurnStages.TurnStart;
                //Debug.Log(currentStage.ToString());
                return true;

            case E_TurnStages.GameOver:

                return true;

        }

        return true;
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
    }
}
