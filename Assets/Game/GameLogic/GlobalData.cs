using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalData : Singleton
{
    public static string player1Name;
    public static string player2Name;

    public static E_Gamemode gamemode;
    public static E_AIType aiType;
    public static int aiPlaystyle;

    private void Start()
    {
        gamemode = E_Gamemode.PvAI;
        aiType = E_AIType.AI_smart;

        player1Name = "Anonymous";
        player2Name = "Bot";

    }


    //public void ChangeP2Name()
    //{
    //    if (string.IsNullOrEmpty(newName))
    //        player2Name = newName;
    //}
}
