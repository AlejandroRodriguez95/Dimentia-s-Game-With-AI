using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece
{
    protected E_PieceType pieceType;

    public virtual E_PieceType PieceType
    {
        get { return pieceType; }
    }
    public abstract Piece Clone();
}
