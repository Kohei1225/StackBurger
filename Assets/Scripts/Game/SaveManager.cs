using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    static SaveManager Instance = new SaveManager();

    const string FILEPATH = "Datas/scoredata.json";

    SaveData Data = default;

    public static void Load()
    {
        Instance.Data = LocalData.Load<SaveData>(FILEPATH);

        if (Instance.Data == null)
        {
            Instance.Data = new SaveData();
        }
    }

    public static SaveData GetData()
    {
        if (Instance.Data == null)
        {
            Load();
        }
        return Instance.Data;
    }

    public static void Save()
    {
        LocalData.Save<SaveData>(FILEPATH, Instance.Data);
    }
}
