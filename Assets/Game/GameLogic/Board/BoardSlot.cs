using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlot
{
    protected int maximumAmountOfPiecesInSlot;
    protected bool playerCanStepOver;
    protected bool objectsCanBePutOver;

    protected Piece[] list;

    public Piece[] GetPieces()
    {
        return list;
    }

    public virtual string PrintType()
    {
        return "None";
    }
}
