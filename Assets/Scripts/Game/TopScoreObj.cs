using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopScoreObj : SingletonMonoBehaviour<TopScoreObj>
{
    [Tooltip("最高得点")]
    [SerializeField,Header("最高得点表示用テキスト")] private Text _CurrentTopRankScoreText = null;
    [Tooltip("１位のプレイヤー名")]
    [SerializeField,Header("１位のプレイヤー名表示用テキスト")] private Text _CurrentTopRankNameText = null;

    void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!_CurrentTopRankScoreText) Debug.LogError(gameObject.name + " has no score Text!!!!");
        if(!_CurrentTopRankNameText) Debug.LogError(gameObject.name + " has no name Text!!!!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTopRankerInfo(int date)
    {
        Debug.Log("Enter UpdateTopRankerInfo()");
        var rank_m = FindObjectOfType<RankingManager>()?.GetComponent<RankingManager>();
        var top_rank = rank_m.SerchAnyRank(1, date);
        UpdateTopScores(top_rank.Name, top_rank.Score);
    }

    private void UpdateTopScores(string playerName,float score)
    {
        Debug.Log("Enter UpdateTopScores()");
        _CurrentTopRankNameText.text = playerName;
        if (score < 0) _CurrentTopRankScoreText.text = "-------";
        else _CurrentTopRankScoreText.text = "$" + score.ToString("N2");
        
    }
}
