using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject[] boardView; // the gameobjects part of the board (visuals)
    [SerializeField] GameObject selectedPiece;
    [SerializeField] GameObject selector;
    GameObject turnIndicatorP1;
    GameObject turnIndicatorP2;
    List<GameObject> allowedMoveIndicators;
    List<GameObject> allowedSelectionIndicators;
    TextMeshProUGUI moveList;
    RectTransform moveListRectTransform;
    int moveListIndex;

    GameObject p1; 
    GameObject p2;

    Player boardP1;
    Player boardP2;



    AI aI; // not used in pvp

    private bool p1Plays;
    
    private (int, int) selectorPosition; // to interact with the model
    [SerializeField] private E_TurnStages turnStage; //Synchronized with model through play function
    private int selectorPositionInArray; // internal variable for this script
    private int previousSelectorPos; // internal variable
    private E_Gamemode E_gamemode; // affects updateview method
    public bool gameOver;
    private PlayerVsAI pvAIReference; // used in games where player plays vs AI, set through gamemanager

    WaitForSeconds waitingAiTime; 


    [SerializeField] private Stack<GameObject>[] piecesReferences; // does not include players. Reference to the top piece on each slot for the selector, filled through gamemanager on board initialization
    GameMode gameModeRef; // referenced through gamemanager, needed to interact with the model

    private Board board; // for debugging, must be deleted when everything is working!


    public RectTransform MoveListRectTransform
    {
        set { moveListRectTransform = value; }
    }
    public TextMeshProUGUI MoveList
    {
        set { moveList = value; }
    }
    public Player BoardP1
    {
        set { boardP1 = value; }
    }
    public Player BoardP2
    {
        set { boardP2 = value; }
    }
    public GameObject TurnIndicatorP1
    {
        set { turnIndicatorP1 = value; }
    }    

    public List<GameObject> AllowedSelectionIndicators
    {
        set { allowedSelectionIndicators = value; }
    }


    public List <GameObject> AllowedMoveIndicators
    {
        set { allowedMoveIndicators = value; }
    }

    public GameObject TurnIndicatorP2
    {
        set { turnIndicatorP2 = value; }
    }
    public Board Board
    {
        set { board = value; }
    }
    public GameMode GameModeRef
    {
        set { gameModeRef = value; }
    }

    public bool GameOver
    {
        set { gameOver = value; }
    }


    public E_Gamemode Set_E_Gamemode
    {
        set { E_gamemode = value; }
    }

    public WaitForSeconds WaitingAiTime
    {
        set { waitingAiTime = value; }
    }

    public AI AI
    {
        set { aI = value; }
    }

    public PlayerVsAI PvAIReference
    {
        set { pvAIReference = value; }
    }

    public Stack<GameObject>[] PiecesReferences
    {
        set { piecesReferences = value; }
    }

    public GameObject P1
    {
        set { p1 = value; }
    }

    public GameObject P2
    {
        set { p2 = value; }
    }

    private void Awake()
    {
        piecesReferences = new Stack<GameObject>[24];

        turnStage = E_TurnStages.TurnStart;
        
        p1Plays = true; // must be synchronized if starting player is randomized
        selectorPosition = (0, 0);
        selectorPositionInArray = 0;
        gameOver = false;
        moveListIndex = 0;
    }



    // Interaction with the view (player controller functions)
    #region InputFunctions

    public void MoveUp(InputAction.CallbackContext cbc)
    {
        if (selectorPosition.Item2 + 1 > 5 || !cbc.started || turnStage == E_TurnStages.TurnStart)
            return;

        selectorPosition.Item2 += 1;
        selectorPositionInArray += 4;
        selector.transform.position = boardView[selectorPositionInArray].transform.position;
    }   
    public void MoveDown(InputAction.CallbackContext cbc)
    {
        if (selectorPosition.Item2 - 1 < 0 || !cbc.started || turnStage == E_TurnStages.TurnStart)
            return;

        selectorPosition.Item2 -= 1;
        selectorPositionInArray -= 4;
        selector.transform.position = boardView[selectorPositionInArray].transform.position;
    }    
    public void MoveLeft(InputAction.CallbackContext cbc)
    {
        if (selectorPosition.Item1 - 1 < 0 || !cbc.started || turnStage == E_TurnStages.TurnStart)
            return;

        selectorPosition.Item1 -= 1;
        selectorPositionInArray -= 1;
        selector.transform.position = boardView[selectorPositionInArray].transform.position;
    }    
    public void MoveRight(InputAction.CallbackContext cbc)
    {
        if (selectorPosition.Item1 + 1 > 3 || !cbc.started || turnStage == E_TurnStages.TurnStart)
            return;

        selectorPosition.Item1 += 1;
        selectorPositionInArray += 1;
        selector.transform.position = boardView[selectorPositionInArray].transform.position;
    }


    public void Select(InputAction.CallbackContext cbc) // Interacts with the model
    {
        if (!cbc.started)
            return;

        if (E_gamemode == E_Gamemode.PvAI && !p1Plays)
            return;

        if (gameOver)
            return;

        if (turnStage == E_TurnStages.GameOver)
            return;

        if (gameModeRef.Play(selectorPosition, ref turnStage)) // play function interacts with model, if result is true, then visuals are updated: 
        {
            UpdateView();          
        }
            UpdateIndicators();
    }

    #endregion


    private void UpdateIndicators()
    {
        Player currentPlayer;
        if (p1Plays) currentPlayer = boardP1;
        else currentPlayer = boardP2;

        switch (turnStage)
        {
            case E_TurnStages.MovePlayer:
                HideAllowedMoveIndicators();
                DisplayAllowedMoveIndicators(currentPlayer, 1);
                break;
            case E_TurnStages.SelectPiece:
                HideAllowedMoveIndicators();
                DisplayAllowedSelectionIndicators(currentPlayer);
                break;
            case E_TurnStages.MovePiece:
                HideAllowedSelectionIndicators();
                DisplayAllowedMoveIndicators(currentPlayer, 2);
                break;
            default:
                HideAllowedSelectionIndicators();
                HideAllowedSelectionIndicators();
                break;
        }

    }

    private void UpdateView()
    {
        int boardViewIndex = IndexOfMoveInViewList(selectorPosition);

        // Turnstage is 1 step ahead each time due to the play function updating it
        switch (turnStage)
        {
            case E_TurnStages.MovePlayer: // Select player phase
                //movePlayerOrPiece = true;

                if (p1Plays)
                {
                    selectedPiece = p1;
                }
                else
                {
                    selectedPiece = p2;
                }
                break;

            case E_TurnStages.SelectPiece: // Move player phase 

                if (!p1Plays && E_gamemode == E_Gamemode.PvAI) // only in games against AI, AI move is read from the PlayerVsAI class
                {
                    boardViewIndex = IndexOfMoveInViewList(pvAIReference.AIMove);
                }


                PlayerMovedToTeleport(ref boardViewIndex); // check if player stepped on teleport



                selectedPiece.transform.position = boardView[boardViewIndex].transform.position;
                break;

            case E_TurnStages.MovePiece: // Select piece phase
                int toSelectIndex = IndexOfMoveInViewList(selectorPosition);

                if (!p1Plays && E_gamemode == E_Gamemode.PvAI) // only in games against AI, AI move is read from the PlayerVsAI class
                {
                    toSelectIndex = IndexOfMoveInViewList(pvAIReference.AIMove);
                }

                previousSelectorPos = toSelectIndex;
                selectedPiece = piecesReferences[toSelectIndex].Peek();
                break;

            case E_TurnStages.TurnStart: // Move piece phase
                //int boardViewIndex = (selectorPosition.Item1 + selectorPosition.Item2 * 4);
                int sortingIndex = board.GetAmountOfPiecesInSlot(selectorPosition);
                int amountOfPillowsInSlot = 0;
                Vector3 pillowOffset = Vector3.zero;
                if (!p1Plays && E_gamemode == E_Gamemode.PvAI) // only in games against AI, AI move is read from the PlayerVsAI class
                {
                    boardViewIndex = IndexOfMoveInViewList(pvAIReference.AIMove);
                    sortingIndex = board.GetAmountOfPiecesInSlot(pvAIReference.AIMove);
                }

                if(selectedPiece.tag == "Pillow")
                {
                    (int, int) slotToCheck = selectorPosition;
                    if (!p1Plays && E_gamemode == E_Gamemode.PvAI)
                        slotToCheck = pvAIReference.AIMove;

                    amountOfPillowsInSlot = board.GetAmountOfPillowsInSlot(slotToCheck);
                    if (amountOfPillowsInSlot == 1)
                        pillowOffset = new Vector3(0.3f, 0.3f);
                    if (amountOfPillowsInSlot == 2)
                        pillowOffset = new Vector3(-0.3f, 0.3f);
                    if (amountOfPillowsInSlot == 3)
                        pillowOffset = new Vector3(0, -0.3f);
                }

                selectedPiece.transform.position = boardView[boardViewIndex].transform.position - pillowOffset;
                selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = sortingIndex;

                piecesReferences[previousSelectorPos].Pop();
                piecesReferences[boardViewIndex].Push(selectedPiece);



                if (p1Plays)
                {
                    p1Plays = false;
                    turnIndicatorP1.SetActive(false);
                    turnIndicatorP2.SetActive(true);
                    selector.transform.position = p2.transform.position;
                    selectorPosition = boardP2.PlayerPosOnBoard;
                    selectorPositionInArray = IndexOfMoveInViewList(selectorPosition);
                    HideAllowedMoveIndicators();
                    AddToMoveList(moveListIndex);

                    if (E_gamemode == E_Gamemode.PvAI)
                        StartCoroutine(AILoop());
                }
                else
                {
                    p1Plays = true;
                    selector.transform.position = p1.transform.position;
                    selectorPosition = boardP1.PlayerPosOnBoard;
                    selectorPositionInArray = IndexOfMoveInViewList(selectorPosition);
                    HideAllowedMoveIndicators();

                    AddToMoveList(moveListIndex);

                    turnIndicatorP1.SetActive(true);
                    turnIndicatorP2.SetActive(false);
                }

                break;

            default:
                Debug.LogWarning($"Error: {this.GetType()} on UpdateView");
                break;
        }

    }




    private bool PlayerMovedToTeleport(ref int newTarget)
    {
        if (newTarget == (20)) 
        {
            newTarget = 3; // 3 comes from 3 + (0*4)
            return true;
        }
        else if (newTarget == 3)
        {
            newTarget = 20; // 20 comes from 0 + (5*4)
            return true;
        }
        return false;
    }

    private int IndexOfMoveInViewList((int, int) slot)
    {
        return slot.Item1 + slot.Item2 * 4;
    }


    private IEnumerator AILoop()
    {
        while (!p1Plays && !gameOver)
        {
            yield return waitingAiTime;
            if (gameModeRef.Play(selectorPosition, ref turnStage)) // play function interacts with model, if result is true, then visuals are updated: 
            {
                UpdateView();
                UpdateIndicators();
            }
        }

    }

    private void AddToMoveList(int index)
    {
        var temp = TurnSystem.allMoves[index];
        moveList.text += $"P{temp.playerNumber} from {temp.playerFrom} to {temp.playerTo}, {temp.selectedPieceType} from {temp.selectedPieceFrom} to {temp.selectedPieceTo} \n";
        moveList.rectTransform.sizeDelta += new Vector2(0, 15f);
        moveListRectTransform.sizeDelta += new Vector2(0, 15f);
        moveListIndex++;
    }


    private void DisplayAllowedMoveIndicators(Player player, int radius)
    {
        var legalMoves = new List<(int,int)>();
        if(radius == 1)
        {
            legalMoves = board.ReturnLegalMovesForPlayer(player, radius);
        }
        else if(radius == 2)
        {
            legalMoves = board.ReturnLegalMovesForPieces(player, radius);
        }
        foreach (var slot in legalMoves)
        {
            allowedMoveIndicators[IndexOfMoveInViewList(slot)].SetActive(true);
        }
    }

    private void HideAllowedMoveIndicators()
    {
        foreach(var indicator in allowedMoveIndicators)
        {
            indicator.SetActive(false);
        }
    }

    private void DisplayAllowedSelectionIndicators(Player player)
    {
        var legalMoves = board.ReturnPiecesInRangeOfPlayer(player);
        foreach (var slot in legalMoves)
        {
            allowedSelectionIndicators[IndexOfMoveInViewList(slot)].SetActive(true);
        }
    }

    private void HideAllowedSelectionIndicators()
    {
        foreach (var indicator in allowedSelectionIndicators)
        {
            indicator.SetActive(false);
        }
    }

    // Debugging:

    public void Debug1(InputAction.CallbackContext cbc)
    {
        if (!cbc.started)
            return;

        board.PrintAllPieces();
    }    
    public void Debug2(InputAction.CallbackContext cbc)
    {
        if (!cbc.started)
            return;

        board.PrintSlotsThatContainPlayers();

    }
    public void Debug3(InputAction.CallbackContext cbc)
    {
        if (!cbc.started)
            return;

        var newAi = aI as Smart;
        newAi.IterateThroughAllPossibleMoves();
        //newAi.MovePlayerOnTestBoard((2, 5));
    }

}
