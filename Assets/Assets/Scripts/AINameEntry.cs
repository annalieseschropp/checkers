using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AINameEntry : MonoBehaviour
{
    public Button playGameButton;
    public Button cancelGameButton;
    public InputField playerOneName;
    public Dropdown dropdownPlayerOne;
    public Dropdown dropdownPlayerColour;
    public Toggle forcedCapture;
    public Text topText;

    // Start is called before the first frame update
    void Start()
    {
        Button playGame = playGameButton.GetComponent<Button>();
        Button cancelGame = cancelGameButton.GetComponent<Button>();
        
        dropdownPlayerOne = dropdownPlayerOne.GetComponent<Dropdown>();
        forcedCapture = forcedCapture.GetComponent<Toggle>();
        dropdownPlayerColour = dropdownPlayerColour.GetComponent<Dropdown>();
        playerOneName = playerOneName.GetComponent<InputField>();
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

        playGame.onClick.AddListener(PlayGameButtonOnClick);
        cancelGame.onClick.AddListener(CancelGameButtonOnClick);
        playerOneName.onValueChanged.AddListener(delegate {PlayerOneNameOnUpdate();});
        dropdownPlayerOne.onValueChanged.AddListener(delegate {PlayerOneDropdownOnUpdate();});
    }

    // Function to check inputs and pass the data onto the game scene
    public void PlayGameButtonOnClick()
    {
        Debug.Log("Clicked + " + forcedCapture.isOn + " Colour: " + dropdownPlayerColour.options[dropdownPlayerColour.value].text);
        topText.text = "";
        if (dropdownPlayerOne.value == 0 && playerOneName.text == "")
        {
            topText.text = "Player Must Select or Enter a Name";
            return;
        }
        if (playerOneName.text == "Computer")
        {
            topText.text = "Player Cannot Use this Reserved Name";
            return;
        }

        // Load the first players name into memory
        if (dropdownPlayerOne.value != 0)
            NameStaticClass.playerOneName = RecordKeeper.listOfRecords[dropdownPlayerOne.value - 1].name;
        else
            NameStaticClass.playerOneName = playerOneName.text;

        // If player chooses white as their colour, set the computer to be black and the player to be white
        if (dropdownPlayerColour.options[dropdownPlayerColour.value].text != "Black")
        {
            NameStaticClass.playerTwoName = NameStaticClass.playerOneName;
            NameStaticClass.playerOneName = "Computer";
        }
        else
        {
            NameStaticClass.playerTwoName = "Computer";
        }
        // Load the forced move option into memory
        NameStaticClass.forcedMove = forcedCapture.isOn;
        NameStaticClass.ai = true;

        if (NameStaticClass.playerOneName != "Computer") 
        {
            RecordKeeper.AddRecord(NameStaticClass.playerOneName);
        }
        else 
        {
            RecordKeeper.AddRecord(NameStaticClass.playerTwoName);
        }
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

    // Functions to ensure that when the dropdown isn't blank, the input value is blank.
    void PlayerOneDropdownOnUpdate()
    {
        int value = dropdownPlayerOne.value;
        playerOneName.text = "";
        dropdownPlayerOne.value = value;
    }
}
