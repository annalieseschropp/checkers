using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

// Serializable structure for each record
[Serializable]
public class Record
{
    public string name;
    public int gamesWon;
    public int gamesLost;
}

public static class RecordKeeper
{
    // Static list of all records stored on the computer
    public static List<Record> listOfRecords = new List<Record>();
    
    // File path to file
    public static string filePath = Application.persistentDataPath + "\\checkersRecordFile.save";
    
    // Function to save the records list to the file.
    public static void SaveData()
    {
        FileStream fs = new FileStream(filePath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        foreach(Record record in listOfRecords)
        {
            binaryFormatter.Serialize(fs, record);
        }

        fs.Close();
    }

    // Function to loads the records data into the list.
    public static void LoadData()
    {
        if (!System.IO.File.Exists(filePath))
        {
            FileStream file = new FileStream(filePath, FileMode.Create);
            file.Close();
        }

        FileStream fs = new FileStream(filePath, FileMode.Open);
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        listOfRecords.Clear();
        while (fs.Position != fs.Length)
        {
            Record rec = (Record)binaryFormatter.Deserialize(fs);
            listOfRecords.Add(rec);
        }

        fs.Close();
    }

    // Function to add a new record to the list of records, if the record exists, then it will be updated (Saftey, prevents duplicate names)
    public static bool AddRecord(string name)
    {
        if (GetRecordIndex(name) == -1)
        {
            Record record = new Record();
            record.name = name;
            record.gamesWon = 0;
            record.gamesLost = 0;

            listOfRecords.Add(record);
            return true;
        }
        return false;
    }

    // Increments the number of games won by user by 1
    public static bool UpdateRecordWon(string name)
    {
        int index = GetRecordIndex(name);
        if (index != -1)
        {
            listOfRecords[index].gamesWon++;
            return true;
        }
        return false;
    }

    // Return the number of games won by a user
    public static int GetRecordWon(string name)
    {
        int index = GetRecordIndex(name);
        if (index != -1)
        {
            return listOfRecords[index].gamesWon;
        }
        return -1;
    }

    // Deletes the record of a specific name
    public static bool DeleteRecord(string name)
    {
        int index = GetRecordIndex(name);
        if (index != -1)
        {
            listOfRecords.RemoveAt(index);
            return true;
        }
        return false;
    }

    // Clear all the records in the game and save the empty
    public static bool ClearRecords()
    {
        listOfRecords.Clear();
        SaveData();
        return true;
    }

    // Increments the number of games lost by the user by 1
    public static bool UpdateRecordLost(string name)
    {
        int index = GetRecordIndex(name);
        if (index != -1)
        {
            listOfRecords[index].gamesLost++;
            return true;
        }
        return false;
    }

    // Return the number of games won by a user
    public static int GetRecordLost(string name)
    {
        int index = GetRecordIndex(name);
        if (index != -1)
        {
            return listOfRecords[index].gamesLost;
        }
        return -1;
    }

    // Private function to get the record index of a certain name
    private static int GetRecordIndex(string _name)
    {
        for (int i = 0; i < listOfRecords.Count; i++)
        {
            if (listOfRecords[i].name == _name)
            {
                return i;
            }
        }
        return -1;
    }

    // Function to update an entire record 
    private static bool UpdateRecord(string name, int gamesWon, int gamesLost)
    {
        int index = GetRecordIndex(name);
        if (index != -1)
        {
            listOfRecords[index].gamesWon = gamesWon;
            listOfRecords[index].gamesLost = gamesLost;
            return true;
        }
        return false;
    }
}