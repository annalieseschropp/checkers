using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardNameDisplay : MonoBehaviour
{
    public Text playerOne;
    public Text playerTwo;
    // Start is called before the first frame update
    void Start()
    {
        playerOne.text = NameStaticClass.playerOneName;
        playerTwo.text = NameStaticClass.playerTwoName;
    }
}
