using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/// <summary>
/// 保存するデータの構造
///
/// 作りたいデータに合わせてクラスを変更or作成してください。
/// 以下のように内部クラスを調整するとやりやすいと思います。
/// 配列を使わないなら内部クラスは一つだけでも大丈夫だと思います。
/// また、新しクラスを他のファイルに作っても大丈夫です。
///
/// 例
/// class GameData
/// {
///     [System.Serializable]
///     class 使うデータ
///     {
///         ...
///     }
///
///     [System.Serializable]
///     class 使うデータで扱う構造(構造体的なイメージ)
///     {
///         ...
///     }
///
///     public 使うデータ 外部から扱うデータ = new 使うデータ()
/// }
/// </summary>
[System.Serializable]
public class GameData
{
    /// <summary> jsonに保存されるデータの構造 </summary>
    [System.Serializable]
    public class Data
    {
        public Person[] Day1 = new Person[5];
        public Person[] Day2 = new Person[5];
        public Person[] Day3 = new Person[5];
        public Person[] Day4 = new Person[5];
        public Person[] Day5 = new Person[5];
        public Person[] Day6 = new Person[5];
        public Person[] Day7 = new Person[5];
    }

    /// <summary> データを構成する要素 </summary>
    [System.Serializable]
    public class Person
    {
        public string Name;
        public float Score;
        public Person()
        {
            this.Name = "記録なし";
            this.Score = -99;
        }
        public Person(string name,float score)
        {
            this.Name = name;
            this.Score = score;
        }
    }

    public static GameData Instance = new GameData();
    public Data CurrentGameData = new Data();

    public static void ResetData()
    {
        Instance = new GameData();
        Debug.Log("セーブデータがリセットされました.\nまだファイルには書き込んでいません.");
    }
}

/// <summary> 任意のゲームデータを扱うクラス </summary>
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance = new GameDataManager();
    private const string FILE_PATH = "/Datas/";
    private const string FILE_NAME = "Test.json";

    public static void SaveData<T>(T saveData,string fileName)
    {
        StreamWriter writer;
        string json = JsonUtility.ToJson(saveData);
        writer = new StreamWriter(Application.dataPath + FILE_PATH + fileName, false);
        writer.Write(json);
        writer.Flush();
        writer.Close();
        Debug.Log("セーブデータをファイルに書き込みました.");
    }

    public static void SaveData()
    {
        SaveData(GameData.Instance.CurrentGameData,FILE_NAME);
    }

    public static T LoadData<T>(string jsonFileName)
    {
        if(File.Exists(Application.dataPath + FILE_PATH + jsonFileName))
        {
            string json;
            StreamReader reader;
            reader = new StreamReader(Application.dataPath + FILE_PATH + jsonFileName, false);
            json = reader.ReadToEnd();
            reader.Close();

            return JsonUtility.FromJson<T>(json);
        }

        Debug.LogError(FILE_PATH + jsonFileName + " is not found!!\nパスか名前が違う可能性があります.");
        T data = default;
        return data;
    }

    public static void CheckDataPath()
    {
        Debug.Log("path:" + Application.dataPath);
    }

    public static void ResetData()
    {
        GameData.ResetData();
        SaveData();
    }
}