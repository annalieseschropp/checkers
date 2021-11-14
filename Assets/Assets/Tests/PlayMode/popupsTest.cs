using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class PopupsTest
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("GameBoard");
    }

    //Test the quit button
    [UnityTest]
    public IEnumerator quitClicked()
    {
        var control = GameObject.Find("Canvas/popupElement").GetComponent(typeof(Popup)) as Popup;;
        control.quitClicked();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "Menu");
    }

    [UnityTest]
    public IEnumerator openPopup()
    {
        var control = GameObject.Find("Canvas/popupElement").GetComponent(typeof(Popup)) as Popup;;
        control.openPopup();
        yield return new WaitForSeconds(1);
        bool popupState = control.getPopupState();
        yield return null;
        Assert.AreEqual(true, popupState);
    }

    [UnityTest]
    public IEnumerator closePopup()
    {
        var control = GameObject.Find("Canvas/popupElement").GetComponent(typeof(Popup)) as Popup;;
        control.closePopup();
        yield return new WaitForSeconds(1);
        bool popupState = control.getPopupState();
        yield return null;
        Assert.AreEqual(false, popupState);
    }


    //For the end game popups
    [UnityTest]
    public IEnumerator quitIsClicked()
    {
        var control = GameObject.Find("Canvas/endGameElement").GetComponent(typeof(EndGamePopup)) as EndGamePopup;
        control.quitIsClicked();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "Menu");
    }

    [UnityTest]
    public IEnumerator restartGameFunc()
    {
        var control = GameObject.Find("Canvas/endGameElement").GetComponent(typeof(EndGamePopup)) as EndGamePopup;
        control.restartGameFunc();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "GameBoard");
    }

}