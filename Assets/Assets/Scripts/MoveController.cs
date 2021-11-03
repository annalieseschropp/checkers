using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CheckersState;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class
/// Helps control the process of making moves. Keeps track of the "game state",
/// while acting as an interface for the LegalMoveGenerator static class.
/// </summary>
public class MoveController
{
    CheckersState.State[,] boardState;
    CheckersMove.Square? selectedSquare;
    CheckersMove.Turn currentTurn;
    List<CheckersMove.Move> legalMoves;
    int startingWhitePieceCount;
    int startingBlackPieceCount;
    bool isMulticaptureInProgress;
    bool forceCaptures;

    [SerializedField] Button _button1;
    [SerializedField] Button _button2;
    [SerializedField] Text _ScoreText;


    //Everything for the endgame popup


    /// <summary>
    /// Method
    /// Initialization method. Can be reused in constructors.
    /// </summary>
    private void InitMembers(bool forceCapturesRule = false)
    {
        selectedSquare = null;
        currentTurn = CheckersMove.Turn.Black;
        legalMoves = new List<CheckersMove.Move>();
        isMulticaptureInProgress = false;
        forceCaptures = forceCapturesRule;
        startingWhitePieceCount = CountWhitePiecesRemaining();
        startingBlackPieceCount = CountBlackPiecesRemaining();
    }

    /// <summary>
    /// Method
    /// Basic constructor.
    /// </summary>
    public MoveController(ref CheckersState.State[,] boardStateRef, bool forceCapturesRule = false)
    {
        boardState = boardStateRef;
        InitMembers(forceCapturesRule);
    }

    /// <summary>
    /// Method
    /// Resets the selected piece, turn, legal moves, and prepares for a new game.
    /// </summary>
    public void RestartGame(ref CheckersState.State[,] boardStateRef, bool forceCapturesRule = false)
    {
        boardState = boardStateRef;
        InitMembers(forceCapturesRule);
    }

    /// <summary>
    /// Method
    /// Selects a piece and returns its legal moves.
    /// If a multicapture is in progress, this method makes no updates,
    /// and continues selecting the multicapturing piece.
    /// </summary>
    public List<CheckersMove.Move> SelectPiece(CheckersMove.Square square)
    {
        if (isMulticaptureInProgress) return legalMoves;

        selectedSquare = square;

        if(selectedSquare is CheckersMove.Square validSquare)
        {
            legalMoves = LegalMoveGenerator.GetLegalMoves(validSquare, boardState, currentTurn, forceCaptures);
        }
        else
        {
            selectedSquare = null;
            legalMoves.Clear();
        }

        return legalMoves;
    }

    /// <summary>
    /// Method
    /// Deselects a piece and resets the legal moves list.
    /// If a multicapture is in progress, this method makes no updates,
    /// and continues selecting the multicapturing piece.
    /// </summary>
    public void DeselectPiece()
    {
        if(isMulticaptureInProgress) return;
        selectedSquare = null;
        legalMoves.Clear();
    }

    /// <summary>
    /// Method
    /// Attempts to move the currently selected piece to the square passed in.
    /// Will return true and perform the move if and only if the move is legal.
    /// Alsp detects if a multicapture is available.
    /// </summary>
    public bool MakeMove(CheckersMove.Square destSquare)
    {
        if(selectedSquare is CheckersMove.Square srcSquare)
        {
            CheckersMove.Move move = new CheckersMove.Move(srcSquare, destSquare);
            if(legalMoves.Contains(move))
            {
                List<CheckersMove.Move> multicaptures = LegalMoveGenerator.MakeLegalMove(move, ref boardState, currentTurn);
                isMulticaptureInProgress = false;

                if(multicaptures.Count > 0)
                {
                    SelectPiece(move.dest);
                    isMulticaptureInProgress = true;
                    legalMoves = multicaptures;
                }
                else
                {
                    isMulticaptureInProgress = false;
                    SwapTurns();
                }

                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// Method
    /// Declines the option to multicapture and updates state accordingly.
    /// Multicaptures cannot be delclined if capturing is forced; in that case,
    /// the method does nothing.
    /// </summary>
    public void DeclineMulticapture()
    {
        if(!forceCaptures && isMulticaptureInProgress)
        {
            isMulticaptureInProgress = false;
            SwapTurns();
        }
    }

    /// <summary>
    /// Method
    /// Helper method for handling state change upon switching turns.
    /// </summary>
    private void SwapTurns()
    {
        currentTurn = currentTurn == CheckersMove.Turn.White ? CheckersMove.Turn.Black : CheckersMove.Turn.White;
        isMulticaptureInProgress = false;
        DeselectPiece();
    }

    /// <summary>
    /// Method
    /// Getter for the currently selected square, which may be null.
    /// </summary>
    public CheckersMove.Square? GetSelectedSquare()
    {
        return selectedSquare;
    }

    /// <summary>
    /// Method
    /// Getter for the current turn.
    /// </summary>
    public CheckersMove.Turn GetCurrentTurn()
    {
        return currentTurn;
    }

    /// <summary>
    /// Method
    /// Returns the list of legal moves associated with the selected piece.
    /// </summary>
    public List<CheckersMove.Move> GetLegalMoves()
    {
        return legalMoves;
    }

    /// <summary>
    /// Method
    /// Returns whether or not we are in the middle of a multicapture.
    /// </summary>
    public bool IsMulticaptureInProgress()
    {
        return isMulticaptureInProgress;
    }

    /// <summary>
    /// Method
    /// Returns a status represeting who won the game: {the game is still in progress, white won, black won, it's a draw}.
    /// This status is calculated by the LegalMoveGenerator, so no manual calcualtion is required. However, be sure to
    /// Check this status at the start of each turn, or else a player may be left with no legal moves.
    /// </summary>
    public CheckersMove.GameStatus GetGameStatus()
    {


        return LegalMoveGenerator.GetGameStatus(boardState, currentTurn);
    }

    /// <summary>
    /// Method
    /// Counts the number of white pieces still on the board.
    /// </summary>
    public int CountWhitePiecesRemaining()
    {
        int count = 0;
        for(int i = 0; i < boardState.GetLength(0); i++ )
        {
            for(int j = 0; j < boardState.GetLength(1); j++ )
            {  
                if(boardState[i,j] == CheckersState.State.White || boardState[i,j] == CheckersState.State.WhiteKing)
                {
                    count++;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// Method
    /// Counts the number of black pieces still on the board.
    /// </summary>
    public int CountBlackPiecesRemaining()
    {
        int count = 0;
        for(int i = 0; i < boardState.GetLength(0); i++ )
        {
            for(int j = 0; j < boardState.GetLength(1); j++ )
            {  
                if(boardState[i,j] == CheckersState.State.Black || boardState[i,j] == CheckersState.State.BlackKing)
                {
                    count++;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// Method
    /// Counts the number of white pieces that have been captured.
    /// </summary>
    public int countWhitePiecesLost()
    {
        return startingWhitePieceCount - CountWhitePiecesRemaining();
    }

    /// <summary>
    /// Method
    /// Counts the number of black pieces that have been captured.
    /// </summary>
    public int countBlackPiecesLost()
    {
        return startingBlackPieceCount - CountBlackPiecesRemaining();
    }
}
