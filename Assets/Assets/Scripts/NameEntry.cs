using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameEntry : MonoBehaviour
{
    public Button playGameButton;
    public Button cancelGameButton;
    public InputField playerOneName;
    public InputField playerTwoName;
    public Dropdown dropdownPlayerOne;
    public Dropdown dropdownPlayerTwo;
    public Toggle forcedCapture;
    int update = 0;

    // Start is called before the first frame update
    void Start()
    {
        Button playGame = playGameButton.GetComponent<Button>();
        Button cancelGame = cancelGameButton.GetComponent<Button>();
        
        dropdownPlayerOne = dropdownPlayerOne.GetComponent<Dropdown>();
        dropdownPlayerTwo = dropdownPlayerTwo.GetComponent<Dropdown>();
        forcedCapture = forcedCapture.GetComponent<Toggle>();
        playerOneName = playerOneName.GetComponent<InputField>();
        playerTwoName = playerTwoName.GetComponent<InputField>();

        List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
        Dropdown.OptionData item1 = new Dropdown.OptionData();
        item1.text = "Hello1";
        list.Add(item1);
        dropdownPlayerOne.AddOptions(list);

        playGame.onClick.AddListener(PlayGameButtonOnClick);
        cancelGame.onClick.AddListener(CancelGameButtonOnClick);
        playerOneName.onValueChanged.AddListener(delegate {PlayerOneNameOnUpdate();});
        dropdownPlayerOne.onValueChanged.AddListener(delegate {PlayerOneDropdownOnUpdate();});
    }

    void PlayGameButtonOnClick()
    {
        Debug.Log("Clicked + " + forcedCapture.isOn);
    }

    void CancelGameButtonOnClick()
    {
        SceneManager.LoadScene("Menu");
    }

    // Ensure that only either a input value or a dropdown menu is selected

    // Functions to ensure that when the entered name has a value, the dropdown is blank.
    void PlayerOneNameOnUpdate()
    {
        string value = playerOneName.text;
        int position = playerOneName.caretPosition;
        dropdownPlayerOne.value = 0;
        playerOneName.text = value;
        playerOneName.caretPosition = position;
    }

    void playerTwoNameOnUpdate()
    {
        string value = playerTwoName.text;
        int position = playerTwoName.caretPosition;
        dropdownPlayerTwo.value = 0;
        playerTwoName.text = value;
        playerTwoName.caretPosition = position;
    }

    // Functions to ensure that when the dropdown isn't blank, the input value is blank.
    void PlayerOneDropdownOnUpdate()
    {
        int value = dropdownPlayerOne.value;
        playerOneName.text = "";
        dropdownPlayerOne.value = value;
    }

    void playerOneDropdownOnUpdate()
    {
        int value = dropdownPlayerOne.value;
        playerTwoName.text = "";
        dropdownPlayerTwo.value = value;
    }
}
