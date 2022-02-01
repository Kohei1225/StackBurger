using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 客の制限時間を制御するクラス </summary>
public class CustomerTimeLimitBar : MonoBehaviour
{
    GameSystem _GameManager;
    SelectCustomer　_SelectCustomerManager;
    CustomerManager _CustomerManager;
    CustomerScript _CustomerInfo;
    Slider _Slider;

    public Image _FillImage;
    public bool _ChangeCustomerFlag = false;
    public bool _ForDebug = false;

    private bool _ActiveSlider = false;
    private int _Number = 5;
    private bool _First = true;
    private bool _Flag = false;


    private int _ChangeChip = 2;        //3からカウントしていく
    private float _TotalRGBRange;           //カウントしていく単位
    /// <summary> 255で割る前の実際の値 </summary>
    private float _RValue;
    /// <summary> 255で割る前の実際の値 </summary>
    private float _GValue;
    /// <summary> 実際にColor()の引数にする値(赤) </summary>
    private float _Red;
    /// <summary> 実際にColor()の引数にする値(緑) </summary>
    private float _Green;                
    private float _RRange;
    private float _GRange;
    private bool _RStop = false;
    private int _MultiLevel = 8;
    private bool _HasReachRLimit = false;
    private bool _HasReachGLimit = false;

    /// <summary> RGBのRの最大値 </summary>
    const float R_LIMIT = 231;
    /// <summary> RGBのGの最大値 </summary>
    const float G_LIMIT = 0;
    /// <summary> RGBのRの初期値 </summary>
    const float R_INI = 60;
    /// <summary> RGBのGの初期値 </summary>
    const float G_INI = 231;               

    private float RValue
    {
        set
        {
            if(value >= R_LIMIT)
            {
                _RValue = R_LIMIT;
                _HasReachRLimit = true;
                return;
            }
            if(value <= R_INI)
            {
                _RValue = R_INI;
                return;
            }

            _RValue = value;
        }
        get { return _RValue; }
    }

    private float GValue
    {
        set
        {
            if (value <= G_LIMIT)
            {
                _GValue = G_LIMIT;
                _HasReachGLimit = true;
                return;
            }
            if (value >= G_INI)
            {
                _GValue = G_INI;
                return;
            }

            _GValue = value;
        }
        get { return _GValue; }
    }

    // Start is called before the _first frame update
    void Start()
    {
        _GameManager = GameObject.Find("Managers").GetComponent<GameSystem>();
        _SelectCustomerManager = GameObject.Find("CustomerPlate").GetComponent<SelectCustomer>();
        _CustomerManager = GameObject.Find("Managers").GetComponent<CustomerManager>();
        _Slider = GetComponent<Slider>();
        _RValue = R_INI;//|231 - 60| = 171カウント
        _GValue = G_INI;//|0 - 231| = 231カウント
        _RRange = Mathf.Abs(R_LIMIT - R_INI);
        _GRange = Mathf.Abs(G_LIMIT - G_INI);
        _TotalRGBRange = _RRange + _GRange;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームが始まっててJudgeScriptでの最初の処理が終わってる
        if(_GameManager._IsStart && !_CustomerManager.HasFinishFirst)
        {
            //このスクリプトでの最初の処理
            if(_First)
            {
                _ActiveSlider = IsActiveSlider();
                _Slider.value = 0;
                _FillImage = GameObject.Find("Fill" + _Number).GetComponent<Image>();   
                _First = false;         
                
                if(_RValue == 0)_Red = 0;
                else _Red = _RValue / 255f;
                if(_GValue == 0)_Green = 0;
                else _Green = _GValue / 255f;
            }
            //R:60->231 G:231->0
        }       
    }

    /// <summary> 自分が有効なスライダーかどうか判定する </summary>
    /// <returns> スライダーが有効かどうか </returns>
    bool IsActiveSlider()
    {
        if(gameObject.name == "TimeLimitBar1")_Number = 1;
        else if(gameObject.name == "TimeLimitBar2")_Number = 2;
        else if(gameObject.name == "TimeLimitBar3")_Number = 3;

        if(_GameManager._CustomerNum >= _Number)return true;

        return false;
    }

    public void InitializeValue()
    {
        _RValue = R_INI;//初期化
        _GValue = G_INI;//初期化

        _Red = R_INI / 255f;    //
        _Green = G_INI / 255f;

        _FillImage.color = new Color(R_INI/255f, G_INI/255f, 0);

        _HasReachRLimit = false;
        _HasReachGLimit = false;
    }

    public void UpdateTimeBar(float rateOfRemainingTime)
    {
        _Slider.value = rateOfRemainingTime;
        ChangeColor(1-rateOfRemainingTime);
    }

    /// <summary> スライダーの進み加減に応じて色を変える </summary>
    /// <param name="rateOfRemainingTime"> 残り時間の割合 </param>
    void ChangeColor(float rateOfRemainingTime)
    {
        float rate_of_total_range = rateOfRemainingTime * _TotalRGBRange;;
        //バーの進んだ割合
        if(_ForDebug)Debug.Log("rate_of_total_range:"+rate_of_total_range);

        if (!_HasReachRLimit)
        {
            RValue = rate_of_total_range + R_INI;
        }
        else if (!_HasReachGLimit)
        {
            GValue = G_INI - (rate_of_total_range - _RRange);
        }

        if (_ForDebug) Debug.Log("R:" + RValue + " G:" + GValue);
        _Red = RValue / 255f;
        _Green = GValue / 255f;

        _FillImage.color = new Color(RValue/255f , GValue/255f, 0);
    }
}