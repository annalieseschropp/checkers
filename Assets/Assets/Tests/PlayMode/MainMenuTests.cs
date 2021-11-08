using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class MainMenuTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Menu");
    }

    [UnityTest]
    public IEnumerator SwitchToNameEntryState()
    {
        var button = GameObject.Find("/Canvas/mainMenu").GetComponent(typeof(mainMenu)) as mainMenu;
        button.PlayGame();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "NameEntry");
    }

    [UnityTest]
    public IEnumerator SwitchToOptions()
    {
        var button = GameObject.Find("/Canvas/mainMenu").GetComponent(typeof(mainMenu)) as mainMenu;
        button.LoadOptions();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "OptionsMenu");
    }

    [UnityTest]
    public IEnumerator SwitchBackFromOptions()
    {
        var button = GameObject.Find("/Canvas/mainMenu").GetComponent(typeof(mainMenu)) as mainMenu;
        button.LoadOptions();
        yield return new WaitForSeconds(1);
        var button2 = GameObject.Find("/Canvas/OptionsMenu").GetComponent(typeof(OptionsMenu)) as OptionsMenu;
        button2.GoBack();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Debug.Log(scene);
        Assert.AreEqual(true, scene == "Menu");
    }
}
