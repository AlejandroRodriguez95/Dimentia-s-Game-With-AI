using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AIMoves
{
    public (int, int) playerMove;
    public (int, int) selectedPiece;
    public (int, int) selectedPieceMove;
    public int[] analysis;
    public AIMoves((int, int) pMove, (int, int) sPiece, (int, int) sPieceMove, int[] analysis)
    {
        playerMove = pMove;
        selectedPiece = sPiece;
        selectedPieceMove = sPieceMove;
        this.analysis = analysis;
    }
}

public struct SuccessfulMove
{
    public int playerNumber;
    public (int, int) playerFrom;
    public (int, int) playerTo;
    public char selectedPieceType;
    public (int, int) selectedPieceFrom;
    public (int, int) selectedPieceTo;
}
