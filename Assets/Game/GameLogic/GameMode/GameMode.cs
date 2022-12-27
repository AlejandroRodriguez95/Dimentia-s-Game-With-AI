using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    BoardSlot[,] board;
    Player P1;
    Player P2;

    Player currentPlayer;

    public GameMode(Board board)
    {
        this.board = board.GetBoard();
    }





    public virtual bool CheckIfMoveIsValid((int, int) from, (int, int) to)
    {
        return true;
    }

    public virtual void MovePiece((int, int) from, (int, int) to)
    {
        
    }

    public virtual void MovePlayer(Player player, (int,int) to)
    {

    }


    public virtual void Test()
    {
        board[0, 0] = new NormalSlot();
    }

}
