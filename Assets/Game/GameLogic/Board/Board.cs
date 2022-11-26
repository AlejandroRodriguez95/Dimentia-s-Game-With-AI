using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board
{
    BoardSlot[,] board;

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
        board[0, 5] = new TeleportSlot();

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

        board[3, 0] = new TeleportSlot();
        board[3, 1] = new NormalSlot();
        board[3, 2] = new NormalSlot();
        board[3, 3] = new NormalSlot();
        board[3, 4] = new NormalSlot();
        board[3, 5] = new StarSlot();
    }


    public bool AddPieceToSlot((int,int) slot, Piece pieceToAdd)
    {
        return board[slot.Item1, slot.Item2].AddPiece(pieceToAdd);
    }

    public bool RemoveTopPieceFromSlot((int,int) slot)
    {
        return board[slot.Item1, slot.Item2].RemovePiece();
    }

    public bool MovePlayer(Player player, (int,int) to)
    {
        player.PlayerPosOnBoard = to;
        // Check then move

        return true;
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
        foreach(BoardSlot slot in board)
        {
            slot.PrintPiecesInSlot();
        }
    }


    #endregion

}
