using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using CheckersState;


public class EndGamePopup : MonoBehaviour
{
    public Button quitButtonInPopupWindow;
    public Button restartGameButton;
    public GameObject popupPanel;
    public Text whoWonText;

    // Start is called before the first frame update
    void Start()
    {
        Button quitBtn = quitButtonInPopupWindow.GetComponent<Button>();
        quitBtn.onClick.AddListener(quitIsClicked);
        quitBtn.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);

        Button restartButton = restartGameButton.GetComponent<Button>();
        restartButton.onClick.AddListener(restartGameFunc);
        restartButton.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
    }


    // Update is called once per frame
    public void quitIsClicked()
    {
        SceneManager.LoadScene("Menu");
    }

    public void restartGameFunc()
    {
        Debug.Log("RESTART Clicked");
        NameStaticClass.SwapPlayerNames();
        SceneManager.LoadScene("GameBoard");
    }
    
}
