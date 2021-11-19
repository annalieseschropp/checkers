using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[Serializable]
public class GameHistoryRecord
{
    public string blackPlayerName;
    public string whitePlayerName;
    public string gameWinner;
    public int blackFinalScore;
    public int whiteFinalScore;
}

public static class GameHistoryRecordKeeper
{
    public static List<GameHistoryRecord> listOfGameHistory = new List<GameHistoryRecord>();

    public static string filePath = Application.persistentDataPath + "\\gameHistoryRecords.save";
    
    public static void saveData()
    {
        FileStream fs = new FileStream(filePath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        foreach (GameHistoryRecord record in listOfGameHistory)
        {
            binaryFormatter.Serialize(fs, record);
        }

        fs.Close();
    }

    public static void loadData()
    {
        if (!System.IO.File.Exists(filePath))
        {
            FileStream file = new FileStream(filePath, FileMode.Create);
            file.Close();
        }

        FileStream fs = new FileStream(filePath, FileMode.Open);
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        listOfGameHistory.Clear();
        while (fs.Position != fs.Length)
        {
            GameHistoryRecord rec = (GameHistoryRecord)binaryFormatter.Deserialize(fs);
            listOfGameHistory.Add(rec);
        }

        fs.Close();
    }

    public static void AddRecord(string blackPlayer, string whitePlayer, string gameWinner, int blackPlayerPoints, int whitePlayerPoints)
    {
        GameHistoryRecord rec = new GameHistoryRecord();
        rec.blackPlayerName = blackPlayer;
        rec.whitePlayerName = whitePlayer;
        rec.gameWinner = gameWinner;
        rec.blackFinalScore = blackPlayerPoints;
        rec.whiteFinalScore = whitePlayerPoints;

        listOfGameHistory.Add(rec);
    }
}
