using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private PieceSet pieceSet;
    private MoveController moveController;

    /// <summary>
    /// Board initialization performed before anything can access it.
    /// </summary>
    void Awake()
    {
        curState = new CheckersState.State[8, 8];
        Create();
        SetInitBoardState();
        moveController = new MoveController();
    }

    /// <summary>
    /// Board actions performed on the first frame update, after Awake.
    /// </summary>
    void Start()
    {
        AddStandardStartingPieces();
        InitPieceSet();
        InitPieceDisplay();
        moveController.RestartGame(ref curState);
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
    private void SetInitBoardState()
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
    /// Sets the reference to the piece set that this board manipulates.
    /// </summary>
    /// <params>None</params>
    /// <returns>Void</returns>
    private void InitPieceSet()
    {
        pieceSet = GameObject.Find("PieceSet").GetComponent(typeof(PieceSet)) as PieceSet;
    }

    /// <summary>
    /// Method
    /// Setter to populate current state array with the standard starting position of pieces.
    /// </summary>
    /// <params>None</params>
    /// <returns>Void</returns>
    private void AddStandardStartingPieces()
    {
        for (int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 3; y++)
            {
                if((x+y)%2 == 0)
                {
                    curState[x, y] = CheckersState.State.White;
                    curState[7-x, 7-y] = CheckersState.State.Black;
                }
            }
        }
    }

    /// <summary>
    /// Method
    /// Initializes the visible display of pieces based on the board state.
    /// </summary>
    /// <params>None</params>
    /// <returns>Void</returns>
    private void InitPieceDisplay()
    {
        pieceSet.SetInitialBoardState(curState);
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

    /// *** Sample code 
    void Update () {
        SampleMoveControl();
    }

    private void SampleMoveControl()
    {
        if(Input.anyKeyDown)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckersMove.Square clickedSquare = new CheckersMove.Square((int)System.Math.Round(worldPosition.x), (int)System.Math.Round(worldPosition.y));

            if(moveController.GetSelectedSquare() is CheckersMove.Square selectedSquare)
            {
                if (moveController.MakeMove(clickedSquare))
                {
                    pieceSet.MakeMove(curState, new CheckersMove.Move(selectedSquare, clickedSquare));
                }
                else
                {
                    if(moveController.IsMulticaptureInProgress())
                    {
                        moveController.DeclineMulticapture();
                    }
                    else
                    {
                        moveController.DeselectPiece();
                    }
                }
            }
            else
            {
                moveController.SelectPiece(clickedSquare);
            }
        }
    }
}
