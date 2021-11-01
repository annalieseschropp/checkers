using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class LegalMoveGeneratorTests
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
    public IEnumerator LegalMovesCorrect()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();
        boardState[0,0] = CheckersState.State.White;
        boardState[2,0] = CheckersState.State.White;
        boardState[7,1] = CheckersState.State.WhiteKing;
        boardState[5,1] = CheckersState.State.WhiteKing;

        boardState[7,7] = CheckersState.State.Black;
        boardState[5,7] = CheckersState.State.Black;
        boardState[0,6] = CheckersState.State.BlackKing;
        boardState[2,6] = CheckersState.State.BlackKing;

        Assert.AreEqual(1, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(0, 0), boardState, CheckersMove.Turn.White).Count);
        Assert.AreEqual(2, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(2, 0), boardState, CheckersMove.Turn.White).Count);
        Assert.AreEqual(2, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(7, 1), boardState, CheckersMove.Turn.White).Count);
        Assert.AreEqual(4, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(5, 1), boardState, CheckersMove.Turn.White).Count);
        Assert.AreEqual(0, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(7, 7), boardState, CheckersMove.Turn.White).Count);

        Assert.AreEqual(1, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(7, 7), boardState, CheckersMove.Turn.Black).Count);
        Assert.AreEqual(2, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(5, 7), boardState, CheckersMove.Turn.Black).Count);
        Assert.AreEqual(2, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(0, 6), boardState, CheckersMove.Turn.Black).Count);
        Assert.AreEqual(4, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(2, 6), boardState, CheckersMove.Turn.Black).Count);
        Assert.AreEqual(0, LegalMoveGenerator.GetLegalMoves(new CheckersMove.Square(0, 0), boardState, CheckersMove.Turn.Black).Count);
        yield return null;
        // board with pieces on different squares, test
    }

    [UnityTest]
    public IEnumerator MakeMoveUpdatesBoard()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();
        boardState[5,5] = CheckersState.State.White;
        boardState[6,6] = CheckersState.State.Black;
        CheckersMove.Move testMove = new CheckersMove.Move(new CheckersMove.Square(5,5), new CheckersMove.Square(7,7));

        LegalMoveGenerator.MakeLegalMove(testMove, ref boardState, CheckersMove.Turn.White);

        Assert.AreEqual(boardState[5,5], CheckersState.State.Empty);
        Assert.AreEqual(boardState[6,6], CheckersState.State.Empty);
        Assert.AreEqual(boardState[7,7], CheckersState.State.WhiteKing);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GameStatusDraw()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();
        Assert.AreEqual(CheckersMove.GameStatus.Draw, LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.Black));
        Assert.AreEqual(CheckersMove.GameStatus.Draw, LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.White));
        yield return null;
    }

    [UnityTest]
    public IEnumerator GameStatusInProgress()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();

        boardState[0,0] = CheckersState.State.White;
        boardState[7,7] = CheckersState.State.Black;
        Assert.AreEqual(CheckersMove.GameStatus.InProgress, LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.White));
        Assert.AreEqual(CheckersMove.GameStatus.InProgress, LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.Black));
        yield return null;
    }

    [UnityTest]
    public IEnumerator GameStatusWhiteWin()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();
        boardState[6,6] = CheckersState.State.White;
        boardState[5,5] = CheckersState.State.White;
        Assert.AreEqual(CheckersMove.GameStatus.WhiteWin, LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.Black));

        boardState[7,7] = CheckersState.State.Black;
        Assert.AreEqual(CheckersMove.GameStatus.WhiteWin, LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.Black));
        yield return null;
    }

    [UnityTest]
    public IEnumerator GameStatusBlackWin()
    {
        CheckersState.State[,] boardState = GetEmptyBoardState();
        boardState[2,2] = CheckersState.State.Black;
        boardState[1,1] = CheckersState.State.Black;
        Assert.AreEqual(CheckersMove.GameStatus.BlackWin, LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.White));

        boardState[0,0] = CheckersState.State.White;
        Assert.AreEqual(CheckersMove.GameStatus.BlackWin, LegalMoveGenerator.GetGameStatus(boardState, CheckersMove.Turn.White));
        yield return null;
    }
}
