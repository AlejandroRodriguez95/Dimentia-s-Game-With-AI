using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : Piece
{
    public PlayerPiece() : base()
    {
        pieceType = E_PieceType.PlayerPiece;
    }

    public override Piece Clone()
    {
        return new PlayerPiece();
    }
}
