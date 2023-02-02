using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSlot
{
    protected int maximumAmountOfPiecesInSlot;
    protected bool slotContainsPlayer;
    protected bool slotContainsTower;
    protected E_BoardSlotType boardSlotType;

    protected int currentPillowsInSlot;

    // In this array we put all the pieces inside of a slot.
    // The index indicates the current next empty slot
    protected Piece[] list;
    protected int currentListIndex;

    public BoardSlot()
    {
        currentListIndex = 0;
    }

    public E_BoardSlotType BoardSlotType
    {
        get { return boardSlotType; }
    }

    public Piece[] GetPieces()
    {
        return list; // list will always have size 4
    }

    public Piece GetTopPiece()
    {
        if (currentListIndex > 0)
            return list[currentListIndex - 1];
        else 
            return null;
    }

    public Piece GetNextPiece()
    {
        if (currentListIndex - 2 > 0)
            return list[currentListIndex - 2];
        else
            return null;
    }

    public bool AddPiece(Piece piece)
    {
        //Slot is full?
        if (SlotIsFull())
            return false;


        list[currentListIndex] = piece;
        

        // Manage local variables
        switch (piece.PieceType)
        {
            case E_PieceType.Tower:
                slotContainsTower = true;
                break;
            case E_PieceType.Pillow:
                currentPillowsInSlot++;
                break;
            case E_PieceType.PlayerPiece:
                slotContainsPlayer = true;
                break;
        }

        currentListIndex++;
        return true;
    }

   public bool RemovePiece() 
    {
        if (SlotIsEmpty())
            return false;


        currentListIndex--;

        // Manage local variables
        switch (list[currentListIndex].PieceType)
        {
            case E_PieceType.Tower:
                slotContainsTower = false;
                break;
            case E_PieceType.Pillow:
                currentPillowsInSlot--;
                break;
            case E_PieceType.PlayerPiece:
                slotContainsPlayer = false;
                break;
        }

        list[currentListIndex] = null;
        return true;
    }


    public virtual bool CheckPieceFitsSlot(Piece piece)
    {
        return true;
    }


    public bool SlotIsFull()
    {
        if (currentListIndex == maximumAmountOfPiecesInSlot)
            return true;

        return false;
    }

    public bool SlotIsEmpty()
    {
        if (currentListIndex == 0)
            return true;

        return false;
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
