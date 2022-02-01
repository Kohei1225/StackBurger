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

            if(_ActiveSlider)
            {

                if(_ChangeCustomerFlag)SetCustomer();
                //客がいなくなる時
                if(_CustomerInfo._HasRecieve)
                {
                    _Slider.value = 0;
                }
                //カウントしていい状態だったら
                if(_CustomerInfo.CanReceiveFood)
                {
                    _Slider.value = _CustomerInfo.MainTimer.RateOfRemainingTime;
                    
                    //時間切れになったら
                    if(_Slider.value <= 0 && !_Flag)
                    {
                        _CustomerInfo.timeOver = true;
                        _Flag = true;
                        _RValue = R_INI;//初期化
                        _GValue = G_INI;//初期化      
                        _RStop = false;
                    }
                    else ChangeColor();
                }            

                //客がまだ来てない時
                else if(!_CustomerInfo._HasVisit)
                {
                    //スライダーとかの情報をセットしておく
                    var orderNum = _CustomerInfo._OrderNum;
                    var recipe = RecipeList.Menu[orderNum].recipe;
                    var foodHeight = RecipeList.CalcHeight(recipe);
                    if (RecipeList.Menu[orderNum].type != RecipeList.ProductType.RANDOM)
                    {
                        //_Slider.maxValue = foodHeight * (_MultiLevel + _GameManager._CustomerNum * 2);
                    }
                    else
                    {
                        //_Slider.maxValue = 10 * (_MultiLevel + _GameManager._CustomerNum * 2);
                    }
                    _Slider.value = _Slider.maxValue;
                }

                _FillImage.color = new Color(_Red , _Green, 0);

                //色の変更
                //時間がない(赤)
                if(_Slider.value < _Slider.maxValue / 3f)
                {
                    //FillImage.color = new Color(231 / 255f, 0, 0);
                    if(_ChangeChip == 1)
                    {
                        _CustomerInfo.chip -= _CustomerInfo.chipLevel;
                        _ChangeChip--;
                    }
                }
                //普通(黄色)
                else if(_Slider.value < (_Slider.maxValue / 3f) * 2)
                {
                    //FillImage.color = new Color(231 / 255f, 198 / 255f,0);
                    if(_ChangeChip == 2)
                    {
                        _CustomerInfo.chip -= _CustomerInfo.chipLevel;
                        _ChangeChip--;
                    }
                }
                
                //60~231
                //231~0

            }     
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

    /// <summary> 自分が担当する客の情報をセットする </summary>
    void SetCustomer()
    {
        _CustomerInfo = _CustomerManager.CustomerInfo[_Number - 1];
        _RValue = R_INI;//初期化
        _GValue = G_INI;//初期化

        _Red = R_INI / 255f;    //
        _Green = G_INI / 255f;

        _FillImage.color = new Color(R_INI/255f, G_INI/255f, 0);
        _RStop = false;
        _ChangeChip = 2;
        _ChangeCustomerFlag = false;
        _Flag = false;
    }

    /// <summary> スライダーの進み加減に応じて色を変える </summary>
    void ChangeColor()
    {
        float _number;
        //バーの進んだ割合
        _number = ( 1 - (_Slider.value / _Slider.maxValue) ) * _TotalRGBRange;

        //if (_ForDebug) Debug.Log("number:" + _number);
        _RValue = _number;

        //赤の処理
        if(_RValue == R_LIMIT){
            if (_ForDebug) Debug.Log("Red()");
            if(R_LIMIT == 0)_Red = 0;
            else _RValue = R_LIMIT;
        }
        else if(!_RStop){
            _RValue = _number + R_INI;
            _Red = _RValue / 255f;
        } 
        else _RValue = R_LIMIT;
        if(_RValue >= R_LIMIT)_RStop = true;
        

        //緑の処理
        if(_number >= _RRange){
            if(_GValue == G_LIMIT)
            {
                if (_ForDebug) Debug.Log("Green()");
                if(G_LIMIT == 0)_Green = 0;
                else _GValue = G_LIMIT;
            }
            else {
                _GValue = G_INI - (_number - _RRange);
                _Green = _GValue / 255f;
            }   
        }

    }
}