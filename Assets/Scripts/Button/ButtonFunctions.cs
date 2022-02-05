using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> ボタンの機能をまとめたクラス </summary>
public class ButtonFunctions : SingletonMonoBehaviour<ButtonFunctions>
{
    #region serialize field
    /// <summary> クリック音 </summary>
    [SerializeField]private AudioClip _ClickSound = null;
    /// <summary> カーソル音 </summary>
    [SerializeField]private AudioClip _CusrorSound = null;
    /// <summary> ゲームの管理クラス </summary>
    [SerializeField] private GameSystem _GameManager = null;
    /// <summary> メニューのUIを管理するクラス </summary>
    [SerializeField] private CalenderManager _CalenderManager = null;
    /// <summary> メニューを管理するクラス </summary>
    [SerializeField] private MenuManager _MenuManager = null;
    /// <summary> 置き手紙の管理クラス </summary>
    [SerializeField]private MessageScript _MessageManager = null;
    /// <summary> 結果画面の管理クラス </summary>
    [SerializeField] private ResultManager _ResultManager = null;
    /// <summary> タイトル画面の管理クラス </summary>
    [SerializeField] private TitleManager _TitleManager = null;
    /// <summary> ランキングの管理クラス </summary>
    [SerializeField] private RankingManager _RankingManager = null;
    #endregion

    #region field
    /// <summary> クリックしたか </summary>
    private bool _HasClicked = false;
    /// <summary> カーソルが合ったか </summary>
    private bool _HasEnterCursor = false;
    /// <summary> 音に関するコンポーネント </summary>
    private AudioSource _AudioSource = null;
    /// <summary> プレイする日付 </summary>
    private int _CurrentDay = 1;
    /// <summary> 次に移動するシーンの名前 </summary>
    private string _NextSceneName;

    /// <summary> ボタンの音量 </summary>
    private float _ButtonVol = 0f;
    /// <summary> ページを捲る音量 </summary>
    private float _SEVol = 0f;
    #endregion

