using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckersState;

/// <summary>
/// Class
/// Models the 2D game board for checkers
/// </summary>
public class Board : MonoBehaviour
{
    public GameObject blackTilePrefab;
    public GameObject whiteTilePrefab;
    public CheckersState.State[,] curState;

    /// <summary>
    /// Constructor of board class.
    /// </summary>
    void Start()
    {
        curState = new CheckersState.State[8, 8];
        Create();
        setInitBoardState();
        Debug.Log(getBoardState()[0,0]);
    }

    /// <summary>
    /// Method
    /// Creates instance of board and populates with alternating coloured tiles.
    /// </summary>
    /// <params>None</params>
    /// <returns>Void</returns>
    private void Create()
    {
        bool isBlack = true;
        GameObject tile;

        // Populates vertically (i.e. 0,0 is the bottom left corner 0,1 is the tile above that etc.),
        // flip the x and y values if horizontally is easier. REMOVE THIS COMMENT AFTER VERDICT
        for (int x = 0; x < 8; x++) // X Axis
        {
            for (int y = 0; y < 8; y++) // Y Axis
            {
                Vector3 tempPosition = new Vector3(x, y, -1);
                if (isBlack) // If black then instantiate a black tile object
                {
                    tile = Instantiate(blackTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                } else // If not black then instantiate a white tile object
                {
                    tile = Instantiate(whiteTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                }
                tile.transform.parent = this.transform;
                tile.name = $"( {x}, {y} )";
                isBlack = !isBlack;
            }
            isBlack = !isBlack; // Offset next column to ensure checkboard pattern
        }
    }

    /// <summary>
    /// Method
    /// Setter to populate current state array for each position on the board with an empty state 
    /// as there are no pieces on the board yet.
    /// </summary>
    /// <params>None</params>
    /// <returns>Void</returns>
    private void setInitBoardState()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                curState[x, y] = CheckersState.State.Empty;
            }
        }
    }

    /// <summary>
    /// Method
    /// Getter for the overall board state
    /// </summary>
    /// <params>None</params>
    /// <returns>curState - 2D array of State enums</returns>
    public CheckersState.State[,] getBoardState()
    {
        return curState;
    }
}
