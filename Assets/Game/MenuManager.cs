using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject initialPanel;
    [SerializeField] GameObject localPvpPanel;
    [SerializeField] GameObject localPvAIPanel;

    [SerializeField]
    TMPro.TMP_InputField p1NameInputField;

    [SerializeField]
    TMPro.TMP_InputField p2NameInputField;

    [SerializeField]
    TMPro.TMP_Dropdown aiDifficulty;

    [SerializeField]
    TMPro.TMP_Dropdown aIPlayStyle;



    public void HideInitialPanel()
    {
        initialPanel.SetActive(false);
    }
    public void ShowInitialPanel()
    {
        initialPanel.SetActive(true);
    }

    public void ShowLocalPvPPanel()
    {
        GlobalData.gamemode = E_Gamemode.PvP;
        localPvpPanel.SetActive(true);
        HideLocalPvAIanel();
    }
    public void HideLocalPvPPanel()
    {
        localPvpPanel.SetActive(false);
    }    
    public void ShowLocalPvAIPanel()
    {
        GlobalData.gamemode = E_Gamemode.PvAI;
        HideLocalPvPPanel();
        localPvAIPanel.SetActive(true);
    }
    public void HideLocalPvAIanel()
    {
        localPvAIPanel.SetActive(false);
    }

    public void ChangeP1Name()
    {
        GlobalData.player1Name = p1NameInputField.text;
    }    
    public void ChangeP2Name()
    {
        GlobalData.player2Name = p2NameInputField.text;
    }

    public void SetAIDifficulty()
    {
        if (aiDifficulty.value == 0)
        {
            GlobalData.aiType = E_AIType.AI_random;
        }
        if(aiDifficulty.value == 1)
        {
            GlobalData.aiType = E_AIType.AI_smart;
            GlobalData.player2Name = "SmartBot";
        }
    }

    public void SetAIPlayStyle()
    {
        if(aIPlayStyle.value == 0)
        {
            GlobalData.aiPlaystyle = 0;
        }
        if(aIPlayStyle.value == 1)
        {
            GlobalData.aiPlaystyle = 1;

        }
        if (aIPlayStyle.value == 2)
        {
            GlobalData.aiPlaystyle = 0;
            GlobalData.randomFactor = 1;
        }
    }

    public void StartAIGame()
    {
        SetAIDifficulty();
        SetAIPlayStyle();
        SceneManager.LoadScene("Game");
    }
    public void StartPvPGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