    #region property
    /// <summary> 日にち </summary>
    public  int CurrentDay
    {
        set
        {
            if(0 < value && value < 8)
            {
                _CurrentDay = value;
            }
        }
        get { return _CurrentDay; }
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("day", 3);
        _AudioSource = GetComponent<AudioSource>();

        //音量の設定
        _ButtonVol = PlayerPrefs.GetFloat("Buttonvol") * PlayerPrefs.GetInt("Buttonex");
        _SEVol = PlayerPrefs.GetFloat("SEvol") * PlayerPrefs.GetInt("SEex");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region public function
    /// <summary> タイトル画面へ </summary>
    public void GoToTitle()
    {
        if(_HasClicked)
        {
            return;
        }
        _HasClicked = true;
        PlayClickSound();
        //タイトル画面に移動
        SceneManager.LoadScene("Title");
    }

    /// <summary> メニュー画面へ </summary>
    public void GoToMenu()
    {
        if (_HasClicked)
        {
            return;
        }
        _HasClicked = true;
        PlayClickSound();
        _NextSceneName = "Menu";

        if(_GameManager == null && _ResultManager == null && _TitleManager == null)
        {
            Debug.Log("今いるシーンを管理するクラスがButtonManagerにアタッチされてない.");
            return;
        }

        //今いる日付けの状態でメニューに遷移する
        if(_GameManager)
        {
            this.CurrentDay = _GameManager.CurrentDay;
        }
        else if(_ResultManager)
        {
            this.CurrentDay = _ResultManager.CurrentDay;
        }

        //遷移先のシーンに値を渡す
        SceneManager.sceneLoaded += GiveDay;
        SceneManager.LoadScene(_NextSceneName);
    }

    /// <summary> ゲーム画面へ </summary>
    public void PlayGame()
    {
        if (_HasClicked)
        {
            return;
        }
        _HasClicked = true;
        PlayClickSound();

        _MenuManager.HasStartGame = true;

    }

    /// <summary> エンディング画面へ </summary>
    public void GoToEnding()
    {
        if (_HasClicked)
        {
            return;
        }
        _HasClicked = true;
        PlayClickSound();
        SceneManager.LoadScene("Ending");
    }

    /// <summary> ゲームを終了 </summary>
    public void QuitGame()
    {
        if (_HasClicked)
        {
            return;
        }
        _HasClicked = true;

        //ゲーム終了
        UnityEngine.Application.Quit();
    }

    /// <summary> 次のページをめくる </summary>
    public void TurnNextPage()
    {
        if(_MessageManager == null)
        {
            return;
        }

        _MessageManager.TurnNextPage();
    }

    /// <summary> 前のページをめくる </summary>
    public void TurnPreviousPage()
    {
        if (_MessageManager == null)
        {
            return;
        }

        _MessageManager.TurnPreviousPage();
    }

    /// <summary> 次の日に進む </summary>
    public void TurnNextDay()
    {
        if(_CalenderManager == null)
        {
            Debug.Log("Calenderがアタッチされてない.");
            return;
        }

        _CalenderManager.TurnNextDay();
    }

    /// <summary> 前日に戻る </summary>
    public void TurnPreviousDay()
    {
        if(_CalenderManager == null)
        {
            Debug.Log("Calenderがアタッチされてない.");
            return;
        }

        _CalenderManager.TurnPreviousDay();
    }

    /// <summary> もう一度同じ日にちでプレイする </summary>
    public void RetryGame()
    {
        if (_HasClicked)
        {
            return;
        }
        PlayClickSound();
        if(_ResultManager == null)
        {
            Debug.Log("ResultManagwerがアタッチされてません.");
            return;
        }
        _HasClicked = true;

        _ResultManager.PlayGame();
    }

    /// <summary> 次の日をプレイする </summary>
    public void PlayNextDay()
    {
        if(_HasClicked)
        {
            return;
        }
        _HasClicked = true;

        PlayClickSound();
        if(_ResultManager == null)
        {
            Debug.Log("ResultManagerがButtonManagerにアタッチされてない");
            return;
        }

        _ResultManager.NextStage();
    }

    /// <summary> カーソルが合ったら音を流す </summary>
    public void PlayCursorOnEnterSound()
    {
        if (_AudioSource == null || _HasClicked)
        {
            return;
        }

        //一旦止める
        _AudioSource.Stop();
        //音量調節
        _AudioSource.volume = _ButtonVol;
        //再生
        _AudioSource.PlayOneShot(_CusrorSound);
    }

    /// <summary> オプション画面の表示 </summary>
    public void DisplayOption()
    {
        if(_TitleManager == null)
        {
            Debug.Log("TitleManagerがアタッチされてない.");
            return;
        }

        PlayClickSound();
        _TitleManager._State = TitleManager.TitleState.Option;
    }

    /// <summary> 遊び方画面の表示 </summary>
    public void DisplayExplain()
    {
        if (_TitleManager == null)
        {
            Debug.Log("TitleManagerがアタッチされてない.");
            return;
        }

        PlayClickSound();
        _TitleManager._State = TitleManager.TitleState.Explain;
    }

    /// <summary> ランキング画面の表示 </summary>
    public void DisplayRanking()
    {
        if (_TitleManager == null)
        {
            Debug.Log("TitleManagerがアタッチされてない.");
            return;
        }

        PlayClickSound();
        _TitleManager._State = TitleManager.TitleState.Ranking;
    }

    /// <summary> ランキング画面の表示 </summary>
    public void DisplayWarning()
    {
        if (_TitleManager == null)
        {
            Debug.Log("TitleManagerがアタッチされてない.");
            return;
        }

        PlayClickSound();
        _TitleManager._State = TitleManager.TitleState.Warning;
        _TitleManager.SetButonsInteractable(false);
    }

    /// <summary> ランキング画面を閉じる </summary>
    public void CloseRankingCanvas()
    {
        if (_TitleManager == null)
        {
            Debug.Log("TitleManagerがアタッチされてない.");
            return;
        }

        PlayClickSound();
        _TitleManager._State = TitleManager.TitleState.Option;
    }

    /// <summary> 警告画面を閉じる </summary>
    public void CloseWarningCanvas()
    {
        if (_TitleManager == null)
        {
            Debug.Log("TitleManagerがアタッチされてない.");
            return;
        }

        PlayClickSound();
        _TitleManager._State = TitleManager.TitleState.Option;
        _TitleManager.SetButonsInteractable(true);
    }

    /// <summary> タイトル画面に戻る </summary>
    public void CloseCanvas()
    {
        if (_TitleManager == null)
        {
            Debug.Log("TitleManagerがアタッチされてない.");
            return;
        }

        PlayClickSound();
        _TitleManager._State = TitleManager.TitleState.Title;
    }

    /// <summary> ランキングの日付を進める </summary>
    public void ChangeNextRankingDate()
    {
        if (_RankingManager == null)
        {
            Debug.LogError("RankingManagerがアタッチされてない.");
            return;
        }
        _RankingManager.ChangeNextDate();
        PlayClickSound();
    }

    /// <summary> ランキングの日付を戻す </summary>
    public void ChangePreviousRankingDate()
    {
        if (_RankingManager == null)
        {
            Debug.LogError("RankingManagerがアタッチされてない.");
            return;
        }
        _RankingManager.ChangePreviousDate();
        PlayClickSound();
    }


    /// <summary> クリックされたら音だけ流す </summary>
    public void PlayClickSound()
    {
        if (_ClickSound == null || _AudioSource == null)
        {
            return;
        }

        //一旦止める
        _AudioSource.Stop();
        //音量調節
        _AudioSource.volume = _ButtonVol;
        
        //再生
        _AudioSource.PlayOneShot(_ClickSound);
    }

    #endregion

    /// <summary> クリックされたら音を流す </summary>
    private void PlayClickSound(AudioClip audioClip)
    {
        if (audioClip == null || _AudioSource == null)
        {
            return;
        }

        //一旦止める
        _AudioSource.Stop();

        _AudioSource.volume = _ButtonVol;
        
        //再生
        _AudioSource.PlayOneShot(audioClip);
    }

    //値を与えるメソッド(https://note.com/suzukijohnp/n/n050aa20a12f1)
    void GiveDay(Scene next, LoadSceneMode mode)
    {
        switch(_NextSceneName)
        {
            case "Game":
                {
                    //シーン切り替え後のスクリプトを取得
                    var gameManager = GameObject.Find("Managers").GetComponent<GameSystem>();

                    //値を渡す
                    gameManager.CurrentDay = _CurrentDay;

                    //イベントから削除
                    SceneManager.sceneLoaded -= GiveDay;
                    break;
                }
            case "Menu":
                {
                    //シーン切り替え後のスクリプトを取得
                    var calenderManager = GameObject.Find("Calendar").GetComponent<CalenderManager>();

                    //値を渡す
                    calenderManager.CurrentDay = CurrentDay;

                    //イベントから削除
                    SceneManager.sceneLoaded -= GiveDay;
                    break;
                }
        }
    }
}
