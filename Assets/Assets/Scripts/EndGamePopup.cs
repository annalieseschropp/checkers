using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//using MoveController;
using CheckersState;


public class EndGamePopup : MonoBehaviour
{
    public Button quitButtonInPopupWindow;
    public Button restartGameButton;
    public GameObject popupPanel;
    public Text whoWonText;


    //MoveController move = new MoveController();

    // Start is called before the first frame update
    void Start()
    {
        Button quitBtn = quitButtonInPopupWindow.GetComponent<Button>();
        quitBtn.onClick.AddListener(quitIsClicked);

        Button restartButton = restartGameButton.GetComponent<Button>();
        restartButton.onClick.AddListener(restartGameFunc);
        
    }

    public void EndGame(string whoOne)
    {
        popupPanel.SetActive(true);
        //Text.text = whoOne + " won the game";
    }

    // Update is called once per frame
    void quitIsClicked()
    {
        SceneManager.LoadScene("Menu");
    }

    void restartGameFunc()
    {
        Debug.Log("RESTART Clicked");
        SceneManager.LoadScene("GameBoard");

        
    }
    
}
