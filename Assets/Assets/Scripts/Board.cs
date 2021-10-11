using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class
/// Models the 2D game board for checkers
/// </summary>
public class Board : MonoBehaviour
{
    public GameObject blackTilePrefab;
    public GameObject whiteTilePrefab;

    /// <summary>
    /// Enum
    /// Holds the state of each tile.
    /// </summary>
    private enum State {
        Empty = 0,
        White = 1,
        Black = 2,
        WhiteKing = 3,
        BlackKing = 4
    };

    /// <summary>
    /// Constructor of board class.
    /// </summary>
    void Start()
    {
        Create();
    }

    /// <summary>
    /// Method
    /// Creates instance of board and populates with alternating coloured tiles.
    /// <params>None</params>
    /// <returns>Void</returns>
    /// </summary>
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
                Vector2 tempPosition = new Vector2(x, y);
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
}
