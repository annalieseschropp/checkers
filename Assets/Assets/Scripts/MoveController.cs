using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CheckersState;

/// <summary>
/// Class
/// Helps control the process of making moves
/// </summary>
public class MoveController
{
    CheckersState.State[,] boardState;
    CheckersMove.Square? selectedSquare;
    CheckersMove.Turn currentTurn;
    List<CheckersMove.Move> legalMoves;
    bool isMulticaptureInProgress;

    public MoveController()
    {
        selectedSquare = null;
        currentTurn = CheckersMove.Turn.White;
        legalMoves = new List<CheckersMove.Move>();
        isMulticaptureInProgress = false;
    }

    public MoveController(ref CheckersState.State[,] boardStateRef)
    {
        boardState = boardStateRef;
        selectedSquare = null;
        currentTurn = CheckersMove.Turn.White;
        legalMoves = new List<CheckersMove.Move>();
        isMulticaptureInProgress = false;
    }

    public void RestartGame(ref CheckersState.State[,] boardStateRef)
    {
        boardState = boardStateRef;
        selectedSquare = null;
        currentTurn = CheckersMove.Turn.White;
        legalMoves = new List<CheckersMove.Move>();
        isMulticaptureInProgress = false;
    }

    public List<CheckersMove.Move> SelectPiece(CheckersMove.Square square)
    {
        if (isMulticaptureInProgress) return legalMoves;

        selectedSquare = square;

        if(selectedSquare is CheckersMove.Square validSquare)
        {
            legalMoves = LegalMoveGenerator.GetLegalMoves(validSquare, boardState, currentTurn);
        }
        else
        {
            selectedSquare = null;
            legalMoves.Clear();
        }

        return legalMoves;
    }

    public void DeselectPiece()
    {
        if(isMulticaptureInProgress) return;
        selectedSquare = null;
    }

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

    public void DeclineMulticapture()
    {
        if(isMulticaptureInProgress)
        {
            isMulticaptureInProgress = false;
            SwapTurns();
        }
    }

    private void SwapTurns()
    {
        currentTurn = currentTurn == CheckersMove.Turn.White ? CheckersMove.Turn.Black : CheckersMove.Turn.White;
        isMulticaptureInProgress = false;
        DeselectPiece();
    }

    /// Getters
    public CheckersMove.Square? GetSelectedSquare()
    {
        return selectedSquare;
    }

    public CheckersMove.Turn? GetCurrentTurn()
    {
        return currentTurn;
    }

    public List<CheckersMove.Move> GetLegalMoves()
    {
        return legalMoves;
    }

    public bool IsMulticaptureInProgress()
    {
        return isMulticaptureInProgress;
    }
}
