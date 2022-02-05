using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : SingletonMonoBehaviour<RankingManager>
{
    [Tooltip("ランキング画面")]
    [SerializeField]
    GameObject m_rankingPanel = default;

    [Tooltip("名前を表示するテキスト")]
    [SerializeField]
    Text[] _NameTexts = default;

    [Tooltip("スコアを表示するテキスト")]
    [SerializeField]
    Text[] _ScoreTexts = default;
    private string _FileName = "Test.json";

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(this);
        }
        Debug.Log("RankingManager.Awake()");

        //最初にjsonファイルから読み込ませたデータを保持する必要があるのでどこかにアタッチした方がいい.
        GameData.Instance.CurrentGameData = GameDataManager.LoadData<GameData.Data>(_FileName);
    }

    void Start()
    {
        for(int i = 0; i < GameData.Instance.CurrentGameData.Day1.Length;i++)
        {
            //Debug.Log(_RankingOfEachDate[i][2].Name);
        }

    }

    public void UpdateRankingView(int date)
    {
        Debug.Log("UpdateRankingView()");
        var current_ranking = RankingOfEachDate(date);
        /*

        Debug.Log(current_ranking);
        */
        for(int i = 0; i < current_ranking.Length;i++)
        {
            //Debug.Log("[" + i + "]name:" + current_ranking[i].Name + " score:" + current_ranking[i].Score);
            var name = string.Format("{0,-15}", current_ranking[i].Name).PadLeft(5);
            var score = string.Format(": ${0,-8}", current_ranking[i].Score.ToString("N2"));
            if (current_ranking[i].Score < 0)
                score = string.Format(": {0,-8}", "--------");

            _NameTexts[i].text = name;
            _ScoreTexts[i].text = score ;
        }
    }

    public void UpdateRanking(string name, float newScore,int date)
    {
        string before_name = default;
        string update_name = name;
        float before_score = -100;
        float update_score = newScore;
        var current_ranking = RankingOfEachDate(date);

        for (int i = 0; i < current_ranking.Length; i++)
        {
            if (current_ranking[i].Score < before_score)
            {
                string tempName = current_ranking[i].Name;
                float tempScore = current_ranking[i].Score;

                update_name = before_name;
                update_score = before_score;

                current_ranking[i].Name = update_name;
                current_ranking[i].Score = update_score;

                before_name = tempName;
                before_score = tempScore;
            }
            else if (current_ranking[i].Score < update_score)
            {
                before_name = current_ranking[i].Name;
                before_score = current_ranking[i].Score;

                current_ranking[i].Name = update_name;
                current_ranking[i].Score = update_score;
            }
        }

        GameDataManager.SaveData();
    }

    /// <summary> 任意の日付のランキングを取得する </summary>
    /// <param name="date"> 取得したいランキングの日付 </param>
    /// <returns> 指定した日付のランキング </returns>
    public GameData.Person[] RankingOfEachDate(int date)
    {
        var current_ranking = GameData.Instance.CurrentGameData.Day1;
        switch (date)
        {
            case 2:
                current_ranking = GameData.Instance.CurrentGameData.Day2;
                break;
            case 3:
                current_ranking = GameData.Instance.CurrentGameData.Day3;
                break;
            case 4:
                current_ranking = GameData.Instance.CurrentGameData.Day4;
                break;
            case 5:
                current_ranking = GameData.Instance.CurrentGameData.Day5;
                break;
            case 6:
                current_ranking = GameData.Instance.CurrentGameData.Day6;
                break;
            case 7:
                current_ranking = GameData.Instance.CurrentGameData.Day7;
                break;
        }
        return current_ranking;
    }

    /// <summary> 指定した日付の特定の順位を取得 </summary>
    /// <param name="rank"> 順位 </param>
    /// <param name="date"> 日付 </param>
    /// <returns> 指定した順位データ </returns>
    public GameData.Person SerchAnyRank(int rank, int date)
    {
        
        var current_ranking = RankingOfEachDate(date);
        if(rank < 1 && current_ranking.Length < rank)
        {
            Debug.LogError("SerchAnyRankの引数rankが範囲外です!!");
            return current_ranking[0];
        }
        return current_ranking[rank - 1];
    }

    /// <summary> 新しいスコアがランクインするかを確認する </summary>
    /// <param name="newScore"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public bool IsRankInScore(int newScore, int date)
    {
        var current_ranking = RankingOfEachDate(date);

        return current_ranking[current_ranking.Length - 1].Score < newScore;
    }
}
