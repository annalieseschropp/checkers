using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public GameObject endTurnButton;
    // Start is called before the first frame update
    void Start()
    {
        HideEndTurnButton();
    }

    public void EndTurnFunction()
    {
        Debug.Log("End Turn Button Clicked!");
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
