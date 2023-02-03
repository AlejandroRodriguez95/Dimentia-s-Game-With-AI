using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void OnGameOverEvent();
    public static event OnGameOverEvent OnGameOver;

    [SerializeField] Controller controller;
    [SerializeField] GameObject[] boardView;
    [SerializeField] GameObject piecesContainer;

    Board board;
    GameMode gameMode;

    Player p1;
    Player p2;

    [SerializeField] GameObject Tower;
    [SerializeField] GameObject Pillow;
    [SerializeField] GameObject Player1Piece;
    [SerializeField] GameObject Player2Piece;

    TurnSystem turnSystem;





    private void Start()
    {
        p1 = new Player(E_PlayerType.Player, "Alejandro", (0,5));
        p2 = new Player(E_PlayerType.Player, "P2", (0, 0));

        board = new Board();
        gameMode = new PlayerVsPlayer(board, p1, p2);

        controller.GameModeRef = gameMode;
        controller.board = board;

        // fill (model) board with pieces:

        board.AddPieceToSlot((0, 0), new PlayerPiece());
        board.AddPieceToSlot((1, 1), new Tower());
        board.AddPieceToSlot((1, 2), new Tower());
        board.AddPieceToSlot((1, 3), new Tower());

        board.AddPieceToSlot((3, 5), new PlayerPiece());

        // Update the view

        InitializeView();
    }

    private void Update()
    {
        if(OnGameOver != null) // GameOver event
        {
            OnGameOver.Invoke();
            OnGameOver = null;
        }
    }



    private void InitializeView() // Loads the model data and updates the view accordingly for the first time
    {
        // Basically, loops through each board slot, checks where there are pieces and instantiates them on the board
        // Not the most efficient, but since it's executed 1 time at the start of the game, it doesn't matter.
        bool firstPlayerSpawned = false;
        int boardViewIndex = 0;

        Stack<GameObject>[] piecesReferenceForController = new Stack<GameObject>[24];

        for(int i=0; i<24; i++)
        {
            piecesReferenceForController[i] = new Stack<GameObject>();
        }


        

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var list = board.GetBoardSlotPieces((j, i)); // list will always have size 4, but some are empty elements. We need to check where the last existing element is!
                                                             // We want to keep the code as clean and independant as possible, so to avoid creating more get functions on 
                                                             // the board/boardslot classes, we do a dirty and expensive check here. This function will only run once at the start!

                int maxListIndex = 0;
                for(int k=0; k < 4; k++)
                {
                    if (list[k] == null)
                    {
                        maxListIndex = k;
                        break;
                    }
                }

                for(int k=0; k < maxListIndex; k++)
                {
                    int addOnIndex = i * 4 + j;
                    //Debug.Log(addOnIndex);

                    switch (list[k].PieceType)
                    {
                        case E_PieceType.PlayerPiece:
                            if (!firstPlayerSpawned)
                            {
                                controller.P1 = Instantiate(Player1Piece, boardView[boardViewIndex].transform.position, Quaternion.identity, piecesContainer.transform);
                                firstPlayerSpawned = true;
                            }
                            else
                                controller.P2 = Instantiate(Player2Piece, boardView[boardViewIndex].transform.position, Quaternion.identity, piecesContainer.transform);


                            break;
                        case E_PieceType.Pillow:
                            piecesReferenceForController[addOnIndex].Push(Instantiate(Pillow, boardView[boardViewIndex].transform.position, Quaternion.identity, piecesContainer.transform));
                            break;
                        case E_PieceType.Tower:
                            piecesReferenceForController[addOnIndex].Push(Instantiate(Tower, boardView[boardViewIndex].transform.position, Quaternion.identity, piecesContainer.transform));
                            break;

                    }
                            // add (top piece on slot) reference to the controller:

                    //Debug.Log(piecesReferenceForController[addOnIndex].Peek());
                }
                boardViewIndex++;
            }
        }

        controller.PiecesReferences = piecesReferenceForController;

    }

}
