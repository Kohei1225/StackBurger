using System.Collections;
using System.Collections.Generic;
/*using System.IO;
using System.Text;
using System.Security.Cryptography;
*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary> ゲーム全体のシステム(得点等)に関わるスクリプト </summary>
public class GameSystem : SingletonMonoBehaviour<GameSystem>
{
    #region field
    /// <summary> 日にち </summary>
    public int _CurrentDay = 0;
    /// <summary> 客の数 </summary>
    public int _CustomerNum{get; private set;} = 2;
    /// <summary>
    /// 出せる料理の範囲(4:初期バーガー 29:バーガー系  34:サンドイッチ  39:マフィン  46:デカい系  59:料理)
    ///何日目かによって増えていく
    /// </summary>
    public int _Range{get; private set;} = 4;
    /// <summary> ゲームが始まってるかどうか </summary>
    public bool _IsStart{get; private set;} = false;
    /// <summary> ゲームが終わったか </summary>
    public bool _HasFinish = false;
    /// <summary> 置き手紙を全部読んだか </summary>
    public bool _HasRead = false;                    

    Animation anim;

    /// <summary> アニメを再生したかどうかの判定 </summary>
    private bool _HasPlayAnim = false;
    /// <summary> 一番最初にする処理につく。やったらfalseにしてできなくする </summary>
    private bool _IsFirstFunc = true;
    /// <summary> キーを押してからゲームが始まるまでの時間稼ぎ </summary>
    private int _Count = 0;              
    TimeLimitScript TimeManager;
    Text _SystemText;
    /// <summary> 終わってから結果画面に遷移するまでの時間 </summary>
    private int _FinishCounter = 0;

    /// <summary> 正しく配給した回数 </summary>
    private int _HappyNum = 0;
    /// <summary> 食材の順番だけ違った回数 </summary>
    private int _ExclamationNum = 0;
    /// <summary> 食材が微妙に違った回数(ギリギリ許容範囲) </summary>
    private int _StrangeNum = 0;
    /// <summary> 普通に間違えた回数(ミス) </summary>
    private int _BadNum = 0;
    /// <summary> 客が怒って帰った回数(時間切れ又はミス3回) </summary>
    private int _AngryNum = 0;
    /// <summary> チップの合計 </summary>
    private float _Chip = 0;
    /// <summary> ゲームで使う食材のプレハブが入ってる配列 </summary>
    [SerializeField] private GameObject[] foodPrefabs;

    /// <summary>
    ///
    /// 
    /// </summary>
    [SerializeField] private GameObject _ChipText = null;
    [SerializeField] private AudioClip _MissSound = null;
    #endregion

    #region property
    public int HappyNum
    {
        get{return this._HappyNum;}
        set{if(value > 0)this._HappyNum = value;Debug.Log("_happyNum:" + _HappyNum);}
    }
    
    public int ExclamationNum
    {
        get{return this._ExclamationNum;}
        set{if(value > 0)this._ExclamationNum = value;}
    } 
    
    public int StrangeNum
    {
        get{return this._StrangeNum;}
        set{if(value > 0)this._StrangeNum = value;}
    }       
    
    public int BadNum
    {
        get { return this._BadNum; }
        set { if (value > 0) this._BadNum = value; }
    }
    
    public int AngryNum
    {
        get{return this._AngryNum;}
        set{if(value > 0)this._AngryNum = value;}
    }      
               
    public float Chip
    {
        get{return _Chip;}
        set{if(value > 0)_Chip = value;}
    }        

    public GameObject[] FoodPrefabs 
    {
        get{return foodPrefabs;}
        set{Debug.Log("foodPrefabへのアクセス権限がありません");}
    }

    public int CurrentDay
    {
        get { return _CurrentDay; }
        set
        {
            if(0 < value && value < 8)
            {
                Debug.Log("Game:" + value);
                _CurrentDay = value;
            }
        }
    }

    public GameObject ChipText
    {
        get { return _ChipText; }
    }


    #endregion
    
    void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the _first frame update
    void Start()
    {
        //day = 7;

        Debug.Log("day(Game):" + CurrentDay);

        //難易度設定
        switch(CurrentDay)
        {
            case 1:
                this._CustomerNum = 1;
                _Range = 4;
                break;
            case 2:
                this._CustomerNum = 2;
                _Range = 4;
                break;
            case 3:
                this._CustomerNum = 2;
                _Range = 29;
                break;
            case 4:
                this._CustomerNum = 2;
                _Range = 34;
                break;
            case 5:
                this._CustomerNum = 2;
                _Range = 39;
                break;
            case 6:
                this._CustomerNum = 2;
                _Range = 46;
                break;
            case 7:
                this._CustomerNum = 3;
                _Range = 59;
                break;
            case -1:
                this._CustomerNum = 1;
                _Range = 1;
                break;
            default:
                this.CurrentDay = 7;
                this._CustomerNum = 1;
                _Range = 59;
                break;
        }

        //this.customerNum = 2;
        //range = 24;
        anim = GameObject.Find("ShutterObject").GetComponent<Animation>();
        _SystemText = GameObject.Find("SystemUI").GetComponent<Text>();
        _SystemText.text = "Press Space to Start !!";
        GameObject.Find("OpenSound").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SEvol") * PlayerPrefs.GetInt("SEex");
        GameObject.Find("CloseSound").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SEvol") * PlayerPrefs.GetInt("SEex");

        GameObject.Find("OpenSound").SetActive(false);
        GameObject.Find("CloseSound").SetActive(false);

        //PlayerPrefs.SetInt("day",5);
        //TimeManager = GameObject.Find("ShutterObject").GetComponent<>();
    }

    // Update is called once per frame
    void Update()
    {
        //スタートする前だったら(最初の一回のみ実行)
        if(_IsFirstFunc)
        {
            if(_HasRead && Input.GetKey(KeyCode.Space))
            {
                if(!_HasPlayAnim && _IsFirstFunc)
                {
                    anim.Play("OpenShutter");
                    _HasPlayAnim = true;
                    _IsFirstFunc = false;
                    _SystemText.text = "";
                }
            }
        }

        //アニメが再生されたか
        if(_HasPlayAnim)
        {
            if(!_IsFirstFunc)_Count++;
            if(_Count > 150)
            {
                //一定時間経ったらゲームを開始する。
                _IsStart = true;
                GameObject.Find("BGM").GetComponent<BGMScript>()._Start = true;
            }
        }

        //ゲーム終了
        if(_HasFinish)
        {
            _IsStart = false;
            if(_HasPlayAnim)
            {
                GameObject.Find("BGM").GetComponent<BGMScript>()._Start = false;
                anim.Play("CloseShutter");
                _HasPlayAnim = false;
                
                _SystemText.text = "\nFinish !!\n";
            }
            //GameObject.Find("Result").GetComponent<Text>().text = this.HappyNum.ToString();
            if(!_HasPlayAnim)_FinishCounter++;
        }
        if(_FinishCounter > 100)
        {
            SceneManager.sceneLoaded += GiveScore;
            //if(AudioFlag)audioSource.Stop();
            SceneManager.LoadScene("Result");
        }
    }

    public void PlayMissSound()
    {
        GetComponent<AudioSource>().PlayOneShot(_MissSound);
    }

    //値を与えるメソッド(https://note.com/suzukijohnp/n/n050aa20a12f1)
    void GiveScore(Scene next, LoadSceneMode mode){
        //シーン切り替え後のスクリプトを取得
        var resultManager = GameObject.Find("Managers").GetComponent<ResultManager>();

        resultManager.GoodSum = this.HappyNum + this.ExclamationNum + this.StrangeNum;
        resultManager.AngrySum = this.AngryNum;
        resultManager.MissSum = this.BadNum;
        resultManager.CurrentDay = this.CurrentDay;
        resultManager.ChipSum = this._Chip;
        resultManager.HappySum = this.HappyNum;

        //イベントから削除
        SceneManager.sceneLoaded -= GiveScore;
    }
}
