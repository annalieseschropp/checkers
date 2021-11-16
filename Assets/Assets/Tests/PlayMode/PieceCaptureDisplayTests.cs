using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PieceCaptureDisplayTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("GameBoard");
    }

    [UnityTest]
    public IEnumerator GetStartingScore()
    {
        var whiteCount = GameObject.Find("Canvas/WhiteScore").GetComponent<Text>();
        var blackCount = GameObject.Find("Canvas/BlackScore").GetComponent<Text>();
        yield return null;
        Assert.AreEqual("0", whiteCount.text.ToString());
        Assert.AreEqual("0", blackCount.text.ToString());
    }
}