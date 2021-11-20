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
    private Button button;

    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Credits");
    }

    [UnityTest]
    public IEnumerator BackButton()
    {
        button = GameObject.Find("BackButton").GetComponent<Button>();
        button.onClick.Invoke();
        yield return new WaitForSeconds(1);
        string scene = SceneManager.GetActiveScene().name; 
        yield return null;
        Assert.AreEqual(true, scene == "Menu");
    }
}
