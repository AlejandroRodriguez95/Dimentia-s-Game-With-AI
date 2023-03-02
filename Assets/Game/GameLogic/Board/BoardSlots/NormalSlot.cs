using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSlot : BoardSlot
{
    public NormalSlot() : base()
    {
        currentPillowsInSlot = 0;
        maximumAmountOfPiecesInSlot = 4;
        list = new Piece[maximumAmountOfPiecesInSlot];
        boardSlotType = E_BoardSlotType.Normal;
    }
    public override bool CheckPieceFitsSlot(Piece piece)
    {
        if (SlotIsFull())
            return false;

        switch (piece.PieceType)
        {
            case E_PieceType.PlayerPiece:
                if (slotContainsPlayer || slotContainsTower)
                    return false;
                break;

            case E_PieceType.Tower:
                if (slotContainsPlayer || slotContainsTower)
                    return false;
                break;


            case E_PieceType.Pillow:
                if (currentPillowsInSlot == 3)
                    return false;
                break;
        }

        return true;
    }

    public override string PrintType()
    {
        return "N";
    }

}
