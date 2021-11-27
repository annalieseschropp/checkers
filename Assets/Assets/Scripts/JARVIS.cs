using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CheckersState;
using CheckersMove;
using System;
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
            
            if (moveController.GetCurrentTurn() == CheckersMove.Turn.White) //Gets the AIs colour, notice it's reversed, that's intentional
            {
                curColour = CurrentColour.Black;
            }
            else 
            {
                curColour = CurrentColour.White;
            }


            CheckersState.State[,] referenceBoard = (CheckersState.State[,]) (findOptimalMove((CheckersState.State[,])boardState.Clone(), 4, forceCapture, true, curColour).move).Clone(); 

            foreach (CheckersMove.Move move in legalMoveList)
            {
                CheckersState.State[,] boardCopy = (CheckersState.State[,])boardState.Clone();

                LegalMoveGenerator.makeTheoreticalMove(move, boardCopy, moveController.GetCurrentTurn());

                if (boardState == referenceBoard)
                {
                    return move;
                }
            }
        }

        //If it's a multicapture just make a move
        int random = UnityEngine.Random.Range(0,legalMoveList.Count);
        return legalMoveList[random];
    }

    /// <summary>
    /// Minimax Method
    /// This is the minimax algorithm which is made for simple turn based games like checkers, chess, go, and tic-tac-toe
    /// It builds a tree of all the possible moves made, then mactracks trying to maximize the ai's points and minimizing the human's lossed points until it finds a best case solution that satisfied both given the current gameboard layout.
    /// It's currently set to tree to a depth of 3 to minimize lagging the game too much. 
    /// </summary>
    private Tuple findOptimalMove(CheckersState.State[,] boardState, int depth, bool forceCapture, bool isMax, CurrentColour aiColour) //curColour is the AIs Colour
    {   

        Tuple returnTuple = new Tuple();        
        CheckersMove.GameStatus status = LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.White);
        
        //Check if we've reached the end of a branch or if the game has ended in some way
        if (depth == 0 || status != CheckersMove.GameStatus.InProgress)
        {
            //Get the score for this branch
            if (aiColour == CurrentColour.White) {
                returnTuple.numeric = moveController.countPiecesRemaining(boardState, CheckersMove.Turn.White) - moveController.countPiecesRemaining(boardState, CheckersMove.Turn.Black);
            }
            else 
            {
                returnTuple.numeric = moveController.countPiecesRemaining(boardState, CheckersMove.Turn.Black) - moveController.countPiecesRemaining(boardState, CheckersMove.Turn.White);
            }  

            returnTuple.move = (CheckersState.State[,]) boardState.Clone();
            return returnTuple;
        }
            

        if (isMax)  //Use AI colour
        {
            CheckersState.State[,] bestMove = (CheckersState.State[,]) boardState.Clone();
            int maxEval = int.MinValue;

            CheckersMove.Turn aiTurn;
            if (aiColour == CurrentColour.White)
            {
                aiTurn = CheckersMove.Turn.White;
            }
            else 
            {
                aiTurn = CheckersMove.Turn.Black;
            }

            foreach (CheckersMove.Move move in LegalMoveGenerator.GetLegalMoves(boardState, aiTurn, forceCapture))
            {
                boardState = (CheckersState.State[,])LegalMoveGenerator.makeTheoreticalMove(move, boardState, aiTurn).Clone();
                Tuple eval = findOptimalMove(boardState, depth-1, forceCapture, false, aiColour);
    
                maxEval = Math.Max(maxEval, eval.numeric);

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

            CheckersMove.Turn humanTurn;
            if (aiColour == CurrentColour.White)
            {
                humanTurn = CheckersMove.Turn.Black;
            }
            else 
            {
                humanTurn = CheckersMove.Turn.White;
            }

            foreach (CheckersMove.Move move in LegalMoveGenerator.GetLegalMoves(boardState, humanTurn, forceCapture))
            {
                boardState = (CheckersState.State[,])LegalMoveGenerator.makeTheoreticalMove(move, boardState, humanTurn).Clone();
                Tuple eval = findOptimalMove(boardState, depth-1, forceCapture, true, aiColour);
                minEval = Math.Min(minEval, eval.numeric);

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
