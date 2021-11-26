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
            CurrentColour curColour = new CurrentColour();
            
            if (moveController.GetCurrentTurn() == CheckersMove.Turn.White) //Gets the AIs colour
            {
                curColour = CurrentColour.White;
            }
            else 
            {
                curColour = CurrentColour.Black;
            }


            //Runs the minimax algo
            CheckersState.State[,] referenceBoard = findOptimalMove(boardState, 3, legalMoveList, true, curColour).move; 
            
            /*foreach (CheckersMove.Move move in legalMoveList)
            {
                boardState = LegalMoveGenerator.makeTheoreticalMove(move, boardState, moveController.GetCurrentTurn());

                if (boardState == referenceBoard)
                {
                    Debug.Log("We here bitch");
                    Debug.Log("the move = " + move.ToString());
                    return move;
                }
            }*/
        }

        //If something crazy happens it just makes a random move
        int random = UnityEngine.Random.Range(0,legalMoveList.Count);
        return legalMoveList[random];
    }

    private Tuple findOptimalMove(CheckersState.State[,] boardState, int depth, List<CheckersMove.Move> legalMoveList, bool isMax, CurrentColour curColour) //curColour is the AIs Colour
    {   
        Debug.Log("Depth = " + depth.ToString());
        Debug.Log("isMax = " + isMax.ToString());
        Tuple returnTuple = new Tuple();
        
        //Get the ai colour
        bool isBlack;
        CheckersMove.Turn turn = moveController.GetCurrentTurn();

        if (turn == CheckersMove.Turn.White)
        {
            isBlack = false;
        }
        else
        {
            isBlack = true;
        }

        CheckersMove.GameStatus status = LegalMoveGenerator.GetGameStatus(boardState, turn);
        
        //Check if we've reached the end of a branch or if the game has ended in some way
        if (depth == 0 || status != CheckersMove.GameStatus.InProgress)
        {
            Debug.Log("WWWWWWWWHHHHHHHAAAAAAATTTTTTTTTTTTTTT");

            //Get the score for this branch
            if (isBlack) 
            {
                returnTuple.numeric = moveController.countPiecesRemaining(boardState, true) - moveController.countPiecesRemaining(boardState, false);
            }
            else 
            {
               returnTuple.numeric = moveController.countPiecesRemaining(boardState, false) - moveController.countPiecesRemaining(boardState, true);
            }

            //return the move being made
            returnTuple.move = boardState;

            return returnTuple;
        }
            

        if (isMax)  //Use AI colour
        {
            Debug.Log("AI Checking for move");
            CheckersState.State[,] bestMove = null;
            int maxEval = 0;

            foreach (CheckersMove.Move move in legalMoveList)
            {
                CheckersMove.Turn tempTurnVar;

                if (curColour == CurrentColour.White)   //If the AI is white this game
                {   
                    tempTurnVar = CheckersMove.Turn.White;
                    boardState = LegalMoveGenerator.makeTheoreticalMove(move, boardState, CheckersMove.Turn.White);
                }
                else //If the AI is black this 
                {
                    tempTurnVar = CheckersMove.Turn.Black;
                    boardState = LegalMoveGenerator.makeTheoreticalMove(move, boardState, CheckersMove.Turn.Black);
                }

                List<CheckersMove.Move> moves = LegalMoveGenerator.GetLegalMoves(boardState, tempTurnVar);


                Tuple eval = findOptimalMove(boardState, depth-1, moves, false, curColour);
                maxEval = Math.Max(int.MaxValue, eval.numeric);

                if (eval.numeric == maxEval)
                {
                    bestMove = eval.move;
                }
            }

            returnTuple.numeric = maxEval;
            returnTuple.move = bestMove;
        } 
        else    //Use Human colour
        {
            Debug.Log("Human checking for move");
            CheckersState.State[,] bestMove = null;
            int minEval = 0;

            foreach (CheckersMove.Move move in legalMoveList)
            {
                CheckersMove.Turn tempTurnVar;
                
                if (curColour == CurrentColour.White)
                {
                    tempTurnVar = CheckersMove.Turn.Black;
                    boardState = LegalMoveGenerator.makeTheoreticalMove(move, boardState, CheckersMove.Turn.Black);
                }
                else 
                {
                    tempTurnVar = CheckersMove.Turn.White;
                    boardState = LegalMoveGenerator.makeTheoreticalMove(move, boardState, CheckersMove.Turn.White);
                }

                List<CheckersMove.Move> moves = LegalMoveGenerator.GetLegalMoves(boardState, moveController.GetCurrentTurn());

                Tuple eval = findOptimalMove(boardState, depth-1, moves, true, curColour);
                minEval = Math.Max(int.MaxValue, eval.numeric);

                if (eval.numeric == minEval)
                {
                    bestMove = eval.move;
                }
            }

            Debug.Log("b2");
            returnTuple.numeric = minEval;
            returnTuple.move = bestMove;
        }

        return returnTuple;        
    }
}
