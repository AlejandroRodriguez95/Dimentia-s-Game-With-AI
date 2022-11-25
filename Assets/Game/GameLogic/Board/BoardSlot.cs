using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlot
{
    protected int maximumAmountOfPiecesInSlot;
    protected bool playerCanStepOver;
    protected bool objectsCanBePutOver;
    protected bool isEmpty;



    // In this array we put all the pieces inside of a slot.
    // The index indicates the current next empty slot
    protected Piece[] list;
    protected int currentListIndex;

    public BoardSlot()
    {
        currentListIndex = 0;
    }


    public Piece[] GetPieces()
    {
        return list;
    }

    public bool AddPiece(Piece piece)
    {
        if (currentListIndex == maximumAmountOfPiecesInSlot)
            return false;

        list[currentListIndex] = piece;
        currentListIndex++;
        return true;
    }

   public bool RemovePiece()
    {
        if (currentListIndex == 0)
            return false;


        currentListIndex--;
        list[currentListIndex] = null;
        return true;
    }





    #region Debugging

    public virtual string PrintType()
    {
        return "None";
    }

    public virtual void PrintPiecesInSlot()
    {
        for(int i=0; i<currentListIndex; i++)
        {
            Debug.Log(list[i].ToString());
        }
    }

    #endregion
}
