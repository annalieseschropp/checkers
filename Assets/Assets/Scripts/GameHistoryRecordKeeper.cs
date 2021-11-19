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
    public int whileFinalScore;
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
    }
}
