using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    TitleManager _TitleManager;//メインメニューマネージャー
    ControlSound _BGMOption;//BGMの設定
    ControlSound _ButtonOption;//ボタンの設定
    ControlSound _SEOption;//その他の音の設定

    public float BGMVolume = 0.5f;//BGMの初期値
    public bool BGMExist = true;
    public float ButtonVolume = 0.2f;//ボタン音の初期値
    public bool ButtonExist = true;
    public float SEVolume = 1f;//効果音の初期値
    public bool SEExist = true;

    // Start is called before the first frame update
    void Start()
    {
        
        //メインメニューだったら
        if(GameObject.Find("firstObject")){
            _TitleManager = GameObject.Find("Managers").GetComponent<TitleManager>();
            _BGMOption = GameObject.Find("BGMVolume").GetComponent<ControlSound>();
            _ButtonOption = GameObject.Find("ButtonVolume").GetComponent<ControlSound>();
            _SEOption = GameObject.Find("SEVolume").GetComponent<ControlSound>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.name);
        if(GameObject.Find("firstObject"))
        {
            if(_TitleManager._State == TitleManager.TitleState.Option)
            {
                //BGM設定の更新
                BGMVolume = _BGMOption._Volume;
                BGMExist = _BGMOption._Exist;
                //ボタン設定の更新
                ButtonVolume = _ButtonOption._Volume;
                ButtonExist = _ButtonOption._Exist;
                //他の音の設定
                SEVolume = _SEOption._Volume;
                SEExist = _SEOption._Exist;
            }
        }

    }
}
