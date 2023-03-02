using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillow : Piece
{
    public Pillow() : base()
    {
        pieceType = E_PieceType.Pillow;
    }

    public override Piece Clone()
    {
        return new Pillow();
    }
}
