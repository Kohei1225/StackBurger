using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary> 保存するデータの構造 </summary>
[System.Serializable]
public class Data
{
    public float _Score = 10;
    public string _Name = "player";
}

/// <summary> 任意のゲームデータを扱うクラス </summary>
public class GameDataManager : MonoBehaviour
{
    public static GameDataBase Instance = new GameDataBase();
    private static string _FilePath = "Datas/";

    public static void SaveData(Data saveData,string fileName)
    {
        StreamWriter writer;

        string json = JsonUtility.ToJson(saveData);

        writer = new StreamWriter(Application.dataPath + _FilePath + fileName, false);
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    public static T LoadData<T>(string fileName)
    {
        if(File.Exists(Application.dataPath + _FilePath + fileName))
        {
            string json = "";

            StreamReader reader;
            reader = new StreamReader(Application.dataPath + _FilePath, false);
            json = reader.ReadToEnd();
            reader.Close();

            return JsonUtility.FromJson<T>(json);
        }

        Debug.LogError(_FilePath + fileName + " is not Found!!");
        T data = default(T);
        return data;
    }
}

