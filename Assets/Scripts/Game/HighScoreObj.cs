using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreObj : MonoBehaviour
{
    [Tooltip("最高得点")]
    [SerializeField,Header("最高得点表示用テキスト")] private Text _CurrentTopRankScore = null;
    [Tooltip("１位のプレイヤー名")]
    [SerializeField,Header("１位のプレイヤー名表示用テキスト")] private Text _CurrentTopRankName = null;

    // Start is called before the first frame update
    void Start()
    {
        if(!_CurrentTopRankScore) Debug.LogError(gameObject.name + " has no score Text!!!!");
        if(!_CurrentTopRankName) Debug.LogError(gameObject.name + " has no name Text!!!!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHighScores(string playerName,float score)
    {
        _CurrentTopRankName.text = playerName;
        _CurrentTopRankScore.text = score.ToString("N2");
    }
}
