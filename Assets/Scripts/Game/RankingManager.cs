using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Tooltip("日付を戻すボタン")]
    [SerializeField]
    Button _PreviousButton = default;

    [Tooltip("日付を進めるボタン")]
    [SerializeField]
    Button _NextButton = default;

    [Tooltip("ランキングのタイトルを表示するテキスト")]
    [SerializeField]
    Text _RankingTitleText = default;

    private string _FileName = "Test.json";
    private int _CurrentDate = 1;

    public int CurrentDate
    {
        set
        {
            if (value < 1) _CurrentDate = 1;
            else if (value > 7) _CurrentDate = 7;
            else _CurrentDate = value;
        }
        get { return _CurrentDate; }
    }

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
        if(SceneManager.GetActiveScene().name == "Title")
        {
            UpdateRankingView();
            _PreviousButton.interactable = false;
        }
    }

    /// <summary> 指定したテキストにランキングを表示 </summary>
    /// <param name="date"></param>
    public void UpdateRankingView(int date)
    {
        Debug.Log("UpdateRankingView()");
        _RankingTitleText.text = date + "日目のランキング";
        var current_ranking = RankingOfEachDate(date);
        if(current_ranking == null)
        {
            for(int i = 0;i < 5;i++)
            {
                _NameTexts[i].text = "そもそも";
                _ScoreTexts[i].text = "取得できてない";
            }
            return;
        }
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

    public void UpdateRankingView()
    {
        UpdateRankingView(CurrentDate);
    }

    public void UpdateRanking(string name, float newScore,int date)
    {
        string before_name = default;
        float before_score = -100;

        var current_ranking = RankingOfEachDate(date);

        for (int i = 0; i < current_ranking.Length; i++)
        {
            if (current_ranking[i].Score <= before_score)
            {
                string temp_name = current_ranking[i].Name;
                float temp_score = current_ranking[i].Score;

                current_ranking[i].Name = before_name;
                current_ranking[i].Score = before_score;

                before_name = temp_name;
                before_score = temp_score;
            }
            else if (current_ranking[i].Score < newScore)
            {
                before_name = current_ranking[i].Name;
                before_score = current_ranking[i].Score;

                current_ranking[i].Name = name;
                current_ranking[i].Score = newScore;
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
    public bool IsRankInScore(float newScore, int date)
    {
        var current_ranking = RankingOfEachDate(date);
        var index = current_ranking.Length - 1;
        var rank = current_ranking[index];
        Debug.Log("name:" + rank.Name + " score:" + rank.Score + "  newScore:" + newScore);
        return current_ranking[current_ranking.Length - 1].Score < newScore;
    }

    public void ChangeNextDate()
    {
        CurrentDate++;
        _NextButton.interactable = CurrentDate != 7;
        _PreviousButton.interactable = true;
        UpdateRankingView();
    }

    public void ChangePreviousDate()
    {
        CurrentDate--;
        _PreviousButton.interactable = CurrentDate != 1;
        _NextButton.interactable = true;
        UpdateRankingView();
    }
}
