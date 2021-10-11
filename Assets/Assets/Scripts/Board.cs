using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject blackTilePrefab;
    public GameObject whiteTilePrefab;
    private Tile[,] boardTiles;

    void Start()
    {
        boardTiles = new Tile[8, 8]; // Create new instance of 8x8 board
        Create();
    }

    private void Create()
    {
        bool isBlack = true;
        for (int x = 0; x < 8; x++) // X Axis
        {
            for (int y = 0; y < 8; y++) // Y Axis
            {
                Vector2 tempPosition = new Vector2(x, y);
                if (isBlack) // If black then instantiate a black tile object
                {
                    Instantiate(blackTilePrefab, tempPosition, Quaternion.identity);
                } else // If not black then instantiate a white tile object
                {
                    Instantiate(whiteTilePrefab, tempPosition, Quaternion.identity);
                }
                isBlack = !isBlack;
            }
            isBlack = !isBlack; // Offset next column to ensure checkboard pattern
        }
    }
}
