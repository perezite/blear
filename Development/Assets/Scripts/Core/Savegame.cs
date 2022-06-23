using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Represents the savegame
public class Savegame
{
    // the savegames file path
    private string filePath;

    // constructor
    public Savegame()
    {
        filePath = Application.persistentDataPath + "/savegame.dat";
    }

    // load savegame from file
    public SavegameData Load()
    {
        var savegameData = new SavegameData();

        // load savegame from existing file or create an empty savegame by resetting
        if (File.Exists(filePath))
        {
            try
            {
                using (FileStream file = File.Open(filePath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    savegameData = (SavegameData)bf.Deserialize(file);
                }
            }
            catch (Exception)
            {
                // the savegame file is either corrupted or something went seriously wrong on the system
                // in either case, the only chance to proceed is trying to reset the savegame 
                savegameData = Reset();
            } 
        }
        else
        { 
            // create an empty savegame by resetting
            savegameData = Reset();
        }

        return savegameData;
    }

    // reset savegame
    public SavegameData Reset()
    {
        var savegameData = new SavegameData();
        savegameData.LastVisitedGameLevel = Levels.FirstGameLevel;
        Save(savegameData);
        return savegameData;
    }

    // save savegame to file
    public void Save(SavegameData newSavegameData)
    {
        using (FileStream file = File.Create(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, newSavegameData);
        }
    }

    // Entity for savegame data
    [Serializable]
    public class SavegameData
    {
        public int LastVisitedGameLevel;
    }
}