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
        quitBtn.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);

        Button goBackBtn = goBackButton.GetComponent<Button>();
        goBackBtn.onClick.AddListener(closePopup);
        goBackBtn.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);

        Button quitGame = quitButtonOnCheckBoard.GetComponent<Button>();
        quitGame.onClick.AddListener(openPopup);
        quitGame.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
    }

    // Update is called once per frame
    public void quitClicked()
    {
        //Close popup before leaving
        popupPanel.SetActive(false);

        //Now quit
        Debug.Log("Quit Game");
        SceneManager.LoadScene("Menu");
    }

    public void closePopup()
    {
        Debug.Log("Pressed");
        popupPanel.SetActive(false);
    }

    public void openPopup()
    {
        Debug.Log("Pressed2");
        popupPanel.SetActive(true);
    }

    public bool getPopupState()
    {
        return popupPanel.activeSelf;
    }
}