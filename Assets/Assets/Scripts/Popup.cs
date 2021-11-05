using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class Popup : MonoBehaviour
{
    public Button quitButtonInPopupWindow;
    public Button goBackButton;
    public GameObject popupPanel;
    public Button quitButtonOnCheckBoard;
    
    // Start is called before the first frame update
    void Start()
    {
        Button quitBtn = quitButtonInPopupWindow.GetComponent<Button>();
        quitBtn.onClick.AddListener(quitClicked);

        Button goBackBtn = goBackButton.GetComponent<Button>();
        goBackBtn.onClick.AddListener(closePopup);

        Button quitGame = quitButtonOnCheckBoard.GetComponent<Button>();
        quitGame.onClick.AddListener(openPopup);

    }

    // Update is called once per frame
    void quitClicked()
    {
        //Close popup before leaving
        popupPanel.SetActive(false);

        //Now quit
        Debug.Log("Quit Game");
        SceneManager.LoadScene("Menu");
    }

    void closePopup()
    {
        Debug.Log("Pressed");
        popupPanel.SetActive(false);
    }

    void openPopup()
    {
        Debug.Log("Pressed2");
        popupPanel.SetActive(true);
    }
}