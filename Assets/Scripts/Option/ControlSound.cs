using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> スライダーで音量を調整するクラス </summary>
public class ControlSound : MonoBehaviour
{
    public float _Volume;//ボリューム。他のスクリプト(多分サウンドマネージャー)から値を受け取る
    public bool _Exist = true;//再生するかどうか

    Toggle CheckBox;//ON/OFFのチェックボックス
    Slider VolumeSlider;

    SoundScript soundManager;

    Text text;//値を表示するテキスト

    public bool _ResetFlag;//値を初期化する時の印

    const float BGM_VOLUME = 0.5f;
    const float BUTTON_VOLUME = 0.2f;
    const float SE_VOLUME = 0.6f;

    string _VolName = "null";
    string _ExName = "null";

    // Start is called before the first frame update
    void Start()
    {
        VolumeSlider = GetComponent<Slider>();
        soundManager = GameObject.Find("Managers").GetComponent<SoundScript>();

        //VolumeSlider.value = Volume;
        //最初に与えられた値を元に設定する   
        //BGMの設定    
        if(gameObject.name == "BGMVolumeSlider")
        {
            _VolName = "BGMvol";
            _ExName = "BGMex";


            CheckBox = GameObject.Find("BGMBox").GetComponent<Toggle>();
            text = GameObject.Find("BGMValue").GetComponent<Text>();
        }
        //ボタン音の設定
        else if(gameObject.name == "ButtonVolumeSlider")
        {
            CheckBox = GameObject.Find("ButtonBox").GetComponent<Toggle>();
            _VolName = "Buttonvol";
            _ExName = "Buttonex";
            text = GameObject.Find("ButtonValue").GetComponent<Text>(); 
        }

        //その他の音(ドア、シンバル、ドラムロール)の設定
        else if(gameObject.name == "SEVolumeSlider")
        {
            CheckBox = GameObject.Find("SEBox").GetComponent<Toggle>();
            _VolName = "SEvol";
            _ExName = "SEex";
            text = GameObject.Find("SEValue").GetComponent<Text>(); 
        }

        //データがなかったら作る
        if(!PlayerPrefs.HasKey(_VolName))
        {
            PlayerPrefs.SetFloat(_VolName,0.5f);
        }
        if(!PlayerPrefs.HasKey(_ExName))PlayerPrefs.SetInt(_ExName,1);

        //スライダーの初期値を決める
        VolumeSlider.value = PlayerPrefs.GetFloat(_VolName);
        if(PlayerPrefs.GetInt(_ExName) == 1)CheckBox.isOn = true;
        else CheckBox.isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Volume = VolumeSlider.value;
        //Exist = CheckBox.isOn;
        text.text = ((int)(VolumeSlider.value * 100)).ToString();
        if(_ResetFlag)
        {
            if (gameObject.name == "BGMVolumeSlider")
            {
                VolumeSlider.value = BGM_VOLUME;
            }
            else if (gameObject.name == "ButtonVolumeSlider")
            {
                VolumeSlider.value = BUTTON_VOLUME;
            }
            else if (gameObject.name == "SEVolumeSlider")
            {
                VolumeSlider.value = SE_VOLUME;
            }
            CheckBox.isOn = true;
            _ResetFlag = false;
        }
        if(PlayerPrefs.HasKey(_VolName))PlayerPrefs.SetFloat(_VolName,VolumeSlider.value);
        if(PlayerPrefs.HasKey(_ExName))
        {
            //もしONなら1、OFFなら0にする
            if(CheckBox.isOn)PlayerPrefs.SetInt(_ExName,1);
            else PlayerPrefs.SetInt(_ExName,0);
        }
    }
}

