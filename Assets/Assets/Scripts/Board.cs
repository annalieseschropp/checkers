using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CheckersState;
using CheckersMove;

/// <summary>
/// Class
/// Models the 2D game board for checkers
/// </summary>
public class Board : MonoBehaviour 
{
    public GameObject blackTilePrefab;
    public GameObject whiteTilePrefab;
    public GameObject selectedPiecePrefab;
    public GameObject highlightedTilePrefab;
    public CheckersState.State[,] curState;

    private PieceSet pieceSet;
    private MoveController moveController;
    private GameObject selected;
    private List<GameObject> possibleMoves;

    /// <summary>
    /// Board initialization performed before anything can access it.
    /// </summary>
    void Awake()
    {
        curState = new CheckersState.State[8, 8];
        Create();
        SetInitBoardState();
        moveController = new MoveController(ref curState);
        selected = null;
        possibleMoves = new List<GameObject>();
    }

    /// <summary>
    /// Board actions performed on the first frame update, after Awake.
    /// </summary>
    void Start()
    {
        AddStandardStartingPieces();
        InitPieceSet();
        InitPieceDisplay();
        moveController.RestartGame(ref curState, false); // Set last parameter to true to force captures
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

        // Populates vertically (i.e. 0,0 is the bottom left corner 0,1 is the tile above that etc.)
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

    /// 
    /// *** Sample code for displaying the usage of the moveController class
    /// *** all of the code below should be removed when proper UI is added
    ///
    void Update () {
        MoveControl();
    }

    /// <summary>
    /// Method
    /// Displays basic logic for how to use the moveController class
    /// </summary>
    private void MoveControl()
    {

        // Check if the game is over
        if(moveController.GetGameStatus() != CheckersMove.GameStatus.InProgress)
        {
            Debug.Log("Game Status: " + moveController.GetGameStatus());
        }
        // If not, try to make move
        else if(Input.anyKeyDown)
        {
            // Brute force click position because we have no UI yet
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = (int)System.Math.Round(worldPosition.x);
            int y = (int)System.Math.Round(worldPosition.y);
            CheckersMove.Square clickedSquare = new CheckersMove.Square(x, y);
            Vector3 tempPosition = new Vector3(x, y, 3);


            // Emulate UI, where we have a square selected
            if (moveController.GetSelectedSquare() is CheckersMove.Square selectedSquare)
            {
                // MakeMove will return false if our move is not legal. We use this here to emulate UI for deselecting a piece
                if (moveController.MakeMove(clickedSquare))
                {
                    pieceSet.MakeMove(curState, new CheckersMove.Move(selectedSquare, clickedSquare));
                }
                else
                {
                    // Pretend we "deselected" with the UI
                    if (moveController.IsMulticaptureInProgress())
                    {
                        moveController.DeclineMulticapture(); // This does nothing if captures are forced
                    }
                    else
                    {
                        moveController.DeselectPiece();
                    }
                }
                Destroy(selected);
                foreach(var move in possibleMoves)
                {
                    Destroy(move);
                }
               
            }
            // UI for selecting a piece
            else
            {
                CheckersMove.Turn curTurn = moveController.GetCurrentTurn();

                // Check if clickedSquare contains a piece and if it does, show selection prefab and select piece
                if (curState[x, y] != CheckersState.State.Empty)
                {
                    // Ensure that piece is of the correct players colour based on current turn
                    if ((curTurn == CheckersMove.Turn.White && (curState[x, y] == CheckersState.State.White | curState[x, y] == CheckersState.State.WhiteKing)) 
                        | (curTurn == CheckersMove.Turn.Black && (curState[x, y] == CheckersState.State.Black | curState[x, y] == CheckersState.State.BlackKing)))
                    {
                        selected = Instantiate(selectedPiecePrefab, tempPosition, Quaternion.identity) as GameObject;
                        moveController.SelectPiece(clickedSquare);
                        List<CheckersMove.Move> legalMoves = moveController.GetLegalMoves();
                        for (int i = 0; i < legalMoves.Count; i++)
                        {
                            tempPosition = new Vector3(legalMoves[i].dest.x, legalMoves[i].dest.y, 3);
                            var possibleMove = Instantiate(highlightedTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                            possibleMoves.Add(possibleMove);
                        }
                    }
                }
                
            }
        }
    }
}
