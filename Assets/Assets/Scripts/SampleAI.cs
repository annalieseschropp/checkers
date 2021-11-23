using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CheckersState;
using CheckersMove;

public class SampleAI : _AIPlayer
{
    override public CheckersMove.Move? GetAIMove(CheckersState.State[,] boardState, CheckersMove.Turn currentTurn, bool forceCapture, CheckersMove.Square? multicaptureSquare = null)
    {
        List<CheckersMove.Move> legalMoveList;
        if (multicaptureSquare is CheckersMove.Square multiSquare)
        {
            legalMoveList = LegalMoveGenerator.GetLegalMoves(multiSquare, boardState, currentTurn, forceCapture);
            if(Random.Range(-1,legalMoveList.Count) == -1) return null;
        }
        else
        {
            legalMoveList = LegalMoveGenerator.GetLegalMoves(boardState, currentTurn, forceCapture);
        }
        
        int random = Random.Range(0,legalMoveList.Count);

        return legalMoveList[random];
    }
}