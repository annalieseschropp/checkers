using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using CheckersState;

public class GameOptionsTests
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    [UnityTest]
    public IEnumerator MoveSpeedSliderTest()
    {
        Slider control = GameObject.Find("/Canvas/OptionsMenu/MoveSpeedSlider").GetComponent<Slider>() as Slider;
        control.value = 0.2f;
        yield return null;
        Assert.AreEqual(0.2f, GameOptionsStaticClass.moveSpeed);
        control.value = 0.5f;
        Assert.AreEqual(0.5f, GameOptionsStaticClass.moveSpeed);
    }
}
