using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    [Tooltip("スコアデータ")]
    static SaveData.ScoreData[] m_scoreDatas = default;

    [Tooltip("ランキング画面")]
    [SerializeField]
    GameObject m_rankingPanel = default;

    [Tooltip("スコアを表示するテキスト")]
    [SerializeField]
    Text[] m_scoreTexts = default;

    [Header("デバッグ用")]
    [SerializeField]
    bool isDebug = default;

    static bool Init = false;

    public static RankingManager Instance { get; private set; }
    public SaveData.ScoreData[] ScoreDatas { get => m_scoreDatas; set => m_scoreDatas = value; }


    void Awake()
    {
        Instance = this;
        if (isDebug)
        {
            GameManager.SaveData();
        }
    }

    void Start()
    {
        EventManager.ListenEvents(Events.RankingView, RankingView);
        EventManager.ListenEvents(Events.RankingUpdate, RankingUpdate);

        if (!Init)
        {
            m_scoreDatas = GameDataBase.Instance.GameData.ScoreDatas;
            Init = true;
        }
    }

    void RankingView()
    {
        m_rankingPanel.SetActive(true);

        for (int i = 0; i < m_scoreTexts.Length; i++)
        {
            m_scoreTexts[i].text = $"{m_scoreDatas[i].PlayerName}：{m_scoreDatas[i].Score}点";
        }
    }

    void RankingUpdate()
    {
        //GameDataBase.Instance.GameData.ScoreDatas = m_scoreDatas;
        Debug.Log("ランキング更新");
    }

    public void RankingUpdate(string name, int newScore)
    {
        string beforeName = default;
        string updateName = name;
        int beforeScore = 0;
        int updateScore = newScore;

        for (int i = 0; i < m_scoreDatas.Length; i++)
        {
            if (m_scoreDatas[i].Score < beforeScore)
            {
                string tempName = m_scoreDatas[i].PlayerName;
                int tempScore = m_scoreDatas[i].Score;

                updateName = beforeName;
                updateScore = beforeScore;

                m_scoreDatas[i].PlayerName = updateName;
                m_scoreDatas[i].Score = updateScore;

                beforeName = tempName;
                beforeScore = tempScore;
            }
            else if (m_scoreDatas[i].Score < updateScore)
            {
                beforeName = m_scoreDatas[i].PlayerName;
                beforeScore = m_scoreDatas[i].Score;

                m_scoreDatas[i].PlayerName = updateName;
                m_scoreDatas[i].Score = updateScore;
            }
        }
    }
}
