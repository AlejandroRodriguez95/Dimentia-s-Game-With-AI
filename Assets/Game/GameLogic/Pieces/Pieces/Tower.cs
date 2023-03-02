using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Piece
{
    public Tower() : base()
    {
        pieceType = E_PieceType.Tower;
    }

    public override Piece Clone()
    {
        return new Tower();
    }
}
