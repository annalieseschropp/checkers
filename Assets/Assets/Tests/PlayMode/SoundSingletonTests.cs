using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class SoundSingletonTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Menu");
    }

    [UnityTest]
    public IEnumerator SoundSingletonExists()
    {
        yield return null;
        Assert.AreEqual(true, !(SoundSingleton.GetInstance() == null));
    }
}
