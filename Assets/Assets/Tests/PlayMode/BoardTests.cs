using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class BoardTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("GameBoard");
    }

    [UnityTest]
    public IEnumerator GetNumBoardTiles()
    {
        var board = GameObject.Find("Board");
        yield return null;
        Assert.AreEqual(64, board.transform.childCount);
    }

    [UnityTest]
    public IEnumerator GetOriginTileCoords()
    {
        var tile = GameObject.Find("Board/( 0, 0 )");
        yield return null;
        Assert.AreEqual(new Vector3(0,0,-1), tile.transform.position);
    }

    [UnityTest]
    public IEnumerator GetMidTileCoords()
    {
        var tile = GameObject.Find("Board/( 3, 3 )");
        yield return null;
        Assert.AreEqual(new Vector3(3, 3, -1), tile.transform.position);
    }

    [UnityTest]
    public IEnumerator GetLastTileCoords()
    {
        var tile = GameObject.Find("Board/( 7, 7 )");
        yield return null;
        Assert.AreEqual(new Vector3(7, 7, -1), tile.transform.position);
    }
}
