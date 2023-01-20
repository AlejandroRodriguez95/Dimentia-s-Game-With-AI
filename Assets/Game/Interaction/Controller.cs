using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject[] boardView;
    private (int, int) selectorPosition; // to interact with the model
    private int selectorPositionInArray; // internal variable for this script

    [SerializeField] GameObject selector;

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
    #endregion
}
