using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    GameObject _SecondObject;
    GameObject _ThirdObject;
    /// <summary> タイトル画面の各状態 </summary>
    public enum TitleState
    {
        Title,
        Explain,
        Option,
    }

    /// <summary> タイトル画面の状態 </summary>
    public TitleState _State = TitleState.Title;
    /// <summary> 遊び方の説明用オブジェクト </summary>
    [SerializeField]private GameObject _Explain = null;
    /// <summary> 設定用オブジェクト </summary>
    [SerializeField]private GameObject _Option = null;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Complete") == 0)GameObject.Find("Complete").SetActive(false);
        _Explain.SetActive(false);
        _Option.SetActive(false);
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
    }

    // Update is called once per frame
    void Update()
    {
        //遊び方説明表示
        if(_State == TitleState.Explain)
        {
            _Explain.SetActive(true);
        }
        //オプション表示
        else if(_State == TitleState.Option)
        {
            _Option.SetActive(true);
        }
        //タイトル画面
        else 
        {
            _Explain.SetActive(false);
            _Option.SetActive(false);
        }
        if(PlayerPrefs.GetInt("Complete") == 0 && GameObject.Find("Complete"))GameObject.Find("Complete").SetActive(false);
    }
}
