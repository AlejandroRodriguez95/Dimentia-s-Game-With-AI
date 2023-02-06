// The type of slots used on the basic board
public enum E_BoardSlotType
{
    Normal,
    Teleport,
    Star
}

public enum E_PlayerType
{
    Player,
    AI_Random,
    AI_Smart
}

public enum E_AIType
{
    AI_random,
    AI_smart,
    none
}

public enum E_PieceType
{
    PlayerPiece,
    Tower,
    Pillow
}
public enum E_Gamemode
{
    PvP,
    PvAI,
    Online,
    Tutorial
}

public enum E_TurnStages
{
    TurnStart, // -> select player piece
    MovePlayer, // -> move player piece to new slot
    GameOverCheck1, // -> check if game over (enemy trapped, no legal moves for the player who moved, reached star)
    SelectPiece, // -> select piece
    MovePiece, // move piece to new slot
    GameOverCheck2, // check if game over (enemy trapped, reached star)
    GameOver // Display gzgzgz
}