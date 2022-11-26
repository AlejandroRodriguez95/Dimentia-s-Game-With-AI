using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSlot : BoardSlot
{
    public TeleportSlot() : base()
    {
        maximumAmountOfPiecesInSlot = 1;
        list = new Piece[maximumAmountOfPiecesInSlot];
        boardSlotType = E_BoardSlotType.Teleport;
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
