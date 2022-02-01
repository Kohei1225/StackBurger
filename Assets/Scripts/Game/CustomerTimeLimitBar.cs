using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 客の制限時間を制御するクラス </summary>
public class CustomerTimeLimitBar : MonoBehaviour
{
    /// <summary> RGBのRの極限値 </summary>
    const float R_LIMIT = 231;
    /// <summary> RGBのGの極限値 </summary>
    const float G_LIMIT = 0;
    /// <summary> RGBのRの初期値 </summary>
    const float R_INI = 60;
    /// <summary> RGBのGの初期値 </summary>
    const float G_INI = 231;

    #region private field
    private Slider _Slider;
    private Image _FillImage;
    private float _TotalRGBRange;
    private float _RValue;
    private float _GValue;  
    private float _RRange;
    private float _GRange;
    private bool _HasReachRLimit = false;
    private bool _HasReachGLimit = false;
    #endregion

    #region property
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
            if (value <= R_INI)
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
    #endregion

    #region private function
    // Start is called before the _first frame update
    void Start()
    {
        _Slider = GetComponent<Slider>();
        _RValue = R_INI;//|231 - 60| = 171カウント
        _GValue = G_INI;//|0 - 231| = 231カウント
        _RRange = Mathf.Abs(R_LIMIT - R_INI);
        _GRange = Mathf.Abs(G_LIMIT - G_INI);
        _TotalRGBRange = _RRange + _GRange;
        _FillImage = transform.Find("Fill Area").gameObject.transform.Find("Fill").GetComponent<Image>();   
    }

    /// <summary> スライダーの進み加減に応じて色を変える </summary>
    /// <param name="rateOfRemainingTime"> 残り時間の割合 </param>
    void ChangeColor(float rateOfTime)
    {
        float rate_of_total_range = rateOfTime * _TotalRGBRange;

        if (!_HasReachRLimit)
        {
            RValue = rate_of_total_range + R_INI;
        }
        else if (!_HasReachGLimit)
        {
            GValue = G_INI - (rate_of_total_range - _RRange);
        }

        //R:60->231 G:231->0
        _FillImage.color = new Color(RValue/255f , GValue/255f, 0);
    }
    #endregion

    #region public function
    public void InitializeValue()
    {
        _RValue = R_INI;//初期化
        _GValue = G_INI;//初期化

        _FillImage.color = new Color(R_INI/255f, G_INI/255f, 0);

        _HasReachRLimit = false;
        _HasReachGLimit = false;
    }

    public void UpdateTimeBar(float rateOfRemainingTime)
    {
        _Slider.value = rateOfRemainingTime;
        ChangeColor(1-rateOfRemainingTime);
    }
    #endregion
}