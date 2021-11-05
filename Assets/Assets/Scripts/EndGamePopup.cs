using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//using MoveController;

public class EndGamePopup : MonoBehaviour
{
    public Button quitButtonInPopupWindow;
    public Button restartGameButton;
    public GameObject popupPanel;
    public Text whoWonText;


    public void EndGame(string whoOne)
    {
        //popupPanel.SetActive(true);
        //Text.text = whoOne + " won the game";
    }

    // Update is called once per frame
    void quitIsClicked()
    {
        //Debug.Log("Quit Game");
        //SceneManager.LoadScene("Menu");
    }

    void restartGameFunc()
    {
        //MoveController.RestartGame();
    }
    
}
