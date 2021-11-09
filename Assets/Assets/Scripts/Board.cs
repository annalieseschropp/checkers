using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private bool animationInProgress;
    private bool updatedRecord;

    //For the final screen popup
    public GameObject popup;

    /// <summary>
    /// Board initialization performed before anything can access it.
    /// </summary>
    void Awake()
    {
        curState = new CheckersState.State[8, 8];
        Create();
        SetInitBoardState();
        moveController = new MoveController(ref curState, NameStaticClass.forcedMove);
        selected = null;
        possibleMoves = new List<GameObject>();
        animationInProgress = false;
        updatedRecord = false;
    }

    /// <summary>
    /// Board actions performed on the first frame update, after Awake.
    /// </summary>
    void Start()
    {
        AddStandardStartingPieces();
        InitPieceSet();
        InitPieceDisplay();
        moveController.RestartGame(ref curState, NameStaticClass.forcedMove); // Set last parameter to true to force captures
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

    /// <summary>
    /// Checks for updates on input from user
    /// </summary>
    void Update () {
        MoveControl();
        CheckForEndGame();
    }

    void CheckForEndGame()
    {
        if(animationInProgress) return;

        popup = GameObject.Find("endGameElement");
        GameObject popupChild = popup.transform.GetChild(0).gameObject;    
        //GameObject textObj = popupChild.transform.GetChild(2).gameObject;   
        Text newText = popupChild.GetComponentInChildren<Text>();

        
        CheckersMove.GameStatus gameState = moveController.GetGameStatus();
        
        
        if (gameState == CheckersMove.GameStatus.WhiteWin)
        {
            newText.text = "White won " + moveController.CountWhitePiecesRemaining().ToString() + " to " + moveController.CountBlackPiecesRemaining().ToString() +"!";
            if (!updatedRecord)
            {
                RecordKeeper.UpdateRecordWon(NameStaticClass.playerTwoName);
                RecordKeeper.UpdateRecordLost(NameStaticClass.playerOneName);
                RecordKeeper.SaveData();
                updatedRecord = true;
            }
            popupChild.SetActive(true);

        }
        else if (gameState == CheckersMove.GameStatus.BlackWin)
        {
            newText.text = "Black won " + moveController.CountBlackPiecesRemaining().ToString() + " to " + moveController.CountWhitePiecesRemaining().ToString() +"!";
            if (!updatedRecord)
            {
                RecordKeeper.UpdateRecordWon(NameStaticClass.playerOneName);
                RecordKeeper.UpdateRecordLost(NameStaticClass.playerTwoName);
                RecordKeeper.SaveData();
                updatedRecord = true;
            }
            popupChild.SetActive(true);
        }
        else if (gameState == CheckersMove.GameStatus.Draw)
        {
            newText.text = "It was a draw!";
            popupChild.SetActive(true);
        };
        
    }

    /// <summary>
    /// Method
    /// Displays basic logic for how to use the moveController class
    /// </summary>
    private void MoveControl()
    {
        if(animationInProgress) return;

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
            Vector3 tempPosition = new Vector3(x, y, 1);

            // We have a selected square
            if (selected != null && moveController.GetSelectedSquare() is CheckersMove.Square selectedSquare)
            {
                // MakeMove will return false if our move is not legal. We use this here to deselect a piece
                if (moveController.MakeMove(clickedSquare))
                {
                    animationInProgress = true;
                    CleanSelection();
                    StartCoroutine(pieceSet.MakeMove(curState, new CheckersMove.Move(selectedSquare, clickedSquare), ()=>
                        {
                            if (moveController.IsMulticaptureInProgress())
                            {
                                tempPosition = new Vector3(clickedSquare.x, clickedSquare.y, 1);
                                DisplaySelection(tempPosition, clickedSquare);
                            }
                            animationInProgress = false;
                        }
                    ));
                }
                else
                {
                    CleanSelection();
                    if (moveController.IsMulticaptureInProgress())
                    {
                        moveController.DeclineMulticapture(); // This does nothing if captures are forced
                    }
                    else
                    {
                        moveController.DeselectPiece();
                    }
                }

            }
            // UI for selecting a piece
            else
            {
                CheckersMove.Turn curTurn = moveController.GetCurrentTurn();

                // Check if clickedSquare contains a piece and if it does, show selection prefab and select piece
                if(x < 0 || y < 0 || x > curState.GetLength(0) || y > curState.GetLength(0)) return;
                
                if (curState[x, y] != CheckersState.State.Empty)
                {
                    // Ensure that piece is of the correct players colour based on current turn
                    if ((curTurn == CheckersMove.Turn.White && (curState[x, y] == CheckersState.State.White | curState[x, y] == CheckersState.State.WhiteKing)) 
                        | (curTurn == CheckersMove.Turn.Black && (curState[x, y] == CheckersState.State.Black | curState[x, y] == CheckersState.State.BlackKing)))
                    {
                        DisplaySelection(tempPosition, clickedSquare);
                    }
                }
                
            }
        }
    }

    /// <summary>
    /// Method
    /// Helper to display the selections available based on a clicked piece
    /// </summary>
    /// <param name="position">current vector position user clicked on</param>
    /// <param name="clickedSquare">the square player selected </param>
    private void DisplaySelection(Vector3 position, CheckersMove.Square clickedSquare)
    {
        moveController.SelectPiece(clickedSquare);

        // Display piece that has been selected
        selected = Instantiate(selectedPiecePrefab, position, Quaternion.identity) as GameObject;

        // Get legal moves the selected piece can do and display those options
        List<CheckersMove.Move> legalMoves = moveController.GetLegalMoves();

        if(moveController.GetSelectedSquare() == clickedSquare)
        {
            for (int i = 0; i < legalMoves.Count; i++)
            {
                var possibleMove = Instantiate(highlightedTilePrefab, new Vector3(legalMoves[i].dest.x, legalMoves[i].dest.y, 3), Quaternion.identity) as GameObject;
                possibleMoves.Add(possibleMove);
            }
        }
    }

    /// <summary>
    /// Method
    /// Helper to remove currently selected piece and legal moves
    /// </summary>
    private void CleanSelection()
    {
        // Deselect the piece and remove the possible moves
        Destroy(selected);
        foreach (var move in possibleMoves)
        {
            Destroy(move);
        }
    }
}
