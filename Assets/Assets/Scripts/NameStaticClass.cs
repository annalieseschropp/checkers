using System.Collections;
using System.Collections.Generic;
// using System.string;
using UnityEngine;

public class NameStaticClass : MonoBehaviour
{
    // Static variables to pass data between scenes
    public static string playerOneName;
    public static string playerTwoName;
    public static bool forcedMove;

    /// <summary>
    /// Method
    /// Swaps the existing player names, simulating the players swapping colours.
    /// </summary>
    public static void SwapPlayerNames()
    {
        string temp = playerOneName;
        playerOneName = playerTwoName;
        playerTwoName = temp;
    }
}
