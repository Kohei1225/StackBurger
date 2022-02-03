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
            var name = string.Format("{0,-15}", current_ranking[i].Name);
            var score = string.Format("{0,-8}", current_ranking[i].Score);
            _ScoreTexts[i].text = name + ": $" + score;
        }
    }

    public void UpdateRanking(string name, int newScore,int date)
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
    }

    
    GameData.Person[] RankingOfEachDate(int date)
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
    
}
