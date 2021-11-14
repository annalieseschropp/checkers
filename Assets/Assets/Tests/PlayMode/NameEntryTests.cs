using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class NameEntryTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("NameEntry");
    }

    [UnityTest]
    public IEnumerator PlayerOneEntryError()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(NameEntry)) as NameEntry;
        control.dropdownPlayerOne.value = 0;
        control.playerOneName.text = "";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("Player One Must Select or Enter a Name", control.topText.text);
    }

    [UnityTest]
    public IEnumerator PlayerTwoEntryError()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(NameEntry)) as NameEntry;
        control.playerOneName.text = "TestOne";
        control.dropdownPlayerTwo.value = 0;
        control.playerTwoName.text = "";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("Player Two Must Select or Enter a Name", control.topText.text);
    }

    [UnityTest]
    public IEnumerator CorrectFirstNameEntry()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(NameEntry)) as NameEntry;
        control.playerOneName.text = "TestOne";
        control.playerTwoName.text = "TestTwo";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("TestOne", NameStaticClass.playerOneName);
    }

    [UnityTest]
    public IEnumerator CorrectSecondNameEntry()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(NameEntry)) as NameEntry;
        control.playerOneName.text = "TestOne";
        control.playerTwoName.text = "TestTwo";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual("TestTwo", NameStaticClass.playerTwoName);
    }

    [UnityTest]
    public IEnumerator ForcedCaptureToggleCheckFalse()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(NameEntry)) as NameEntry;
        control.playerOneName.text = "TestOne";
        control.playerTwoName.text = "TestTwo";
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual(false, NameStaticClass.forcedMove);
    }

    [UnityTest]
    public IEnumerator ForcedCaptureToggleCheckTrue()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(NameEntry)) as NameEntry;
        control.playerOneName.text = "TestOne";
        control.playerTwoName.text = "TestTwo";
        control.forcedCapture.isOn = true;
        control.PlayGameButtonOnClick();
        yield return null;
        Assert.AreEqual(true, NameStaticClass.forcedMove);
    }

    [UnityTest]
    public IEnumerator CancelButtonCheck()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(NameEntry)) as NameEntry;
        control.CancelGameButtonOnClick();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "Menu");
    }

    [UnityTest]
    public IEnumerator SwapNamesTest()
    {
        var control = GameObject.Find("/Canvas/nameEntry").GetComponent(typeof(NameEntry)) as NameEntry;
        control.playerOneName.text = "TestOne";
        control.playerTwoName.text = "TestTwo";
        control.PlayGameButtonOnClick();
        NameStaticClass.SwapPlayerNames();
        yield return null;
        Assert.AreEqual("TestTwo", NameStaticClass.playerOneName);
        Assert.AreEqual("TestOne", NameStaticClass.playerTwoName);
    }
}