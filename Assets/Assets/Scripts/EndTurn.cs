using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public GameObject endTurnButton;
    public Board board;

    void Start()
    {
        HideEndTurnButton();
    }

    public void EndTurnOnClick()
    {
        board.EndTurnOnMulticapture();
        HideEndTurnButton();
    }
    
    public void ShowEndTurnButton()
    {
        endTurnButton.SetActive(true);
    }

    public void HideEndTurnButton()
    {
        endTurnButton.SetActive(false);
    }
}
