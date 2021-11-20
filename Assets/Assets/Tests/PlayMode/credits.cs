using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class credits : MonoBehaviour
{

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Credits");
    }

    [UnityTest]
    public IEnumerator BackButton()
    {
        var button = GameObject.Find("Canvas").GetComponent(typeof(Credits)) as Credits;
        button.GoBack();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name;
        yield return null;
        Assert.AreEqual(true, scene == "Menu");
    }
}
