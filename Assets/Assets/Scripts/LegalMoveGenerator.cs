using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CheckersState;
using CheckersMove;

/// <summary>
/// Class
/// Models the 2D game board for checkers
/// </summary>
public static class LegalMoveGenerator
{
    public static CheckersMove.Move[] GetLegalMoves(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn)
    {
        if (!IsSquareInbounds(square, boardState)) return new CheckersMove.Move[0];

        CheckersState.State piece = GetMovablePiece(square, boardState, currentTurn);
        if(piece == CheckersState.State.Empty) return new CheckersMove.Move[0];
        
        return new CheckersMove.Move[0];
    }

    public static CheckersMove.Move[] MakeLegalMove(CheckersMove.Move move, CheckersState.State[,] boardstate, CheckersMove.Turn currentTurn)
    {
        return new CheckersMove.Move[0];
    }

    private static bool IsSquareInbounds(CheckersMove.Square square, CheckersState.State[,] boardState)
    {
        // is the board treated as [0] = x or [1] = x?
        if(square.x < 0 || square.x >= boardState.GetLength(0)) return false;
        if(square.y < 0 || square.y >= boardState.GetLength(1)) return false;
        return true;
    }

    private static CheckersState.State GetMovablePiece(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn)
    {
        return GetLegalPiece(square, boardState, currentTurn, true);
    }

    private static CheckersState.State GetLegalPiece(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn)
    {
        return GetLegalPiece(square, boardState, currentTurn, false);
    }

    private static CheckersState.State GetLegalPiece(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool movableOnly)
    {
        if(!IsSquareInbounds(square, boardState)) throw new System.ArgumentException("Square is out of bounds");
        
        CheckersState.State piece = boardState[square.x, square.y];
        switch (currentTurn)
        {
            case CheckersMove.Turn.GameOver:
                return CheckersState.State.Empty;

            case CheckersMove.Turn.White:
                if(movableOnly && !(piece == CheckersState.State.White || piece == CheckersState.State.WhiteKing)) return CheckersState.State.Empty;
                break;

            case CheckersMove.Turn.Black:
                if(movableOnly && !(piece == CheckersState.State.Black || piece == CheckersState.State.BlackKing)) return CheckersState.State.Empty;
                break;

            default:
                return CheckersState.State.Empty;
        }
        
        return piece;
    }
}
