using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> メニュー画面のUI等を管理するクラス </summary>
public class CalenderManager : MonoBehaviour
{
    /// <summary> 動的な値 </summary>
    private int _CurrentDay = 1;
    /// <summary> 今できる一番高い難易度の上限 </summary>
    private int _MaxDay;//
    /// <summary> ステージタイトル </summary>
    private string [] _Explains = {
        "本日\nオープン！！",
        "新聞に\n載りました！",
        "新バーガー\n追加！！",
        "サンドイッチ\nはじめました",
        "マフィンも\nはじめました",
        "巨大バーガー\n登場！！",
        "おまかせも\n対応します！"
    };
    /// <summary> 表示する曜日 </summary>
    private string [] _DayOfWeekNames = {
        "Mon","Tue","Wed","Thu","Fri","Sat","Sun"
    };

    /// <summary> 日付けのテキスト </summary>
    Text _DayText;//
    /// <summary> メニュー看板のテキスト </summary>
    Text _BoardText;
    Text _MenuDay;
    Text _DayOfWeekText;
    MenuManager _MenuManager;//
    GameObject _TomorrowButton;
    GameObject _YesterdayButon;

    AudioSource _AudioSource;
    public AudioClip _ChangeDaySound;

    #region property
    /// <summary> 選択してる日付 </summary>
    public int CurrentDay
    {
        set
        {   if(0 < value && value <= MaxDay)
            {
                _CurrentDay = value;
            }
        }
        get { return _CurrentDay; }
    }

    /// <summary> 今できる日付の上限 </summary>
    public int MaxDay
    {
        set
        {
            if(0 < value && value < 8)
            {
                _MaxDay = value;
            }
        }
        get { return _MaxDay; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(day);
        _TomorrowButton = GameObject.Find("Tomorrow");
        _YesterdayButon = GameObject.Find("Yesterday");
        //データがなければ作る
        if(!PlayerPrefs.HasKey("day"))PlayerPrefs.SetInt("day",0);
        //進み具合によって最大値を決める
        _MaxDay = PlayerPrefs.GetInt("day") + 1;
        //Debug.Log(PlayerPrefs.GetInt("day"));

        //6日目が終わってれば7日目にする
        if(_MaxDay > 6)_MaxDay = 7;
        _DayText = GameObject.Find("Day").GetComponent<Text>();
        _BoardText = GameObject.Find("MenuText").GetComponent<Text>();
        _MenuDay = GameObject.Find("MenuDay").GetComponent<Text>();
        _DayOfWeekText = GameObject.Find("DayOfWeek").GetComponent<Text>();
        _MenuManager = GameObject.Find("Managers").GetComponent<MenuManager>();
        //Debug.Log(day);
        _AudioSource = GetComponent<AudioSource>();
        _AudioSource.volume = PlayerPrefs.GetFloat("SEvol") * PlayerPrefs.GetInt("SEex");

        var top_rank_m = FindObjectOfType<TopScoreObj>().GetComponent<TopScoreObj>();
        top_rank_m.UpdateTopRankerInfo(CurrentDay);
    }

    // Update is called once per frame
    void Update()
    {
        //日付によるボタンの表示の判定
        //一番日付が遅い時
        if (CurrentDay == MaxDay)
        {
            _TomorrowButton.SetActive(false);
        }
        else
        {
            _TomorrowButton.SetActive(true);
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                TurnNextDay();
            }
        }
        //前に日付がなかったら前の日付に戻るボタンを非表示
        if (CurrentDay == 1)
        {
            _YesterdayButon.SetActive(false);
        }
        //前の日付があったら前の日付に戻るボタンを表示
        else
        {
            _YesterdayButon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                TurnPreviousDay();
            }
        }

        //エンターキーかスペースキーが押されたら選択してるステージを始める
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            _MenuManager.HasStartGame = true;
        }
        
        
    }

    /// <summary> 次の日へ </summary>
    public void TurnNextDay()
    {
        PlayTurnDaySound();
        CurrentDay++;
        ChangeDay();
    }

    /// <summary> 前の日へ </summary>
    public void TurnPreviousDay()
    {
        PlayTurnDaySound();
        CurrentDay--;
        ChangeDay();
    }

    /// <summary> 日付を変更する </summary>
    public void ChangeDay()
    {

        _MenuManager.CurrentDay = this.CurrentDay;

        //テキストの更新
        _DayText.text = CurrentDay.ToString();
        ChangeTextColor();

        _BoardText.text = _Explains[CurrentDay - 1];
        _MenuDay.text = "6/" + CurrentDay.ToString();
        _DayOfWeekText.text = _DayOfWeekNames[CurrentDay - 1];

        var top_rank_m = FindObjectOfType<TopScoreObj>().GetComponent<TopScoreObj>();
        top_rank_m.UpdateTopRankerInfo(CurrentDay);
    }


    /// <summary> 日付を変える音を再生 </summary>
    void PlayTurnDaySound()
    {
        if(_ChangeDaySound == null)
        {
            return;
        }
        _AudioSource.Stop();
        _AudioSource.PlayOneShot(_ChangeDaySound);
    }

    void ChangeTextColor()
    {
        if (_CurrentDay == 7)
        {
            _DayText.color = new Color(1, 0, 0);
            _DayOfWeekText.color = new Color(1, 0, 0);
        }
        else
        {
            _DayText.color = new Color(0, 0, 0);
            _DayOfWeekText.color = new Color(0, 0, 0);
        }
        return;
    }
}
