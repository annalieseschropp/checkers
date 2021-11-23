using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CheckersState;
using CheckersMove;

/// <summary>
/// Abstract Class
/// A one-method abstract class defining the interface needed for an AI player.
/// The class extends monobehaviour so it can be selected in the inspector.
/// </summary>
public abstract class AIPlayer : MonoBehaviour
{
    /// <summary>
    /// Method
    /// Expected to return the move that the AI would like to make. How you decide this move is up to you.
    /// Important Notes:
    /// ** <boardState> is the current state of the board.
    /// ** There is no need to update the <boardState>: that will be done automatically after you return a move.
    /// ** <currentTurn> is colour of the current side to move; this will always be the AI's colour.
    /// ** <forceCapture> indicates whether the game is being played with force capture rules on.
    /// ** <multicaptureSquare> having a valid square indicates a multicapture in progress, where the piece is on the given square.
    /// ** <multicaptureSquare> being null indicates no multicapture is in progress.
    /// ** returning "null" indicates declining a multicapture
    /// </summary>
    public abstract CheckersMove.Move? GetAIMove(CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool forceCapture, CheckersMove.Square? multicaptureSquare = null);
}