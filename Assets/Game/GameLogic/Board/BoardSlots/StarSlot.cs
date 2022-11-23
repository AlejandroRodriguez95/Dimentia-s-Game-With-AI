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
    }
    public override void PrintType()
    {
        Debug.Log("S");
    }
}