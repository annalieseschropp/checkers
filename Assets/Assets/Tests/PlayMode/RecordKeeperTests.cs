using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class RecordKeeperTests
{
    // [SetUp]
    // public void SetUp()
    // {
    //     SceneManager.LoadScene("LeaderBoard");
    // }

    [UnityTest]
    public IEnumerator AddNewRecord()
    {
        RecordKeeper.ClearRecords();
        yield return null;
        Assert.AreEqual(true, RecordKeeper.AddRecord("NewRecord1"));
    }

    [UnityTest]
    public IEnumerator AddExistingRecord()
    {
        RecordKeeper.LoadData();
        RecordKeeper.AddRecord("NewRecord1");
        RecordKeeper.SaveData();
        yield return null;
        Assert.AreEqual(false, RecordKeeper.AddRecord("NewRecord1"));
    }

    [UnityTest]
    public IEnumerator UpdateExistingRecordWon()
    {
        RecordKeeper.ClearRecords();
        RecordKeeper.AddRecord("NewRecord1");
        RecordKeeper.UpdateRecordWon("NewRecord1");
        yield return null;
        Assert.AreEqual(1, RecordKeeper.GetRecordWon("NewRecord1"));
    }

    [UnityTest]
    public IEnumerator UpdateExistingRecordLost()
    {
        RecordKeeper.ClearRecords();
        RecordKeeper.AddRecord("NewRecord1");
        RecordKeeper.UpdateRecordLost("NewRecord1");
        yield return null;
        Assert.AreEqual(1, RecordKeeper.GetRecordLost("NewRecord1"));
    }

    [UnityTest]
    public IEnumerator ClearAllRecords()
    {
        RecordKeeper.AddRecord("NewRecord1");
        RecordKeeper.AddRecord("NewRecord2");
        RecordKeeper.ClearRecords();
        yield return null;
        Assert.AreEqual(0, RecordKeeper.listOfRecords.Count);
    }

    [UnityTest]
    public IEnumerator SaveDataFileCreation()
    {
        RecordKeeper.SaveData();
        yield return null;
        Assert.AreEqual(true, System.IO.File.Exists(RecordKeeper.filePath));
    }

    [UnityTest]
    public IEnumerator LoadDataFromSaveFile()
    {
        RecordKeeper.ClearRecords();
        RecordKeeper.SaveData();
        RecordKeeper.AddRecord("Record1");
        RecordKeeper.AddRecord("Record2");
        RecordKeeper.AddRecord("Record3");
        RecordKeeper.AddRecord("Record4");
        RecordKeeper.SaveData();
        RecordKeeper.listOfRecords.Clear();
        RecordKeeper.LoadData();
        yield return false;
        Assert.AreEqual(4, RecordKeeper.listOfRecords.Count);
    }
}