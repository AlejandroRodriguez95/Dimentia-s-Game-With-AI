using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : ICloneable
{
    BoardSlot[,] board;
    
    
    public Piece selectedPiece;
    public (int, int) selectedSlot;
    List<(int, int)> legalMoves; // for selected piece

    List<(int, int)> AILegalMoves; // for selected AI
    List<(int, int)> AILegalPieceMoves; // for selected AI
    List<(int, int)> enemyLegalMoves;
    List<(int, int)> piecesInRangeOfPlayer;

    PlayerPiece playerPiece; // dummy piece for checking if player fits slot

    public BoardSlot GetBoardSlotType((int,int) pos)
    {
        return board[pos.Item1, pos.Item2];
    }

    public Piece[] GetBoardSlotPieces((int, int) pos)
    {
        return board[pos.Item1, pos.Item2].GetPieces();
    }

    public BoardSlot[,] GetBoard()
    {
        return board;
    }

    public List<(int,int)> legalMovesForAI
    {
        get { return AILegalMoves; }
    }

    /// <Summary>
    /// Fills the board with slots of type BoardSlot
    /// The board looks like this:
    /// 
    ///         5x0     5x1     5x2     5x3
    ///         4x0     4x1     4x2     4x3
    ///         3x0     3x1     3x2     3x3
    ///         2x0     2x1     2x2     2x3
    ///         1x0     1x1     1x2     1x3
    ///         0x0     0x1     0x2     0x3
    /// 
    /// </Summary>

    public Board()
    {
        board = new BoardSlot[4, 6];

        board[0, 0] = new StarSlot();
        board[0, 1] = new NormalSlot();
        board[0, 2] = new NormalSlot();
        board[0, 3] = new NormalSlot();
        board[0, 4] = new NormalSlot();
        board[0, 5] = new TeleportSlot((3, 0));

        board[1, 0] = new NormalSlot();
        board[1, 1] = new NormalSlot();
        board[1, 2] = new NormalSlot();
        board[1, 3] = new NormalSlot();
        board[1, 4] = new NormalSlot();
        board[1, 5] = new NormalSlot();

        board[2, 0] = new NormalSlot();
        board[2, 1] = new NormalSlot();
        board[2, 2] = new NormalSlot();
        board[2, 3] = new NormalSlot();
        board[2, 4] = new NormalSlot();
        board[2, 5] = new NormalSlot();

        board[3, 0] = new TeleportSlot((0, 5));
        board[3, 1] = new NormalSlot();
        board[3, 2] = new NormalSlot();
        board[3, 3] = new NormalSlot();
        board[3, 4] = new NormalSlot();
        board[3, 5] = new StarSlot();

        piecesInRangeOfPlayer = new List<(int, int)>();
        legalMoves = new List<(int, int)>();
        playerPiece = new PlayerPiece();
        AILegalMoves = new List<(int, int)>();
        AILegalPieceMoves = new List<(int, int)>();
        enemyLegalMoves = new List<(int, int)>();
    }


    public bool AddPieceToSlot((int,int) slot, Piece pieceToAdd)
    {
        return board[slot.Item1, slot.Item2].AddPiece(pieceToAdd);
    }

    public bool RemoveTopPieceFromSlot((int,int) slot)
    {
        return board[slot.Item1, slot.Item2].RemovePiece();
    }

    public char GetSelectedPieceType()
    {
        switch (selectedPiece.PieceType)
        {
            case E_PieceType.Tower:
                return 'T';
            case E_PieceType.Pillow:
                return 'P';
        }
        return 'F';
    }

    public bool MovePlayer(ref Player player, (int,int) to)
    {
        //if (selectedPiece.PieceType != E_PieceType.PlayerPiece)
        //    return false;

        if (legalMoves.Count == 0)
            return false;

        if (!legalMoves.Contains(to))
            return false;

        if (board[to.Item1, to.Item2] is TeleportSlot teleportSlot)
            to = teleportSlot.Target; // should've used an event, instead. With this, we also have to manually change the view!


        //Debug.Log(player.PlayerPosOnBoard);
        board[player.PlayerPosOnBoard.Item1, player.PlayerPosOnBoard.Item2].RemovePiece();
        board[to.Item1, to.Item2].AddPiece(selectedPiece);


        player.PlayerPosOnBoard = to;

        //DeselectPiece();
        legalMoves.Clear();

        //ScanPiecesInRangeOfPlayer(player);

        return true;
    }

    public bool AIMovePlayer(ref Player player, (int, int) to)
    {
        //if (selectedPiece.PieceType != E_PieceType.PlayerPiece)
        //    return false;


        if (board[to.Item1, to.Item2] is TeleportSlot teleportSlot)
            to = teleportSlot.Target; // should've used an event, instead. With this, we also have to manually change the view!

        board[player.PlayerPosOnBoard.Item1, player.PlayerPosOnBoard.Item2].RemovePiece();
        board[to.Item1, to.Item2].AddPiece(selectedPiece);

        //Debug.Log($"AIMovePlayer has moved {selectedPiece} to {to}");

        player.PlayerPosOnBoard = to;

        return true;
    }



    public bool MovePiece((int, int) to)
    {
        if (selectedPiece.PieceType == E_PieceType.PlayerPiece)
            return false;

        if (!legalMoves.Contains(to))
            return false;

        bool useNextPiece = false;

        if (board[selectedSlot.Item1, selectedSlot.Item2].SlotContainsPlayer)
            useNextPiece = true;


        if (useNextPiece)
        {
            board[selectedSlot.Item1, selectedSlot.Item2].RemoveNextPiece();
            board[to.Item1, to.Item2].AddPiece(selectedPiece);

            //Debug.Log($"Moved {selectedPiece} from {selectedSlot} to {to} using RemoveNextPiece()");
        }
        else
        {
            board[selectedSlot.Item1, selectedSlot.Item2].RemovePiece();
            board[to.Item1, to.Item2].AddPiece(selectedPiece);
            //Debug.Log($"Moved {selectedPiece} from {selectedSlot} to {to}");
        }

        //DeselectPiece();
        return true;
    }    
    public bool AIMovePiece((int, int) to, bool useNextPiece)
    {
        if(useNextPiece)
        {
            board[selectedSlot.Item1, selectedSlot.Item2].RemoveNextPiece();
            board[to.Item1, to.Item2].AddPiece(selectedPiece);

            //Debug.Log($"Moved {selectedPiece} from {selectedSlot} to {to} using RemoveNextPiece()");
        }
        else
        {
            board[selectedSlot.Item1, selectedSlot.Item2].RemovePiece();
            board[to.Item1, to.Item2].AddPiece(selectedPiece);
            //Debug.Log($"Moved {selectedPiece} from {selectedSlot} to {to}");
        }

        //Debug.Log($"moved piece to {to}");
        //DeselectPiece();
        return true;
    }

    public bool SelectPiece((int, int) slot, bool ignorePlayers)
    {
        selectedPiece = board[slot.Item1, slot.Item2].GetTopPiece();

        if(selectedPiece != null)
        {
            if(ignorePlayers && selectedPiece.PieceType == E_PieceType.PlayerPiece)
            {
                selectedPiece = board[slot.Item1, slot.Item2].GetNextPiece();
            }
        }

        if (piecesInRangeOfPlayer.Count == 0)
            return false;

        if (!piecesInRangeOfPlayer.Contains(slot))
            return false;

        selectedSlot = slot;
        //PrintSelectedPiece();

        

        if (selectedPiece == null)
        {
            selectedSlot = (10, 10);
            return false;
        }
        else
        {
            return true;
        }
    }   
    public bool AISelectPiece((int, int) slot, bool ignorePlayers) // return value: true means next piece must be used, false means continue with the top piece
    {
        bool nextPieceUsed = false;
        selectedPiece = board[slot.Item1, slot.Item2].GetTopPiece();


        if(selectedPiece != null)
        {
            if(ignorePlayers && selectedPiece.PieceType == E_PieceType.PlayerPiece)
            {
                selectedPiece = board[slot.Item1, slot.Item2].GetNextPiece();
                nextPieceUsed = true;
            }
        }

        selectedSlot = slot;

        return nextPieceUsed;
    }

    public bool SelectPlayer(Player toSelect)
    {
        SelectPiece(toSelect.PlayerPosOnBoard, false);

        if (selectedPiece == null)
        {
            selectedSlot = (10, 10);
            return false;
        }
        else
        {
            //Debug.Log($"You have selected {toSelect.PlayerName}");
            ScanLegalMoves(1, toSelect.PlayerPosOnBoard);
            //PrintLegalMovesForPlayer(toSelect);
            return true;
        }
    }    
    public bool AISelectPlayer(Player toSelect)
    {
        AISelectPiece(toSelect.PlayerPosOnBoard, false);
        //PrintPiecesInSlot(toSelect.PlayerPosOnBoard);
        //Debug.Log($"AI has selected {toSelect.PlayerName} at position: {toSelect.PlayerPosOnBoard}");
        return true;
    }

    public void DeselectPiece() // must be called after successfully? moving a piece
    {
        selectedPiece = null;
        selectedSlot = (10,10);

        legalMoves.Clear();
        //Debug.Log("Selected piece has been cleared.");
    }

    public void PrintSelectedPiece()
    {
        Debug.Log($"You have selected: {selectedPiece} at position: {selectedSlot}");
    }

    public List<(int, int)> ReturnLegalMovesForPlayer(Player player, int radius)
    {
        AILegalMoves.Clear();
        var slot = player.PlayerPosOnBoard;
        for (int i = slot.Item1 - radius; i <= slot.Item1 + radius; i++)
        {
            for (int j = slot.Item2 - radius; j <= slot.Item2 + radius; j++)
            {
                if (i < 0 || j < 0 || i > 3 || j > 5) // array size is [3, 5]
                    continue;

                if (board[i, j].CheckPieceFitsSlot(selectedPiece)  && (i, j) != player.PlayerPosOnBoard)
                    AILegalMoves.Add((i, j));
            }
        }

        if (AILegalMoves.Count == 0)
        {
            Debug.LogWarning("AI couldn't find any legal moves!");
            return AILegalMoves;
        }

        return AILegalMoves;
    } // AI method
        public List<(int, int)> ReturnLegalMovesForEnemy(Player player, int radius)
    {
        enemyLegalMoves.Clear();
        var slot = player.PlayerPosOnBoard;
        for (int i = slot.Item1 - radius; i <= slot.Item1 + radius; i++)
        {
            for (int j = slot.Item2 - radius; j <= slot.Item2 + radius; j++)
            {
                if (i < 0 || j < 0 || i > 3 || j > 5) // array size is [3, 5]
                    continue;

                if (board[i, j].CheckPieceFitsSlot(selectedPiece) && (i, j) != player.PlayerPosOnBoard)
                    enemyLegalMoves.Add((i, j));
            }
        }

        if (enemyLegalMoves.Count == 0)
        {
            Debug.LogWarning("AI couldn't find any legal moves!");
            return enemyLegalMoves;
        }

        return enemyLegalMoves;
    } // AI method


    public List<(int, int)> ReturnLegalMovesForPieces(Player player, int radius)
    {
        AILegalPieceMoves.Clear();
        var slot = player.PlayerPosOnBoard;
        for (int i = slot.Item1 - radius; i <= slot.Item1 + radius; i++)
        {
            for (int j = slot.Item2 - radius; j <= slot.Item2 + radius; j++)
            {
                if (i < 0 || j < 0 || i > 3 || j > 5) // array size is [3, 5]
                    continue;

                if (board[i, j].CheckPieceFitsSlot(selectedPiece) && selectedSlot != (i, j))
                    AILegalPieceMoves.Add((i, j));
            }
        }

        if (AILegalMoves.Count == 0)
        {
            Debug.LogWarning("AI couldn't find any legal moves for pieces!");
            return AILegalPieceMoves;
        }

        return AILegalPieceMoves;
    } // AI method

    public bool ScanLegalMoves(int radius, (int, int) slot)
    {
        legalMoves.Clear();

        for(int i=slot.Item1-radius; i <= slot.Item1 + radius; i++)
        {
            for(int j=slot.Item2-radius; j <= slot.Item2 + radius; j++)
            {
                if (i < 0 || j < 0 || i > 3 || j > 5) // array size is [3, 5]
                    continue;

                if (board[i, j].CheckPieceFitsSlot(selectedPiece) && selectedSlot != (i, j))
                    legalMoves.Add((i, j));
            }
        }

        if(legalMoves.Count == 0)
        {
            Debug.LogWarning("No legal moves! game over");
            return false;
        }

        return true;
        
    }

    public int AmountOfLegalMovesForPlayer(Player player)
    {
        int amountOfMoves = 0;
        var slot = player.PlayerPosOnBoard;
        for (int i = slot.Item1 - 1; i <= slot.Item1 + 1; i++)
        {
            for (int j = slot.Item2 - 1; j <= slot.Item2 + 1; j++)
            {
                if (i < 0 || j < 0 || i > 3 || j > 5) // array size is [3, 5]
                    continue;

                if (board[i, j].CheckPieceFitsSlot(playerPiece) && (i, j) != player.PlayerPosOnBoard)
                    amountOfMoves++;
            }
        }

        return amountOfMoves;
    }

    public bool ScanPiecesInRangeOfPlayer(Player player)
    {
        piecesInRangeOfPlayer.Clear();



        for (int i = player.PlayerPosOnBoard.Item1 - 1; i <= player.PlayerPosOnBoard.Item1 + 1; i++)
        {
            for (int j = player.PlayerPosOnBoard.Item2 - 1; j <= player.PlayerPosOnBoard.Item2 + 1; j++)
            {
                if (i < 0 || j < 0 || i > 3 || j > 5) // array size is [3, 5]
                    continue;

                var tempPiece = board[i, j].GetTopPiece();

                if (tempPiece == null)
                    continue;

                if(tempPiece.PieceType == E_PieceType.PlayerPiece)
                {
                    tempPiece = board[i, j].GetNextPiece();
                    if (tempPiece == null)
                        continue;
                }
                if (tempPiece.PieceType != E_PieceType.PlayerPiece)
                {
                    //Debug.Log($"{i}, {j}");
                    piecesInRangeOfPlayer.Add((i, j));
                }         
            }
        }

        //SelectPlayer(player);

        if (piecesInRangeOfPlayer.Count == 0)
        {
            //Debug.LogWarning("No pieces in range! game over");
            return false;
        }

        return true;
    }

    public List<(int,int)> ReturnPiecesInRangeOfPlayer(Player player)
    {
        ScanPiecesInRangeOfPlayer(player);

        var newList = new List<(int, int)>(piecesInRangeOfPlayer);
        return newList;
    }


    public bool PlayerIsTrapped(Player player)
    {
        for (int i = player.PlayerPosOnBoard.Item1 - 1; i <= player.PlayerPosOnBoard.Item1 + 1; i++)
        {
            for (int j = player.PlayerPosOnBoard.Item2 - 1; j <= player.PlayerPosOnBoard.Item2 + 1; j++)
            {
                if (i < 0 || j < 0 || i > 3 || j > 5) // array size is [3, 5]
                    continue;

                if (board[i, j].CheckPieceFitsSlot(playerPiece))
                    return true;
            }
        }

        return false;
    }

    public int GetAmountOfPiecesInSlot((int, int) slot)
    {
        return board[slot.Item1, slot.Item2].CurrentListIndex;
    }

    public int GetAmountOfPillowsInSlot((int,int) slot)
    {
        return board[slot.Item1, slot.Item2].CurrentPillowsInSlot;
    }


    #region Debugging
    public void PrintBoard()
    {
        for(int i = board.GetLength(1) - 1; i >= 0; i--)
        {
            Debug.Log($"  X[0]Y[{i}]: {board[0, i].PrintType()}   |   X[1]Y[{i}]: {board[1, i].PrintType()}   |   X[2]Y[{i}]: {board[2, i].PrintType()}   |   X[3]Y[{i}]: {board[3, i].PrintType()}");
        }
    }

    public void PrintAllPieces()
    {
        for(int i = 0; i <= 3; i++)
        {
            for(int j =0; j <= 5; j++)
            {

                Debug.Log($"slot: {i}, {j}");
                board[i,j].PrintPiecesInSlot();
            }
        }
    }

    public void PrintLegalMovesForPlayer(Player player)
    {
        ScanLegalMoves(1, player.PlayerPosOnBoard);
        foreach((int,int) move in legalMoves)
        {
            Debug.Log($"x: {move.Item1} y: {move.Item2}");
        }
    }

    public void PrintPiecesInSlot((int,int) slot)
    {
        board[slot.Item1, slot.Item2].PrintPiecesInSlot();
    }


    public void PrintSlotsThatContainPlayers()
    {
        foreach(BoardSlot slot in board)
        {
            Debug.Log($"slot {slot} = {slot.SlotContainsPlayer}");
        }
    }

    #endregion



    public object Clone()
    {
        Board newBoard = new Board();
        var newBoardSlots = newBoard.GetBoard();



        for(int i=0; i<newBoardSlots.GetLength(0); i++)
        {
            for(int j=0; j<newBoardSlots.GetLength(1); j++)
            {
                if (board[i, j].SlotIsEmpty())
                {
                    continue;
                }

                var piecesToClone = board[i, j].GetPieces();
                
                foreach (var piece in piecesToClone)
                {
                    if (piece == null)
                        continue;

                    var tempPiece = piece.Clone();

                    newBoardSlots[i, j].AddPiece(tempPiece);
                }
            }
        }
        return newBoard;
    }

}
