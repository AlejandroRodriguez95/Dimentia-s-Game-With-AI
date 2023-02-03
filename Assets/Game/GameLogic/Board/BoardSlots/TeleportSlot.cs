using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSlot : BoardSlot
{
    (int, int) target;
    public TeleportSlot((int, int) target) : base()
    {
        maximumAmountOfPiecesInSlot = 1;
        list = new Piece[maximumAmountOfPiecesInSlot];
        boardSlotType = E_BoardSlotType.Teleport;
        this.target = (target.Item1, target.Item2);
    }

    public (int,int) Target
    {
        get { return target; }
    }

    public override bool CheckPieceFitsSlot(Piece piece)
    {
        if (piece.PieceType == E_PieceType.PlayerPiece)
            return true;
        
        return false;
    }



    public override string PrintType()
    {
        return "T";
    }
}
