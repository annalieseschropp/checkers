using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CheckersState;
using CheckersMove;
using System;

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
public class JARVIS : AIPlayer
{
    private MoveController moveController;


    //Used for checking scores and what the move is
    private struct Tuple {
        public int numeric;
        public CheckersState.State[,] move;
    };

    private enum CurrentColour {
        White = 0,
        Black = ~White, //Black is NOT White
    };

    /// <summary>
    /// Method
    /// This is a sample AI move algorithm that just picks a random move.
    /// </summary>
    override public CheckersMove.Move? GetAIMove(CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool forceCapture, CheckersMove.Square? multicaptureSquare = null)
    {
        List<CheckersMove.Move> legalMoveList;
        moveController = new MoveController(ref boardState, forceCapture);

        /*
            If there is a multicapture in progress (signalled by <multicaptureSquare> not being null)
            then we choose a random move with that piece.
        */
        if (multicaptureSquare is CheckersMove.Square multiSquare)
        {
            // Get the legal moves for this piece. This will account for forced capture automatically.
            legalMoveList = LegalMoveGenerator.GetLegalMoves(multiSquare, boardState, currentTurn, forceCapture);

            Debug.Log("Legal move list = " + (legalMoveList[0]).ToString());
        }
        /*
            Else choose any random move.
        */
        else
        {
            // Get the legal moves for all pieces. This will account for forced capture automatically.
            legalMoveList = LegalMoveGenerator.GetLegalMoves(boardState, currentTurn, forceCapture);

            //Get the current colour of the ai's turn and set it to the enum
            /*CurrentColour curColour = new CurrentColour();
            
            if (moveController.GetCurrentTurn() == CheckersMove.Turn.White) //Gets the AIs colour, notice it's reversed, that's intentional
            {
                curColour = CurrentColour.Black;
            }
            else 
            {
                curColour = CurrentColour.White;
            }*/


            CheckersState.State[,] referenceBoard = (CheckersState.State[,]) (findOptimalMove((CheckersState.State[,])boardState.Clone(), 4, forceCapture, true).move).Clone(); 

            foreach (CheckersMove.Move move in legalMoveList)
            {
                CheckersState.State[,] boardCopy = (CheckersState.State[,])boardState.Clone();

                LegalMoveGenerator.makeTheoreticalMove(move, boardCopy, moveController.GetCurrentTurn());

                if (boardState == referenceBoard)
                {
                    Debug.Log("We here bitch");
                    return move;
                }
            }
        }

        //If something crazy happens it just makes a random move
        int random = UnityEngine.Random.Range(0,legalMoveList.Count);
        return legalMoveList[random];
    }

    //Assuming white is always AI for NOW-------------------------------------
    private Tuple findOptimalMove(CheckersState.State[,] boardState, int depth, bool forceCapture, bool isMax) //curColour is the AIs Colour
    {   

        //Debug.Log("Depth = " + depth.ToString());
        //Debug.Log("isMax = " + isMax.ToString());
        Tuple returnTuple = new Tuple();
        //returnTuple.move = boardState;
        //return returnTuple;
        
        CheckersMove.GameStatus status = LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.White);
        
        //Check if we've reached the end of a branch or if the game has ended in some way
        if (depth == 0 || status != CheckersMove.GameStatus.InProgress)
        {
            //Get the score for this branch
            returnTuple.numeric = moveController.countPiecesRemaining(boardState, CheckersMove.Turn.White) - moveController.countPiecesRemaining(boardState, CheckersMove.Turn.Black);
            Debug.Log("whites = " + moveController.countPiecesRemaining(boardState, CheckersMove.Turn.White).ToString());
            Debug.Log("blacks = " +  moveController.countPiecesRemaining(boardState, CheckersMove.Turn.Black).ToString());
            Debug.Log("Score = " + returnTuple.numeric.ToString());
    
            //return the move being made
            returnTuple.move = (CheckersState.State[,]) boardState.Clone();

            return returnTuple;
        }
            

        if (isMax)  //Use AI colour
        {
            CheckersState.State[,] bestMove = (CheckersState.State[,]) boardState.Clone();
            int maxEval = int.MinValue;

            foreach (CheckersMove.Move move in LegalMoveGenerator.GetLegalMoves(boardState, CheckersMove.Turn.White, forceCapture))
            {
                boardState = (CheckersState.State[,])LegalMoveGenerator.makeTheoreticalMove(move, boardState, CheckersMove.Turn.White).Clone();
                Tuple eval = findOptimalMove(boardState, depth-1, forceCapture, false);
                
                Debug.Log("A");
                Debug.Log("maxEval b4 = " + maxEval.ToString());
                Debug.Log("eval.numeric b4 = " + eval.numeric.ToString());
                maxEval = Math.Max(maxEval, eval.numeric);
                Debug.Log("maxEval After = " + maxEval.ToString());

                if (eval.numeric == maxEval)
                {

                    bestMove = (CheckersState.State[,]) (eval.move).Clone();
                }
            }

            returnTuple.numeric = maxEval;
            returnTuple.move = bestMove;
        } 
        else    //Use Human colour
        {
            CheckersState.State[,] bestMove = (CheckersState.State[,]) boardState.Clone();;
            int minEval = int.MaxValue;

            foreach (CheckersMove.Move move in LegalMoveGenerator.GetLegalMoves(boardState, CheckersMove.Turn.Black, forceCapture))
            {
                boardState = (CheckersState.State[,])LegalMoveGenerator.makeTheoreticalMove(move, boardState, CheckersMove.Turn.White).Clone();
                Tuple eval = findOptimalMove(boardState, depth-1, forceCapture, true);
                minEval = Math.Min(minEval, eval.numeric);


                Debug.Log("B");
                Debug.Log("minEval b4 = " + minEval.ToString());
                Debug.Log("eval.numeric b4 = " + eval.numeric.ToString());
                minEval = Math.Min(minEval, eval.numeric);
                Debug.Log("minEval After = " + minEval.ToString());

                if (eval.numeric == minEval)
                {
                    bestMove = (CheckersState.State[,])eval.move.Clone();
                }
            }
            returnTuple.numeric = minEval;
            returnTuple.move = bestMove;
        }

        return returnTuple;        
    }
}
