using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Popup : MonoBehaviour
{
    public Button quitButtonInPopupWindow;
    public Button goBackButton;
    public GameObject popupPanel;
    public Button quitButtonOnCheckBoard;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        Button quitBtn = quitButtonInPopupWindow.GetComponent<Button>();
        quitBtn.onClick.AddListener(QuitClicked);

        Button goBackBtn = goBackButton.GetComponent<Button>();
        goBackBtn.onClick.AddListener(ClosePopup);

        Button quitGame = quitButtonOnCheckBoard.GetComponent<Button>();
        quitGame.onClick.AddListener(OpenPopup);

    }

    /// <summary>
    /// Method
    /// Closes the popup window and the current game
    /// Update is called once per frame
    /// </summary>
    void QuitClicked()
    {
        //Close popup before leaving
        popupPanel.SetActive(false);

        //Now quit
        Debug.Log("Quit Game");
        SceneManager.LoadScene("Menu");
    }


    /// <summary>
    /// Method
    /// Closes the popup
    /// </summary>
    void ClosePopup()
    {
        popupPanel.SetActive(false);
    }

    /// <summary>
    /// Method
    /// Opens the popup
    /// </summary>
    void OpenPopup()
    {
        popupPanel.SetActive(true);
    }
}
