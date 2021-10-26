using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameEntry : MonoBehaviour
{
    public Button playGameButton;
    public Button cancelGameButton;
    public InputField playerOneName;
    public InputField playerTwoName;
    int update = 0;

    // Start is called before the first frame update
    void Start()
    {
        Button playGame = playGameButton.GetComponent<Button>();
        Button cancelGame = cancelGameButton.GetComponent<Button>();
        playerOneName = playerOneName.GetComponent<InputField>();
        playerTwoName = playerTwoName.GetComponent<InputField>();
    
        playGame.onClick.AddListener(PlayGameButtonOnClick);
        cancelGame.onClick.AddListener(CancelGameButtonOnClick);
        playerOneName.onValueChanged.AddListener(delegate {PlayerOneNameOnUpdate();});
    }

    void PlayGameButtonOnClick()
    {
        Debug.Log("Clicked + " + playerOneName.text);
    }

    void CancelGameButtonOnClick()
    {

    }

    void PlayerOneNameOnUpdate()
    {
        
    }
}
