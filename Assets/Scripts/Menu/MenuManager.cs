using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> メニューシーンの管理クラス </summary>
public class MenuManager : MonoBehaviour
{
    #region field
    /// <summary> 選択してる日付 </summary>
    private int _CurrentDay = 1;
    /// <summary> ゲームをスタートしたかどうか </summary>
    private bool _HasStartGame = false;
    #endregion

    #region property
    /// <summary> 現在の日にち </summary>
    public int CurrentDay
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


    /// <summary> ゲームを始めたかどうか </summary>
    public bool HasStartGame
    {
        set { _HasStartGame = value; }
        get { return _HasStartGame; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //何も保存されてなければ０日目にする
        //(ここで保存した日にちは終わらせた日付。なので最新の日は day + 1 になる)
        if(!PlayerPrefs.HasKey("day"))PlayerPrefs.SetInt("day",0);
        //day = PlayerPrefs.GetInt("day") + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(HasStartGame)
        {
            SceneManager.sceneLoaded += GiveDay;
            SceneManager.LoadScene("Game");
        }
    }

    //次のシーンのクラスに値を与える(https://note.com/suzukijohnp/n/n050aa20a12f1)
    void GiveDay(Scene next, LoadSceneMode mode){
        //シーン切り替え後のスクリプトを取得
        var GameManager = GameObject.Find("Managers").GetComponent<GameSystem>();

        //値を渡す
        GameManager.CurrentDay = CurrentDay;
        //イベントから削除
        SceneManager.sceneLoaded -= GiveDay;
    }
}