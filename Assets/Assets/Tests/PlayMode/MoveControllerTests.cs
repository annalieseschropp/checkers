using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class MoveControllerTests
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
    public IEnumerator TestNormalGame()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();
        boardState[1,1] = CheckersState.State.White;
        boardState[2,2] = CheckersState.State.Black;
        boardState[4,4] = CheckersState.State.Black;
        boardState[4,6] = CheckersState.State.Black;
        boardState[2,6] = CheckersState.State.Black;
        boardState[1,5] = CheckersState.State.Black;

        MoveController moveController = new MoveController(ref boardState);
        int illegalCount = 0;

        CheckersMove.Move[] moveList = new CheckersMove.Move[]{
            new CheckersMove.Move(1,5,0,4),
            new CheckersMove.Move(1,1,3,3),
            new CheckersMove.Move(3,3,5,5),
            new CheckersMove.Move(5,5,3,7),
            new CheckersMove.Move(3,7,1,5),
            new CheckersMove.Move(1,5,2,4), // Illegal
            new CheckersMove.Move(0,4,1,5), // Illegal
            new CheckersMove.Move(0,4,1,3), 
            new CheckersMove.Move(1,5,2,4),
            new CheckersMove.Move(1,3,2,2),
        };

        for(int i = 0; i < moveList.Length; i++)
        {
            moveController.SelectPiece(moveList[i].src);
            if (!moveController.MakeMove(moveList[i].dest))
            {
                illegalCount++;
            }
        }

        Assert.AreEqual(2, illegalCount);
        Assert.AreEqual(CheckersState.State.WhiteKing, boardState[2,4]);
        Assert.AreEqual(CheckersMove.GameStatus.InProgress, moveController.GetGameStatus());

        yield return null;
    }


    [UnityTest]
    public IEnumerator TestForceCaptureGame()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();
        boardState[1,1] = CheckersState.State.White;
        boardState[3,3] = CheckersState.State.Black;
        boardState[4,4] = CheckersState.State.Black;
        boardState[4,6] = CheckersState.State.Black;
        boardState[2,6] = CheckersState.State.Black;
        boardState[0,4] = CheckersState.State.Black;

        MoveController moveController = new MoveController(ref boardState, true);
        int illegalCount = 0;

        CheckersMove.Move[] moveList = new CheckersMove.Move[]{
            new CheckersMove.Move(3,3,2,2),
            new CheckersMove.Move(1,1,0,2), // Illegal
            new CheckersMove.Move(1,1,3,3),
            new CheckersMove.Move(3,3,5,5),
            new CheckersMove.Move(5,5,3,7),
            new CheckersMove.Move(3,7,1,5),
            new CheckersMove.Move(1,5,2,4), // Illegal
            new CheckersMove.Move(0,4,1,5), // Illegal
            new CheckersMove.Move(0,4,1,3), 
            new CheckersMove.Move(1,5,2,4),
            new CheckersMove.Move(1,3,2,2),
        };

        for(int i = 0; i < moveList.Length; i++)
        {
            moveController.SelectPiece(moveList[i].src);
            if (!moveController.MakeMove(moveList[i].dest))
            {
                illegalCount++;
            }
        }

        Assert.AreEqual(3, illegalCount);
        Assert.AreEqual(CheckersState.State.WhiteKing, boardState[2,4]);
        Assert.AreEqual(CheckersMove.GameStatus.InProgress, moveController.GetGameStatus());

        yield return null;
    }

    [UnityTest]
    public IEnumerator TestPieceCount()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();
        boardState[1,7] = CheckersState.State.BlackKing;
        boardState[7,7] = CheckersState.State.Black;
        boardState[6,6] = CheckersState.State.White;
        boardState[4,4] = CheckersState.State.WhiteKing;

        MoveController moveController = new MoveController(ref boardState, false);

        Assert.AreEqual(2, moveController.CountWhitePiecesRemaining());
        Assert.AreEqual(2, moveController.CountBlackPiecesRemaining());

        moveController.SelectPiece(new CheckersMove.Square(7,7));
        moveController.MakeMove(new CheckersMove.Square(5,5));
        moveController.DeclineMulticapture();

        Assert.AreEqual(1, moveController.CountWhitePiecesRemaining());
        Assert.AreEqual(2, moveController.CountBlackPiecesRemaining());

        moveController.SelectPiece(new CheckersMove.Square(4,4));
        moveController.MakeMove(new CheckersMove.Square(6,6));

        Assert.AreEqual(1, moveController.CountWhitePiecesRemaining());
        Assert.AreEqual(1, moveController.CountBlackPiecesRemaining());

        yield return null;
    }
}
