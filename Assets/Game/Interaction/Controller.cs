using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject[] boardView; // the gameobjects part of the board (visuals)
    private (int, int) selectorPosition; // to interact with the model

    private int selectorPositionInArray; // internal variable for this script

    [SerializeField] GameObject selector;

    GameMode gameModeRef; // referenced through gamemanager, needed to interact with the model

    public Board board; // for debugging, must be deleted when everything is working!

    public GameMode GameModeRef
    {
        set { gameModeRef = value; }
    }

    private void Awake()
    {
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

        gameModeRef.Play(selectorPosition);
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

        board.PrintAllPieces();
    }
    public void Debug3(InputAction.CallbackContext cbc)
    {
        if (!cbc.started)
            return;
    }

    #endregion
}
