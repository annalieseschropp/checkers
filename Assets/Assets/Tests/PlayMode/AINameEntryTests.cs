using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class AINameEntryTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("AINameEntry");
    }

    [UnityTest]
    public IEnumerator PlayerEntryError()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(AINameEntry)) as AINameEntry;
        control.dropdownPlayerOne.value = 0;
        control.playerOneName.text = "";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("Player Must Select or Enter a Name", control.topText.text);
    }

    [UnityTest]
    public IEnumerator PlayerNameEntryError()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(AINameEntry)) as AINameEntry;
        control.playerOneName.text = "Computer";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("Player Cannot Use this Reserved Name", control.topText.text);
    }

    [UnityTest]
    public IEnumerator CorrectNameEntry()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(AINameEntry)) as AINameEntry;
        control.playerOneName.text = "TestOne";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("TestOne", NameStaticClass.playerOneName);
    }

    [UnityTest]
    public IEnumerator CorrectBlackColourSelection()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(AINameEntry)) as AINameEntry;
        control.playerOneName.text = "TestOne";
        control.dropdownPlayerColour.value = 0;
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("Computer", NameStaticClass.playerTwoName);
    }

    [UnityTest]
    public IEnumerator CorrectWhiteColourSelection()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(AINameEntry)) as AINameEntry;
        control.playerOneName.text = "TestOne";
        control.dropdownPlayerColour.value = 1;
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("Computer", NameStaticClass.playerOneName);
    }

    [UnityTest]
    public IEnumerator ForcedCaptureToggleCheckFalse()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(AINameEntry)) as AINameEntry;
        control.playerOneName.text = "TestOne";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual(false, NameStaticClass.forcedMove);
    }

    [UnityTest]
    public IEnumerator ForcedCaptureToggleCheckTrue()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(AINameEntry)) as AINameEntry;
        control.playerOneName.text = "TestOne";
        control.forcedCapture.isOn = true;
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual(true, NameStaticClass.forcedMove);
    }

    [UnityTest]
    public IEnumerator CancelButtonCheck()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(AINameEntry)) as AINameEntry;
        control.CancelGameButtonOnClick();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "Menu");
    }

}
