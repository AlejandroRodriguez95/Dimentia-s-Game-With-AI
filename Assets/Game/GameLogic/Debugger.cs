using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    Board board;
    void Start()
    {
        board = new Board();
        board.PrintBoard();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
