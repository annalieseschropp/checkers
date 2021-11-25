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
/// This is a sample AI demonstrating how to create an AI.
/// ** Extend the AIPlayer class
/// ** Override the <GetAIMove> method
/// ** Add a dummy object in the GameBoard scene as a child of the "AIPlayers" object.
/// ** Add the script containing your extension of the AIPlayer class to that object.
/// ** To have the board class use your AI, click on "Board" in the inspector in the GameBoard scene.
/// ** Set the Board's <MyAIPlayer> option to the object corresponding with your AI.
/// ** Start a 1Player game, and your AI will be used.
/// </summary>
public class SampleAI : AIPlayer
{
    /// <summary>
    /// Method
    /// This is a sample AI move algorithm that just picks a random move.
    /// </summary>
    override public CheckersMove.Move? GetAIMove(CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool forceCapture, CheckersMove.Square? multicaptureSquare = null)
    {
        List<CheckersMove.Move> legalMoveList;

        /*
            If there is a multicapture in progress (signalled by <multicaptureSquare> not being null)
            then we choose a random move with that piece.
        */
        if (multicaptureSquare is CheckersMove.Square multiSquare)
        {
            // Get the legal moves for this piece. This will account for forced capture automatically.
            legalMoveList = LegalMoveGenerator.GetLegalMoves(multiSquare, boardState, currentTurn, forceCapture);

            // Include a chance to decline the multicapture, if that is allowed
            if(!forceCapture && Random.Range(-1,legalMoveList.Count) == -1) return null;
        }
        /*
            Else choose any random move.
        */
        else
        {
            // Get the legal moves for all pieces. This will account for forced capture automatically.
            legalMoveList = LegalMoveGenerator.GetLegalMoves(boardState, currentTurn, forceCapture);
        }
        
        // Get the random number to select one of our random moves
        int random = Random.Range(0,legalMoveList.Count);

        // Return a (possibly null) random move.
        return legalMoveList[random];
    }
}
