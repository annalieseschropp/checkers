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
        return new CheckersMove.Move[0];
    }
}
