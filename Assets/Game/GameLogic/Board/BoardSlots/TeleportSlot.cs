using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSlot : BoardSlot
{
    // Constructor
    public TeleportSlot()
    {
        maximumAmountOfPiecesInSlot = 1;
        playerCanStepOver = true;
        objectsCanBePutOver = false;
        list = new Piece[maximumAmountOfPiecesInSlot];
    }

    public override string PrintType()
    {
        return "T";
    }
}
