using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary> 現在のプレイヤー名 </summary>
    public string CurrentPlayerName { get; set; }
    /// <summary> 現在のスコア </summary>
    public int CurrentScore { get; set; } = 0;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ゲームデータをセーブする
    /// </summary>
    public static void SaveData()
    {
        SaveData data = SaveManager.GetData();
        var rm = FindObjectOfType<GameDataBase>();

        ISave saveIf = rm.GetComponent<ISave>();
        saveIf.Save(data.CurrentScoreDatas);
        Debug.Log(data.CurrentScoreDatas);
        SaveManager.Save();
    }

    /// <summary>
    /// ゲームデータをロードする
    /// </summary>
    public static void LoadData()
    {
        SaveManager.Load();
        SaveData data = SaveManager.GetData();

        var gameData = FindObjectOfType<GameDataBase>();

        ISave saveIf = gameData.GetComponent<ISave>();
        saveIf.Load(data.CurrentScoreDatas);
        Debug.Log(data);
        RankingManager.Instance.ScoreDatas = data.CurrentScoreDatas.ScoreDatas;
    }
}
