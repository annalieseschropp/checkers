using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class PieceSetTests
{
    // 12 white pieces
    // 12 black pieces
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("GameBoard");
    }

    [UnityTest]
    public IEnumerator PieceSetExists()
    {
        var pieceSet = GameObject.Find("PieceSet");
        var exists = pieceSet != null;
        yield return null;
        Assert.AreEqual(true, exists);
    }

    [UnityTest]
    public IEnumerator PieceSetScriptExists()
    {
        var pieceSetComponent = GameObject.Find("PieceSet").GetComponent(typeof(PieceSet)) as PieceSet;
        var exists = pieceSetComponent != null;
        yield return null;
        Assert.AreEqual(true, exists);
    }

    [UnityTest]
    public IEnumerator PiecesExists()
    {
        var pieceSetComponent = GameObject.Find("PieceSet").GetComponent(typeof(PieceSet)) as PieceSet;
        var pieces = pieceSetComponent.GetPieces();
        var exists = pieces != null;
        yield return null;
        Assert.AreEqual(true, exists);
    }

    [UnityTest]
    public IEnumerator GetNumWhitePieces()
    {
        var pieceSetComponent = GameObject.Find("PieceSet").GetComponent(typeof(PieceSet)) as PieceSet;
        var pieces = pieceSetComponent.GetPieces();

        int whiteCount = 0;
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                if(pieces[x,y] != null && pieces[x,y].GetPieceType() == CheckersState.State.White)
                {
                    whiteCount++;
                }
            }
        }
        yield return null;
        Assert.AreEqual(12, whiteCount);
    }

    [UnityTest]
    public IEnumerator GetNumBlackPieces()
    {
        var pieceSetComponent = GameObject.Find("PieceSet").GetComponent(typeof(PieceSet)) as PieceSet;
        var pieces = pieceSetComponent.GetPieces();

        int blackCount = 0;
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                if(pieces[x,y] != null && pieces[x,y].GetPieceType() == CheckersState.State.Black)
                {
                    blackCount++;
                }
            }
        }
        yield return null;
        Assert.AreEqual(12, blackCount);
    }

    [UnityTest]
    public IEnumerator GetPiecesOnWhiteSquares()
    {
        var pieceSetComponent = GameObject.Find("PieceSet").GetComponent(typeof(PieceSet)) as PieceSet;
        var pieces = pieceSetComponent.GetPieces();

        int whiteSquareCount = 0;
        for(int x = 0; x < 8; x++)
        {
            for(int y = 0; y < 8; y++)
            {
                if((x+y)%2 == 1 && pieces[x,y] != null)
                {
                    whiteSquareCount++;
                }
            }
        }
        yield return null;
        Assert.AreEqual(0, whiteSquareCount);
    }

    [UnityTest]
    public IEnumerator MakeKings()
    {
        var pieceSetComponent = GameObject.Find("PieceSet").GetComponent(typeof(PieceSet)) as PieceSet;
        var pieces = pieceSetComponent.GetPieces();

        pieces[0,0].SetPieceType(CheckersState.State.WhiteKing);
        pieces[7,7].SetPieceType(CheckersState.State.BlackKing);

        yield return null;
        Assert.AreEqual(CheckersState.State.WhiteKing, pieces[0,0].GetPieceType());
        Assert.AreEqual(CheckersState.State.BlackKing, pieces[7,7].GetPieceType());
    }
}
