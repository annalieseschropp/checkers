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
    public Text topText;
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
        topText = topText.GetComponent<Text>();
        
        // Load the previous players in the game
        RecordKeeper.LoadData();
        List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
        
        for (int i = 0; i < RecordKeeper.listOfRecords.Count; i++)
        {
            Dropdown.OptionData item = new Dropdown.OptionData();
            item.text = RecordKeeper.listOfRecords[i].name;
            list.Add(item);
        }

        dropdownPlayerOne.AddOptions(list);
        dropdownPlayerTwo.AddOptions(list);

        playGame.onClick.AddListener(PlayGameButtonOnClick);
        cancelGame.onClick.AddListener(CancelGameButtonOnClick);
        
        playGame.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
        cancelGame.onClick.AddListener(SoundSingleton.GetInstance().PlayButtonClickSound);
        forcedCapture.onValueChanged.AddListener(delegate {SoundSingleton.GetInstance().PlayButtonClickSound();});
        
        playerOneName.onValueChanged.AddListener(delegate {PlayerOneNameOnUpdate();});
        dropdownPlayerOne.onValueChanged.AddListener(delegate {PlayerOneDropdownOnUpdate();});
        playerTwoName.onValueChanged.AddListener(delegate {PlayerTwoNameOnUpdate();});
        dropdownPlayerTwo.onValueChanged.AddListener(delegate {PlayerTwoDropdownOnUpdate();});
    }

    // Function to check inputs and pass the data onto the game scene
    public void PlayGameButtonOnClick()
    {
        Debug.Log("Clicked + " + forcedCapture.isOn);
        topText.text = "";
        if (dropdownPlayerOne.value == 0 && playerOneName.text == "")
        {
            topText.text = "Player One Must Select or Enter a Name";
            return;
        }
        if (dropdownPlayerTwo.value == 0 && playerTwoName.text == "")
        {
            topText.text = "Player Two Must Select or Enter a Name";
            return;
        }
        if (dropdownPlayerOne.value != 0 && dropdownPlayerTwo.value != 0 & dropdownPlayerOne.value == dropdownPlayerTwo.value)
        {
            topText.text = "Players cannot have the same name";
            return;
        }
        if (playerOneName.text != "" && playerTwoName.text != "" && playerOneName.text == playerTwoName.text)
        {
            topText.text = "Players cannot have the same name";
            return;
        }

        // Load the first players name into memory
        if (dropdownPlayerOne.value != 0)
            NameStaticClass.playerOneName = RecordKeeper.listOfRecords[dropdownPlayerOne.value - 1].name;
        else
            NameStaticClass.playerOneName = playerOneName.text;
        
        // Load the second players name into memory
        if (dropdownPlayerTwo.value != 0)
            NameStaticClass.playerTwoName = RecordKeeper.listOfRecords[dropdownPlayerTwo.value - 1].name;
        else
            NameStaticClass.playerTwoName = playerTwoName.text;

        // Load the forced move option into memory
        NameStaticClass.forcedMove = forcedCapture.isOn;
        NameStaticClass.ai = false;

        RecordKeeper.AddRecord(NameStaticClass.playerOneName);
        RecordKeeper.AddRecord(NameStaticClass.playerTwoName);
        RecordKeeper.SaveData();
        SceneManager.LoadScene("GameBoard");
    }

    // Return back to main menu after clicking on this
    public void CancelGameButtonOnClick()
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

    void PlayerTwoNameOnUpdate()
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

    void PlayerTwoDropdownOnUpdate()
    {
        int value = dropdownPlayerTwo.value;
        playerTwoName.text = "";
        dropdownPlayerTwo.value = value;
    }
}
