using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class EndTurnTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("GameBoard");
    }

    private CheckersState.State[,] GetEmptyBoardState()
    {
        CheckersState.State[,] curState = new CheckersState.State[8,8]; 
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                curState[x, y] = CheckersState.State.Empty;
            }
        }
        return curState;
    }

    [UnityTest]
    public IEnumerator MultiCaptureCancellation()
    {
       CheckersState.State[,] boardState = GetEmptyBoardState();
        boardState[7,5] = CheckersState.State.Black;
        boardState[6,4] = CheckersState.State.White;
        boardState[4,2] = CheckersState.State.White;

        MoveController moveController = new MoveController(ref boardState);

        CheckersMove.Move[] moveList = new CheckersMove.Move[]{
            new CheckersMove.Move(7,5,5,3) // Legal
        };

        for (int i = 0; i < moveList.Length; i++)
        {
            moveController.SelectPiece(moveList[i].src);
            moveController.MakeMove(moveList[i].dest);
        }

        Assert.AreEqual(true, moveController.IsMulticaptureInProgress());
        moveController.DeclineMulticapture();
        yield return null;
        Assert.AreEqual(false, moveController.IsMulticaptureInProgress());
    }
}