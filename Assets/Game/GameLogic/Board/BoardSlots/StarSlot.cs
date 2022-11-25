using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSlot : BoardSlot
{
    // Constructor
    public StarSlot()
    {
        maximumAmountOfPiecesInSlot = 4;
        playerCanStepOver = true;
        objectsCanBePutOver = true;
        list = new Piece[maximumAmountOfPiecesInSlot];
    }
    public override string PrintType()
    {
        return "S";
    }
}
