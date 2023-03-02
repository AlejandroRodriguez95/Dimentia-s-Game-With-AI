using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem
{
    Player P1;
    Player P2;
    Player opponent;
    private bool gameOver;
    public static List<SuccessfulMove> allMoves;

    (int, int) tempPlayerFrom;
    (int, int) tempPlayerTo;
    (int, int) tempPieceFrom;
    (int, int) tempPieceTo;
    int tempPlayerNumber;
    char tempPieceType;

    public TurnSystem(Player p1, Player p2)
    {
        P1 = p1;
        P2 = p2;
        gameOver = false;
        allMoves = new List<SuccessfulMove>();
    }



    public bool Play(Board board, (int, int) selectedSlot, ref E_TurnStages currentStage, ref Player currentPlayer)
    {
        switch (currentStage)
        {
            case E_TurnStages.TurnStart:

                if (board.SelectPlayer(currentPlayer))
                {
                    tempPlayerFrom = currentPlayer.PlayerPosOnBoard;
                    if(currentPlayer == P1) tempPlayerNumber = 1;
                    else tempPlayerNumber = 2;
                    currentStage = E_TurnStages.MovePlayer;
                    //Debug.Log(currentPlayer.PlayerName);
                    return true;
                }
                return false;

            case E_TurnStages.MovePlayer:
                if (board.MovePlayer(ref currentPlayer, selectedSlot))
                {
                    tempPlayerTo = selectedSlot;
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
                    currentStage = E_TurnStages.SelectPiece;
                    GameManager.winner = (currentPlayer == P2) ? 2 : 1;
                    GameManager.OnGameOver += GameOver;
                    gameOver = true;

                }
                return true;

            case E_TurnStages.SelectPiece:

                if (gameOver)
                {
                    currentStage = E_TurnStages.GameOver;
                    return true;
                }
                if (board.SelectPiece(selectedSlot, true))
                {
                    //Debug.Log("Working!");
                    tempPieceType = board.GetSelectedPieceType();
                    tempPieceFrom = selectedSlot;
                    currentStage = E_TurnStages.MovePiece;
                    board.ScanLegalMoves(2, currentPlayer.PlayerPosOnBoard);
                    return true;
                }

                return false;

            case E_TurnStages.MovePiece:
                if (board.MovePiece(selectedSlot))
                {
                    currentStage = E_TurnStages.GameOverCheck2;
                    tempPieceTo = selectedSlot;
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
                    currentStage = E_TurnStages.TurnStart;
                }
                else
                {
                    board.MovePlayer(ref currentPlayer, selectedSlot);
                    currentStage = E_TurnStages.TurnStart;
                    gameOver = true;
                    GameManager.OnGameOver += GameOver;
                    if (currentPlayer == P2)
                        GameManager.winner = 2;
                    else
                        GameManager.winner = 1;
                    return true;
                }

                if (currentPlayer == P1)
                {
                    currentPlayer = P2;
                }

                else
                {
                    currentPlayer = P1;
                }

                SuccessfulMove newMove = new();
                newMove.playerFrom = tempPlayerFrom;
                newMove.playerTo = tempPlayerTo;
                newMove.playerNumber = tempPlayerNumber;
                newMove.selectedPieceType = tempPieceType;
                newMove.selectedPieceFrom = tempPieceFrom;
                newMove.selectedPieceTo = tempPieceTo;

                allMoves.Add(newMove);

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
        int dummy = 99;
    }
}
