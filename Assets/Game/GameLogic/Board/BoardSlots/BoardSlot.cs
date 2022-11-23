using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlot
{
    protected int maximumAmountOfPiecesInSlot;
    protected bool playerCanStepOver;
    protected bool objectsCanBePutOver;

    public virtual void PrintType()
    {
    }
}
