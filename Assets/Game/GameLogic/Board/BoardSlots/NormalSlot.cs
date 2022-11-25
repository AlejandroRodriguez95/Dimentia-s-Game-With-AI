using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSlot : BoardSlot
{
    // Constructor
    public NormalSlot()
    {
        maximumAmountOfPiecesInSlot = 4;
        playerCanStepOver = true;
        objectsCanBePutOver = true;
        list = new Piece[maximumAmountOfPiecesInSlot];
    }
    public override string PrintType()
    {
        return "N";
    }
}
