using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 制限時間を制御するクラス </summary>
public class TimeLimitScript : MonoBehaviour
{
    /// <summary> 制限時間 </summary>
    const int TIMELIMIT = 210;

    GameSystem _GameManager;
    Text _CurrentTimeText;

    /// <summary> 現在の経過時間 </summary>
    float _CurrentTime = 0;

    /// <summary> 時計の針のオブジェクト </summary>
    GameObject _ClockNeedle;
    /// <summary> 経過時間を色で埋めるイメージ </summary>
    Image _ClockFillColor;


    // Start is called before the first frame update
    void Start()
    {
        _GameManager = GameObject.Find("Managers").GetComponent<GameSystem>();
        _CurrentTimeText = GameObject.Find("TimeLimit").GetComponent<Text>();
        _ClockNeedle = GameObject.Find("ClockNeedle");
        _ClockFillColor = GameObject.Find("ClockColor").GetComponent<Image>();

        if((TIMELIMIT / 60) < 10)_CurrentTimeText.text = "0";
        else _CurrentTimeText.text = "";
        _CurrentTimeText.text += (TIMELIMIT / 60) + ":";
        if((TIMELIMIT % 60) < 10)_CurrentTimeText.text += "0";
        _CurrentTimeText.text += (TIMELIMIT % 60);

        _ClockFillColor.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_GameManager._IsStart)
        {
            //現在の時間の取得
            _CurrentTime += Time.deltaTime;
            int minute = (TIMELIMIT - (int)_CurrentTime) / 60;
            int second = (TIMELIMIT - (int)_CurrentTime) % 60;

            //テキストに２桁になるように数値を代入
            if(minute < 10) _CurrentTimeText.text = "0";
            else _CurrentTimeText.text = "";
            _CurrentTimeText.text += minute + ":";
            if(second < 10)_CurrentTimeText.text += "0";
            _CurrentTimeText.text += second;

            //時計を経過時間によって色で埋める
            _ClockFillColor.fillAmount = _CurrentTime / TIMELIMIT;
            _ClockNeedle.transform.rotation = Quaternion.Euler(0, 0, -(_CurrentTime / TIMELIMIT) * 360f);
            if(_CurrentTime >= TIMELIMIT)
            {
                _GameManager._HasFinish = true;
                _CurrentTimeText.text = "00:00";
                _ClockFillColor.fillAmount = 1;
                _ClockNeedle.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
