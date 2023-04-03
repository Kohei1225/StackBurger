using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    GameObject _SecondObject;
    GameObject _ThirdObject;
    /// <summary> タイトル画面の各状態 </summary>
    public enum TitleState
    {
        Title,
        Explain,
        Option,
        Ranking,
        Warning,
    }

    /// <summary> タイトル画面の状態 </summary>
    public TitleState _State = TitleState.Title;
    /// <summary> 遊び方の説明用オブジェクト </summary>
    [SerializeField]private GameObject _Explain = null;
    /// <summary> 設定用オブジェクト </summary>
    [SerializeField]private GameObject _Option = null;
    /// <summary> ランキング表示用オブジェクト </summary>
    [SerializeField] private GameObject _Ranking = null;
    /// <summary> 警告表示用オブジェクト </summary>
    [SerializeField] private GameObject _Warning = null;
    /// <summary> オプションのボタンの配列 </summary>
    [SerializeField] private Button[] _OptionButtons = null;
    [SerializeField] private GameObject _titleCanvas = null;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Complete") == 0)GameObject.Find("Complete").SetActive(false);
        _Explain.SetActive(false);
        _Option.SetActive(false);
        _Warning.SetActive(false);
        _SecondObject = GameObject.Find("secondObject");
        _SecondObject.SetActive(false);
        _ThirdObject = GameObject.Find("thirdObject");
        _ThirdObject.SetActive(false);
        if(!PlayerPrefs.HasKey("day"))PlayerPrefs.SetInt("day",0);
        if(PlayerPrefs.GetInt("day") >= 4)_SecondObject.SetActive(true);
        if(PlayerPrefs.GetInt("day") >= 5)_ThirdObject.SetActive(true);

        if(_Explain == null)
        {
            Debug.Log("ExPlainがアタッチされてない");
        }
        if(_Option == null)
        {
            Debug.Log("Optionがアタッチされてない");
        }
        if(_Ranking == null)
        {
            Debug.LogError("Ranking is no attached !!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _Explain.SetActive(_State == TitleState.Explain);
        _Option.SetActive(_State == TitleState.Option || _State == TitleState.Warning);
        _Ranking.SetActive(_State == TitleState.Ranking);
        _Warning.SetActive(_State == TitleState.Warning);
        _titleCanvas.SetActive(_State == TitleState.Title);

        if(PlayerPrefs.GetInt("Complete") == 0 && GameObject.Find("Complete"))
            GameObject.Find("Complete").SetActive(false);
    }

    public void SetButonsInteractable(bool value)
    {
        for(int i = 0;i < _OptionButtons.Length;i++)
        {
            _OptionButtons[i].interactable = value;
        }
    }
}
