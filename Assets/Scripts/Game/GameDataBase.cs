using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataBase : MonoBehaviour, ISave
{
    [SerializeField]
    SaveData.GameData m_gameData = default;

    public static GameDataBase Instance { get; private set; }

    public SaveData.GameData GameData { get => m_gameData; set => m_gameData = value; }

    void Awake()
    {
        Instance = this;
        //EventManager.ListenEvents(Events.RankingUpdate, SetUp);
    }

    void Start()
    {
        SetUp();
    }

    /// <summary>
    /// セーブする
    /// </summary>
    /// <param name="data"> セーブ先のデータ </param>
    public void Save(SaveData.GameData data)
    {
        data.ScoreDatas = m_gameData.ScoreDatas;
    }

    /// <summary>
    /// ロードする
    /// </summary>
    /// <param name="data"> ロード先のデータ </param>
    public void Load(SaveData.GameData data)
    {
        m_gameData.ScoreDatas = data.ScoreDatas;
    }

    /// <summary>
    /// これまでに保存されていたデータがあればセットする
    /// </summary>
    public void SetUp()
    {
        //m_gameData.ScoreDatas = SaveManager.GetData().CurrentScoreDatas.ScoreDatas;
    }

    /// <summary>
    /// ランキングを更新する
    /// </summary>
    /// <param name="name"></param>
    /// <param name="newScore"></param>
    public void RankingUpdate(string name, int newScore)
    {
        string beforeName = default;
        string updateName = name;
        int beforeScore = 0;
        int updateScore = newScore;

        for (int i = 0; i < m_gameData.ScoreDatas.Length; i++)
        {
            if (m_gameData.ScoreDatas[i].Score < beforeScore)
            {
                string tempName = m_gameData.ScoreDatas[i].PlayerName;
                int tempScore = m_gameData.ScoreDatas[i].Score;

                updateName = beforeName;
                updateScore = beforeScore;

                m_gameData.ScoreDatas[i].PlayerName = updateName;
                m_gameData.ScoreDatas[i].Score = updateScore;

                beforeName = tempName;
                beforeScore = tempScore;
            }
            else if (m_gameData.ScoreDatas[i].Score < updateScore)
            {
                beforeName = m_gameData.ScoreDatas[i].PlayerName;
                beforeScore = m_gameData.ScoreDatas[i].Score;

                m_gameData.ScoreDatas[i].PlayerName = updateName;
                m_gameData.ScoreDatas[i].Score = updateScore;
            }
        }
        //RankingManager.Instance.ScoreDatas = m_gameData.ScoreDatas;
    }
}
