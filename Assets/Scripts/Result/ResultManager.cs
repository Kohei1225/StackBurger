using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary> 結果表示画面の処理をするクラス </summary>
public class ResultManager : SingletonMonoBehaviour<ResultManager>
{
    /// <summary> 受け渡した回数 </summary>
    private int _GoodSum = 10;
    /// <summary> 怒らせた回数 </summary>
    private int _AngrySum = 2;
    /// <summary> ミスした回数 </summary>
    private int _MissSum = 4;
    /// <summary> チップの合計 </summary>
    private float _ChipSum = 100.245f;
    /// <summary> 何日目か </summary>
    private int _CurrentDay;
    /// <summary> 完璧に渡した回数 </summary>
    private int _HappySum;
    /// <summary>  </summary>
    private float _MoveValue = 0;
    /// <summary> 最終的なスコア </summary>
    private float _FinalScore;
    /// <summary> 各スコアを表示する順番 </summary>
    private int _Junban = 0;
    /// <summary>  </summary>
    private int _FrameCounter = 0;
    /// <summary> ノルマスコア </summary>
    private int _ClearPoint = 190;

    public GameObject[] _TextObject;
    Text[] _ResultTexts = new Text[5];
    string[] _ResultTextTail = { "回", "回", "回", "$", "" };
    float[] _Score = new float[5];
    bool _HasStart = false;
    GameObject _NextDayButton;
    GameObject _EndingButton;
    GameObject _EndText;

    AudioSource _AudioSource;
    public AudioClip _Drum;
    public AudioClip _Cymbal;

    [SerializeField] Button[] _Buttons = null;

    #region property
    /// <summary> 何日目か </summary>
    public int CurrentDay
    {
        set
        {
            if(0 < value && value < 8)
            {
                _CurrentDay = value;
                return;
            }
            else
            {
                _CurrentDay = 1;
            }
        }
        get { return _CurrentDay; }
    }

    /// <summary>  </summary>
    public int GoodSum
    {
        set { _GoodSum = value; }
        get { return _GoodSum; }
    }

    /// <summary>  </summary>
    public int AngrySum
    {
        set { _AngrySum = value; }
        get { return _AngrySum; }
    }

    /// <summary>  </summary>
    public int MissSum
    {
        set { _MissSum = value; }
        get { return _MissSum; }
    }

    /// <summary>  </summary>
    public float ChipSum
    {
        set { _ChipSum = value; }
        get { return _ChipSum; }
    }

    /// <summary>  </summary>
    public int HappySum
    {
        set { _GoodSum = value; }
        get { return _GoodSum; }
    }

