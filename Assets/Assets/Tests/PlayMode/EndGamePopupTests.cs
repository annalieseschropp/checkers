using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class EndGamePopupTest
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("GameBoard");
    }

    //Test the quit button
    [UnityTest]
    public IEnumerator QuitClickTest()
    {
        var control = GameObject.Find("Canvas/endGameElement").GetComponent(typeof(EndGamePopup)) as EndGamePopup;
        control.quitIsClicked();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "Menu");
    }

    //Test the play again button
    [UnityTest]
    public IEnumerator PlayAgainClickTest()
    {
        var control = GameObject.Find("Canvas/endGameElement").GetComponent(typeof(EndGamePopup)) as EndGamePopup;
        NameStaticClass.playerOneName = "Alice";
        NameStaticClass.playerTwoName = "Bob";
        control.restartGameFunc();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "GameBoard");
        // Swap names
        Assert.AreEqual("Alice", NameStaticClass.playerTwoName);
        Assert.AreEqual("Bob", NameStaticClass.playerOneName);
    }
}
