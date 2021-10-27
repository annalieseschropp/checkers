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
    public static List<CheckersMove.Move> GetLegalMoves(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn)
    {
        if (!IsSquareInbounds(square, boardState)) return new List<CheckersMove.Move>();

        CheckersState.State piece = GetMovablePiece(square, boardState, currentTurn);
        if(piece == CheckersState.State.Empty) return new List<CheckersMove.Move>();

        return GenerateLegalMoves(square, boardState, currentTurn);
    }

    public static List<CheckersMove.Move> MakeLegalMove(CheckersMove.Move move, CheckersState.State[,] boardstate, CheckersMove.Turn currentTurn)
    {
        // Make legal move
        // Return list of captures from new board state + location
        return new List<CheckersMove.Move>();
    }

    private static List<CheckersMove.Move> GenerateLegalMoves(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool capturesOnly)
    {
        CheckersState.State piece = GetMovablePiece(square, boardState, currentTurn);
        int direction = (currentTurn == CheckersMove.Turn.White) ? 1 : -1;
        int numMoves = piece == CheckersState.State.White || piece == CheckersState.State.Black ? 2 : 4;
        CheckersMove.Square[] vectors = new CheckersMove.Square[]
            {
                new Square(-1,1),
                new Square(1,1),
                new Square(-1,-1),
                new Square(1,-1),
            };

        List<CheckersMove.Move> moveList = new List<CheckersMove.Move>();

        if(!capturesOnly)
        {
            for(int i = 0; i < numMoves; i++)
            {
                if(IsSquareOccupied(square + direction * vectors[i], boardState)) continue;
                CheckersMove.Move newMove = new CheckersMove.Move(square, direction * vectors[i]);
                moveList.Add(newMove);
            }
        }

        for(int i = 0; i < numMoves; i++)
        {
            if(!IsSquareEnemy(square + direction * vectors[i], boardState, currentTurn)) continue;
            if(IsSquareOccupied(square + 2 * direction * vectors[i], boardState)) continue;

            CheckersMove.Move newMove = new CheckersMove.Move(square, 2 * direction * vectors[i]);
            moveList.Add(newMove);
        }

        return moveList;
    }

    private static List<CheckersMove.Move> GenerateLegalMoves(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn)
    {
        return GenerateLegalMoves(square, boardState, currentTurn, false);
    }

    private static List<CheckersMove.Move> GenerateLegalCaptures(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn)
    {
        return GenerateLegalMoves(square, boardState, currentTurn, true);
    }

    private static bool IsSquareInbounds(CheckersMove.Square square, CheckersState.State[,] boardState)
    {
        // is the board treated as [0] = x or [1] = x?
        if(square.x < 0 || square.x >= boardState.GetLength(0)) return false;
        if(square.y < 0 || square.y >= boardState.GetLength(1)) return false;
        return true;
    }

    private static bool IsSquareOccupied(CheckersMove.Square square, CheckersState.State[,] boardState)
    {
        return !(GetLegalPiece(square, boardState, CheckersMove.Turn.White) == CheckersState.State.Empty);
    }

    private static bool IsSquareEnemy(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn)
    {
        if(currentTurn == CheckersMove.Turn.White) return !(GetMovablePiece(square, boardState, CheckersMove.Turn.Black) == CheckersState.State.Empty);
        else if(currentTurn == CheckersMove.Turn.Black) return !(GetMovablePiece(square, boardState, CheckersMove.Turn.White) == CheckersState.State.Empty);
        else return false;
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