    public float FinalScore
    {
        set
        {
            _FinalScore = value;
            if (value < 0) _FinalScore = 0;
        }
        get { return _FinalScore; }
    }
    #endregion


    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_CurrentDay > 6)PlayerPrefs.SetInt("Complete",1);
        _EndText = GameObject.Find("End?");
        _EndText.SetActive(false);
        _AudioSource = GetComponent<AudioSource>();
        GameObject.Find("Day").GetComponent<Text>().text = _CurrentDay + "日目";
        _NextDayButton = GameObject.Find("NextDayButton");
        _NextDayButton.SetActive(false);
        _EndingButton = GameObject.Find("EndingButton");
        _EndingButton.SetActive(false);
        //GameObject.Find("RetryButton").GetComponent<PlayButton>().day = this._CurrentDay;

        //あらかじめスコアを計算しておく
        for(int i = 0; i < 5; i++)_ResultTexts[i] = _TextObject[i].GetComponent<Text>();
        FinalScore = (_GoodSum * 0.02f * _ChipSum) - (_AngrySum * 10) - (_MissSum * 2.5f) + _ChipSum + 0*_HappySum;
        _Score[0] = _GoodSum; _Score[1] = _AngrySum; 
        _Score[2] = _MissSum; _Score[3] = _ChipSum; 
        _Score[4] = _FinalScore;

        Debug.Log("chipSum:" + _ChipSum);
        if(PlayerPrefs.HasKey("day"))Debug.Log((PlayerPrefs.GetInt("day") + 1) + "日目");//日付表示

        //もし今やってるステージが最新ステージだったら
        if(_CurrentDay == PlayerPrefs.GetInt("day") + 1)
        {
            //もし合格点を出したら次の日を更新する
            if(_FinalScore > _ClearPoint)PlayerPrefs.SetInt("day",_CurrentDay);
        }

        _AudioSource.volume = PlayerPrefs.GetFloat("SEvol") * PlayerPrefs.GetInt("SEex");
        PlayerPrefs.Save();

        var top_rank_m = FindObjectOfType<TopScoreObj>().GetComponent<TopScoreObj>();
        top_rank_m.UpdateTopRankerInfo(CurrentDay);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_HasStart)_FrameCounter++;
        if(_FrameCounter > 50)
        {
            if(!_HasStart)_AudioSource.PlayOneShot(_Drum);
            _HasStart = true;
            
        }

        //印がついたら始める
        if(_HasStart && _Junban < 5)
        {
            //Debug.Log("入ってる");

            //_ResultTexts[_Junban].text = _MoveValue.ToString() + _ResultTextTail[_Junban];
            //Debug.Log(_MoveValue);

            

            if (UpdateCountValue(_Junban))
            {
                //値をリセット
                _MoveValue = 0;
                //次のテキストに移動
                _Junban++;

                //Debug.Log("等しい");

                //最後の要素だったら
                if(_Junban == 5)
                {
                    _AudioSource.Stop();
                    _AudioSource.PlayOneShot(_Cymbal);
                }
            }

            if (_Junban == 5)
            {
                if (_FinalScore >= _ClearPoint)
                {
                    if (_CurrentDay < 7)
                    {
                        _NextDayButton.SetActive(true);
                    }
                    else if (PlayerPrefs.GetInt("Easy") == 0)
                    {
                        _EndingButton.SetActive(true);
                    }
                    else
                    {
                        _EndText.SetActive(true);
                    }
                }
                else
                {
                    _EndText.SetActive(true);
                    _EndText.GetComponent<Text>().text = _ClearPoint + "が最低ラインだ！！";
                    if(RankingManager.Instance.IsRankInScore(FinalScore,CurrentDay))
                    {
                        FindObjectOfType<RankInManager>().PlayRankInAnim();
                    }
                }
                return;
            }
            //それ以外
            //else
            //{
            //    if(_Score[_Junban] >= 0)
            //    {
            //        //ある程度近づくまでは飛ばす
            //        if(_MoveValue < _Score[_Junban] - 150)_MoveValue += 100;
            //        else if(_MoveValue < _Score[_Junban] - 30)_MoveValue += 10;
            //        //ある程度値が近づいたら１づつプラスしていく
            //        else _MoveValue ++;       

            //        Debug.Log("カウント中...");
            //    }
            //    else
            //    {
            //        //ある程度近づくまでは飛ばす
            //        if(_MoveValue > _Score[_Junban] + 1100)_MoveValue -= 1000;
            //        else if(_MoveValue > _Score[_Junban] + 150)_MoveValue -= 100;
            //        else if(_MoveValue > _Score[_Junban] + 30)_MoveValue -= 10;
            //        //ある程度値が近づいたら１づつプラスしていく
            //        else _MoveValue --;
            //        Debug.Log("カウント中...");
            //    }
            //}
        }
    }

    /// <summary> ゲームをリトライする </summary>
    public void PlayGame()
    {
        SceneManager.sceneLoaded += GiveDay;
        SceneManager.LoadScene("Game");
    }

    public void NextStage()
    {
        _CurrentDay++;
        PlayGame();
    }

    bool UpdateCountValue(int order)
    {
        //
        if(_MoveValue == 0)
        {
            Debug.Log("[ " + order + " ](" + _Score[order] + ")");
        }
        Debug.Log(_MoveValue);
        var diff = _Score[order] - _MoveValue;
        //Debug.Log("diff:" + diff);
        var dir = 1;

        if(_Score[order] < 0)dir = -1;

        if (diff > 200) _MoveValue += dir*10;
        else if (diff > 1) _MoveValue += dir;
        else _MoveValue = _Score[order];

        if (order >= 3)
        {
            _ResultTexts[order].text = _MoveValue.ToString("N2") + _ResultTextTail[order];
            if(order == 3)
            {
                _ResultTexts[order].text = _ResultTextTail[order] + _MoveValue.ToString("N2");
            }

        }
        else
        {
            _ResultTexts[order].text = _MoveValue + _ResultTextTail[order];
        }
        return _MoveValue == _Score[order];
    }

    //次のシーンのクラスに値を与える(https://note.com/suzukijohnp/n/n050aa20a12f1)
    void GiveDay(Scene next, LoadSceneMode mode)
    {
        //シーン切り替え後のスクリプトを取得
        var GameManager = GameObject.Find("Managers").GetComponent<GameSystem>();

        //値を渡す
        GameManager.CurrentDay = _CurrentDay;
        //イベントから削除
        SceneManager.sceneLoaded -= GiveDay;
    }
}
