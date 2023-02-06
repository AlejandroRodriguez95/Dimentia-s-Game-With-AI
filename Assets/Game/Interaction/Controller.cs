using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject[] boardView; // the gameobjects part of the board (visuals)
    [SerializeField] GameObject selectedPiece;
    [SerializeField] GameObject selector;

    GameObject p1; 
    GameObject p2;

    private bool p1Plays;

    private (int, int) selectorPosition; // to interact with the model
    [SerializeField] private E_TurnStages turnStage; //Synchronized with model through play function
    private int selectorPositionInArray; // internal variable for this script
    private int previousSelectorPos; // internal variable


    [SerializeField] private Stack<GameObject>[] piecesReferences; // does not include players. Reference to the top piece on each slot for the selector, filled through gamemanager on board initialization
    GameMode gameModeRef; // referenced through gamemanager, needed to interact with the model

    public Board board; // for debugging, must be deleted when everything is working!

    public GameMode GameModeRef
    {
        set { gameModeRef = value; }
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
    }


    // Interaction with the view (player controller functions)
    #region InputFunctions
 
    public void MoveUp(InputAction.CallbackContext cbc)
    {
        if (selectorPosition.Item2 + 1 > 5 || !cbc.started)
            return;

        selectorPosition.Item2 += 1;
        selectorPositionInArray += 4;
        selector.transform.position = boardView[selectorPositionInArray].transform.position;
    }   
    public void MoveDown(InputAction.CallbackContext cbc)
    {
        if (selectorPosition.Item2 - 1 < 0 || !cbc.started)
            return;

        selectorPosition.Item2 -= 1;
        selectorPositionInArray -= 4;
        selector.transform.position = boardView[selectorPositionInArray].transform.position;
    }    
    public void MoveLeft(InputAction.CallbackContext cbc)
    {
        if (selectorPosition.Item1 - 1 < 0 || !cbc.started)
            return;

        selectorPosition.Item1 -= 1;
        selectorPositionInArray -= 1;
        selector.transform.position = boardView[selectorPositionInArray].transform.position;
    }    
    public void MoveRight(InputAction.CallbackContext cbc)
    {
        if (selectorPosition.Item1 + 1 > 3 || !cbc.started)
            return;

        selectorPosition.Item1 += 1;
        selectorPositionInArray += 1;
        selector.transform.position = boardView[selectorPositionInArray].transform.position;
    }


    public void Select(InputAction.CallbackContext cbc) // Interacts with the model
    {
        if (!cbc.started)
            return;

        if (turnStage == E_TurnStages.GameOver)
            return;

        if (gameModeRef.Play(selectorPosition, ref turnStage)) // play function interacts with model, if result is true, then visuals are updated: 
        {
            UpdateView();
        }
    }

    #endregion





    private void UpdateView()
    {
        // Turnstage is 1 step ahead each time due to the play function updating it

        if (turnStage == E_TurnStages.MovePlayer) // Select player phase
        {
            //movePlayerOrPiece = true;

            if (p1Plays)
                selectedPiece = p1;
            else
                selectedPiece = p2;

        }
        else if (turnStage == E_TurnStages.SelectPiece || turnStage == E_TurnStages.GameOver) // Move player phase
        {
            int boardViewIndex = (selectorPosition.Item1 + selectorPosition.Item2 * 4);

            PlayerMovedToTeleport(ref boardViewIndex);


            selectedPiece.transform.position = boardView[boardViewIndex].transform.position;

        }
        else if (turnStage == E_TurnStages.MovePiece) // Select piece phase
        {
            int toSelectIndex = (selectorPosition.Item1 + selectorPosition.Item2 * 4);
            previousSelectorPos = toSelectIndex;
            selectedPiece = piecesReferences[toSelectIndex].Peek();
        }
        else if (turnStage == E_TurnStages.TurnStart) // Move piece phase
        {
            int boardViewIndex = (selectorPosition.Item1 + selectorPosition.Item2 * 4);
            selectedPiece.transform.position = boardView[boardViewIndex].transform.position;

            piecesReferences[previousSelectorPos].Pop();
            piecesReferences[boardViewIndex].Push(selectedPiece);



            if (p1Plays)
                p1Plays = false;
            else
                p1Plays = true;
        }
    }




    private bool PlayerMovedToTeleport(ref int newTarget)
    {
        // hard coded values of teleport positions! should've made everything event-based! check board.cs moveplayer function!
        // this checks if the player is stepping on a teleport, and teleports the player to the second teleport position
        if (selectorPosition == (0, 5))
        {
            newTarget = 3; // 3 comes from 3 + (0*4)
            return true;
        }
        else if (selectorPosition == (3, 0))
        {
            newTarget = 20; // 20 comes from 0 + (5*4)
            return true;
        }
        return false;
    }

    // Debugging:

    public void Debug1(InputAction.CallbackContext cbc)
    {
        if (!cbc.started)
            return;

        board.GetBoardSlotPieces((1, 1));
    }    
    public void Debug2(InputAction.CallbackContext cbc)
    {
        if (!cbc.started)
            return;

        (int, int) move = (-1, -1);
        Debug.Log(Player.ai.GenerateMove(ref move, E_TurnStages.MovePlayer));
    }
    public void Debug3(InputAction.CallbackContext cbc)
    {
        if (!cbc.started)
            return;
    }

}
