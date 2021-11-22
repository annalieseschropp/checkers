using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class SoundBankTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Menu");
    }

    [UnityTest]
    public IEnumerator SoundBankExists()
    {
        yield return null;
        Assert.AreEqual(true, !(SoundBank.GetInstance() == null));
    }

    [UnityTest]
    public IEnumerator SoundBankSoundsExist()
    {
        yield return null;
        Assert.AreEqual(true, !(SoundBank.GetInstance().buttonClickSound == null));
        Assert.AreEqual(true, !(SoundBank.GetInstance().menuMusicSound == null));
        Assert.AreEqual(true, !(SoundBank.GetInstance().checkerSound1 == null));
        Assert.AreEqual(true, !(SoundBank.GetInstance().checkerSound2 == null));
        Assert.AreEqual(true, !(SoundBank.GetInstance().checkerSound3 == null));
        Assert.AreEqual(true, !(SoundBank.GetInstance().gameOverSound == null));
    }
}
