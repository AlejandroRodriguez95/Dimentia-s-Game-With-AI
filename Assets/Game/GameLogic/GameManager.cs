using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void OnGameOverEvent();
    public static event OnGameOverEvent OnGameOver;

    [SerializeField] Controller controller;
    [SerializeField] GameObject[] boardView;
    [SerializeField] GameObject piecesContainer;
    [SerializeField] TextMeshProUGUI p1UIName;
    [SerializeField] TextMeshProUGUI p2UIName;
    [SerializeField] TextMeshProUGUI moveList;
    [SerializeField] GameObject turnIndicatorP1;
    [SerializeField] GameObject turnIndicatorP2;
    [SerializeField] GameObject allowedMoveIndicator;
    [SerializeField] TextMeshProUGUI winnerText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject allowedSelectionIndicator;
    List<GameObject> allowedMoveIndicators;
    List<GameObject> allowedSelectionIndicators;

    public static int winner;

    [SerializeField] E_AIDefaultBehavior E_defaultAIBehavior;
    int defaultBehavior;

    public static Board board;
    GameMode gameMode;

    Player p1;
    Player p2;

    // GameConfig:
    string p1Name;
    string p2Name;
    E_Gamemode gamemode;
    E_AIType aiType;


    [SerializeField] float aIDelayBetweenMoves;

    [SerializeField] GameObject Tower;
    [SerializeField] GameObject Pillow;
    [SerializeField] GameObject Player1Piece;
    [SerializeField] GameObject Player2Piece;

    TurnSystem turnSystem;

    // Debugging:



    private void Start()
    {
        p1Name = GlobalData.player1Name;
        p2Name = GlobalData.player2Name;
        gamemode = GlobalData.gamemode;
        aiType = GlobalData.aiType;
        defaultBehavior = GlobalData.aiPlaystyle;

        
        
        p1UIName.text = p1Name;
        p2UIName.text = p2Name;


        allowedSelectionIndicators = new List<GameObject>();
        allowedMoveIndicators = new List<GameObject>();

        InitializeGame(gamemode, p1Name, aiType);

        // Update the view

        InitializeView();
        InstantiateIndicators();

        

    }

    private void Update()
    {
        if(OnGameOver != null) // GameOver event
        {
            OnGameOver.Invoke();
            controller.GameOver = true;
            GameOver();
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
                var amountOfPillowsInSlot = board.GetAmountOfPillowsInSlot((j, i));

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
                            Vector3 pillowOffset = Vector3.zero;

                            if (amountOfPillowsInSlot == 1)
                                pillowOffset = new Vector3(0.3f, 0.3f);
                            if (amountOfPillowsInSlot == 2)
                                pillowOffset = new Vector3(-0.3f, 0.3f);
                            if (amountOfPillowsInSlot == 3)
                                pillowOffset = new Vector3(0, -0.3f);

                            piecesReferenceForController[addOnIndex].Push(Instantiate(Pillow, boardView[boardViewIndex].transform.position - pillowOffset, Quaternion.identity, piecesContainer.transform));
                            piecesReferenceForController[addOnIndex].Peek().GetComponent<SpriteRenderer>().sortingOrder = piecesReferenceForController[addOnIndex].Count;
                            break;
                        case E_PieceType.Tower:
                            piecesReferenceForController[addOnIndex].Push(Instantiate(Tower, boardView[boardViewIndex].transform.position, Quaternion.identity, piecesContainer.transform));
                            piecesReferenceForController[addOnIndex].Peek().GetComponent<SpriteRenderer>().sortingOrder = piecesReferenceForController[addOnIndex].Count;
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

    private void InstantiateIndicators()
    {
        int boardviewIndex = 0;
        for (int i=0; i<=5; i++)
        {
            for (int j=0; j<=3; j++)
            {
                allowedMoveIndicators.Add(Instantiate(allowedMoveIndicator, boardView[boardviewIndex].transform.position, Quaternion.identity));
                allowedSelectionIndicators.Add(Instantiate(allowedSelectionIndicator, boardView[boardviewIndex].transform.position, Quaternion.identity));
                allowedMoveIndicators[boardviewIndex].SetActive(false);
                allowedSelectionIndicators[boardviewIndex].SetActive(false);
                boardviewIndex++;
            }
        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        controller.gameOver = true;
        string winnerName;
        winnerName = (winner == 1) ? p1Name : p2Name;
        winnerText.text = $"Player {winnerName} has won!";
    }

    private void FillStartingBoardPieces()
    {
        board.AddPieceToSlot((0, 0), new PlayerPiece());

        board.AddPieceToSlot((1, 1), new Pillow());
        board.AddPieceToSlot((1, 1), new Tower());
        board.AddPieceToSlot((1, 2), new Tower());
        board.AddPieceToSlot((1, 2), new Pillow());
        board.AddPieceToSlot((2, 0), new Pillow());
        board.AddPieceToSlot((3, 1), new Tower());
        board.AddPieceToSlot((0, 1), new Tower());

        board.AddPieceToSlot((2, 4), new Pillow());
        board.AddPieceToSlot((3, 4), new Tower());
        board.AddPieceToSlot((2, 3), new Tower());
        board.AddPieceToSlot((2, 3), new Pillow());
        board.AddPieceToSlot((2, 5), new Tower());
        board.AddPieceToSlot((0, 4), new Pillow());
        board.AddPieceToSlot((1, 4), new Tower());

        board.AddPieceToSlot((3, 5), new PlayerPiece());
    }

    private void InitializeGame(E_Gamemode E_Gamemode, string playerName, E_AIType AI_Type)
    {
        controller.TurnIndicatorP1 = turnIndicatorP1;
        controller.TurnIndicatorP2 = turnIndicatorP2;
        controller.AllowedMoveIndicators = allowedMoveIndicators;
        controller.AllowedSelectionIndicators = allowedSelectionIndicators;
        controller.MoveList = moveList;
        controller.MoveListRectTransform = moveList.transform.parent.GetComponent<RectTransform>();

        switch (E_Gamemode)
        {
            case E_Gamemode.PvP:
                board = new Board();

                p1 = new Player(E_PlayerType.Player, playerName, (3, 5));
                p2 = new Player(E_PlayerType.Player, p2Name, (0, 0));
                //p2 = new Player(E_PlayerType.Player, "P2", (0, 0));

                controller.BoardP1 = p1;
                controller.BoardP2 = p2;

                gameMode = new PlayerVsPlayer(board, p1, p2);

                controller.GameModeRef = gameMode;
                controller.Board = board;
                controller.Set_E_Gamemode = E_Gamemode.PvP;


                FillStartingBoardPieces(); // fill model

                break;
            case E_Gamemode.PvAI:
                switch (AI_Type)
                {
                    case E_AIType.AI_random:
                        board = new Board();

                        p1 = new Player(E_PlayerType.Player, playerName, (3, 5));
                        p2 = new Player(E_PlayerType.AI_Random, "Bot", (0, 0));

                        gameMode = new PlayerVsAI(board, p1, p2, aIDelayBetweenMoves);

                        controller.BoardP1 = p1;
                        controller.BoardP2 = p2;

                        controller.Set_E_Gamemode = E_Gamemode.PvAI;
                        controller.GameModeRef = gameMode;
                        controller.Board = board;
                        controller.AI = p2.AI;
                        controller.PvAIReference = gameMode as PlayerVsAI;
                        controller.WaitingAiTime = new WaitForSeconds(aIDelayBetweenMoves);

                        FillStartingBoardPieces();

                        break;

                    case E_AIType.AI_smart:
                        board = new Board();

                        FillStartingBoardPieces();

                        switch (E_defaultAIBehavior)
                        {
                            case E_AIDefaultBehavior.attackHotArea:
                                defaultBehavior = 0;
                                break;
                            case E_AIDefaultBehavior.defendHotArea:
                                defaultBehavior = 1;
                                break;
                            case E_AIDefaultBehavior.trapOpponent:
                                defaultBehavior = 2;
                                break;
                            case E_AIDefaultBehavior.defendSelfFromTrap:
                                defaultBehavior = 3;
                                break;
                        }



                        p1 = new Player(E_PlayerType.Player, playerName, (3, 5));
                        p2 = new Player(E_PlayerType.AI_Smart, "Bot", (0, 0));

                        controller.BoardP1 = p1;
                        controller.BoardP2 = p2;

                        var tempSmart = p2.AI as Smart;
                        tempSmart.Enemy = p1;
                        tempSmart.DefaultBehavior = defaultBehavior;

                        gameMode = new PlayerVsAI(board, p1, p2, aIDelayBetweenMoves);

                        controller.Set_E_Gamemode = E_Gamemode.PvAI;
                        controller.GameModeRef = gameMode;
                        controller.Board = board;
                        controller.AI = p2.AI;
                        controller.PvAIReference = gameMode as PlayerVsAI;
                        controller.WaitingAiTime = new WaitForSeconds(aIDelayBetweenMoves);


                        break;
                }

                break;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
