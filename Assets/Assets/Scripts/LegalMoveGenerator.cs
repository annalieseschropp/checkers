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
    public static List<CheckersMove.Move> GetLegalMoves(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool forceCapture = false)
    {
        if (!IsSquareInbounds(square, boardState)) return new List<CheckersMove.Move>();

        CheckersState.State piece = GetMovablePiece(square, boardState, currentTurn);
        if(piece == CheckersState.State.Empty) return new List<CheckersMove.Move>();

        if(forceCapture)
        {
            List<CheckersMove.Move> allLegalMoves = GetAllLegalMoves(boardState, currentTurn, true);
            if(allLegalMoves.Count > 0)
            {
                Debug.Log("Gnerating legal captures");
                return GenerateLegalCaptures(square, boardState, currentTurn);
            }
        }

        return GenerateLegalMoves(square, boardState, currentTurn);
    }

    public static List<CheckersMove.Move> MakeLegalMove(CheckersMove.Move move, ref CheckersState.State[,] boardstate, CheckersMove.Turn currentTurn)
    {
        // Make legal move
        List<CheckersMove.Move> multicaptures = new List<CheckersMove.Move>();

        boardstate[move.dest.x, move.dest.y] = GetDestinationPiece(move, boardstate, currentTurn);
        boardstate[move.src.x, move.src.y] = CheckersState.State.Empty;

        if(IsMoveACapture(move))
        {
            CheckersMove.Square capturedSquare = GetCaptureSquare(move);
            boardstate[capturedSquare.x, capturedSquare.y] = CheckersState.State.Empty;
            multicaptures = GenerateLegalCaptures(move.dest, boardstate, currentTurn);
        }

        return multicaptures;
    }

    public static CheckersMove.GameStatus GetGameStatus(CheckersState.State[,] boardstate, CheckersMove.Turn currentTurn)
    {
        List<CheckersMove.Move> currentSideMoves = GetAllLegalMoves(boardstate, currentTurn);
        if(currentSideMoves.Count > 0)
        {
            return CheckersMove.GameStatus.InProgress;
        }
        else
        {
            CheckersMove.Turn otherTurn = currentTurn == CheckersMove.Turn.White ? CheckersMove.Turn.Black : CheckersMove.Turn.White;
            List<CheckersMove.Move> otherSideMoves = GetAllLegalMoves(boardstate, otherTurn);
            if(otherSideMoves.Count > 0)
            {
                return otherTurn == CheckersMove.Turn.White ? CheckersMove.GameStatus.WhiteWin : CheckersMove.GameStatus.BlackWin;
            }
            else
            {
                return CheckersMove.GameStatus.Draw;
            }
        }
    }

    public static List<CheckersMove.Move> GetAllLegalMoves(CheckersState.State[,] boardstate, CheckersMove.Turn currentTurn, bool capturesOnly = false)
    {
        List<CheckersMove.Move> allMoves = new List<CheckersMove.Move>();
        for(int x = 0; x < boardstate.GetLength(0); x++)
        {
            for(int y = 0; y < boardstate.GetLength(1); y++)
            {
                allMoves.AddRange(GenerateLegalMoves(new CheckersMove.Square (x,y), boardstate, currentTurn, capturesOnly));
            }
        }

        return allMoves;
    }

    private static CheckersState.State GetDestinationPiece(CheckersMove.Move move, CheckersState.State[,] boardstate, CheckersMove.Turn currentTurn)
    {
        CheckersState.State piece = GetMovablePiece(move.src, boardstate, currentTurn);
        if(piece == CheckersState.State.White && move.dest.y == boardstate.GetLength(1) - 1) return CheckersState.State.WhiteKing;
        else if(piece == CheckersState.State.Black && move.dest.y == 0) return CheckersState.State.BlackKing;
        return piece;
    }

    private static bool IsMoveACapture(CheckersMove.Move move)
    {
        return System.Math.Abs((move.src - move.dest).x) == 2;
    }

    private static CheckersMove.Square GetCaptureSquare(CheckersMove.Move move)
    {
        return (move.src + move.dest)/2;
    }

    private static List<CheckersMove.Move> GenerateLegalMoves(CheckersMove.Square square, CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool capturesOnly = false)
    {
        CheckersState.State piece = GetMovablePiece(square, boardState, currentTurn);

        if (piece == CheckersState.State.Empty) return new List<CheckersMove.Move>();

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
                if(!IsSquareInbounds(square + direction * vectors[i], boardState)) continue;
                if(IsSquareOccupied(square + direction * vectors[i], boardState)) continue;
                CheckersMove.Move newMove = new CheckersMove.Move(square, square + direction * vectors[i]);
                moveList.Add(newMove);
            }
        }

        for(int i = 0; i < numMoves; i++)
        {
            if(!IsSquareInbounds(square + direction * vectors[i], boardState)) continue;
            if(!IsSquareInbounds(square + 2 * direction * vectors[i], boardState)) continue;
            if(!IsSquareEnemy(square + direction * vectors[i], boardState, currentTurn)) continue;
            if(IsSquareOccupied(square + 2 * direction * vectors[i], boardState)) continue;

            CheckersMove.Move newMove = new CheckersMove.Move(square, square + 2 * direction * vectors[i]);
            moveList.Add(newMove);
        }

        return moveList;
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
