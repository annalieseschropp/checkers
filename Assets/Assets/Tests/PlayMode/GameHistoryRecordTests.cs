using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class GameHistoryRecordKeeperTests
{
    [UnityTest]
    public IEnumerator AddNewRecord()
    {
        GameHistoryRecordKeeper.DestroyAllData();
        GameHistoryRecordKeeper.AddRecord("Player1", "Player2", "Player2", 4, 4);
        yield return null;
        Assert.AreEqual(1, GameHistoryRecordKeeper.listOfGameHistory.Count);
    }

    [UnityTest]
    public IEnumerator AddMultipleRecords()
    {
        GameHistoryRecordKeeper.DestroyAllData();
        GameHistoryRecordKeeper.AddRecord("Player1", "Player2", "Player1", 1, 12);
        GameHistoryRecordKeeper.AddRecord("Player1", "Player2", "Player1", 1, 12);
        GameHistoryRecordKeeper.AddRecord("Player1", "Player2", "Player1", 1, 12);
        yield return null;
        Assert.AreEqual(3, GameHistoryRecordKeeper.listOfGameHistory.Count);
    }

    [UnityTest]
    public IEnumerator SaveData()
    {
        GameHistoryRecordKeeper.DestroyAllData();
        GameHistoryRecordKeeper.SaveData();
        yield return null;
        Assert.AreEqual(true, File.Exists(GameHistoryRecordKeeper.filePath));
    }

    [UnityTest]
    public IEnumerator LoadData()
    {
        GameHistoryRecordKeeper.DestroyAllData();
        GameHistoryRecordKeeper.AddRecord("Player1", "Player2", "Player1", 1, 12);
        GameHistoryRecordKeeper.AddRecord("Player2", "Player1", "Player1", 1, 12);
        GameHistoryRecordKeeper.AddRecord("Player1", "Player2", "Player2", 1, 12);
        GameHistoryRecordKeeper.AddRecord("Player2", "Player1", "Player1", 1, 12);
        GameHistoryRecordKeeper.SaveData();
        GameHistoryRecordKeeper.listOfGameHistory.Clear();
        GameHistoryRecordKeeper.LoadData();
        yield return null;
        Assert.AreEqual(4, GameHistoryRecordKeeper.listOfGameHistory.Count);
    }
}