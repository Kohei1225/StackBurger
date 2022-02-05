using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
/// <summary> 食材自体にアタッチするクラス </summary>
public class FoodScript : MonoBehaviour
{
    PlateScript _PlateScript;        //プレートのスクリプト
    GameObject _Plate;
    GameObject _Select;              //プレート選択オブジェクト
    SelectPlate _SelectManager;

    /// <summary> </summary>
    private float _YPos;
    /// <summary>  </summary>
    public float _ZPos;
    /// <summary>  </summary>
    private bool _IsFood = true;     //ゲーム上で動く食材か(不具合が起こるとfalseになる)
    /// <summary>  </summary>
    private bool _Move = false;      //動いてるかどうかの判定

    private float depth;
    /// <summary>  </summary>
    private float _Ytmp;   

    /// <summary>  </summary>
    public bool _TopMove = false;    //一番上だけの移動
    /// <summary>  </summary>
    public bool _AllMove = false;       //全体の移動

    // Start is called before the first frame update
    void Start()
    {
        SetPlate();
        _Select = GameObject.Find("Select");
        _SelectManager = _Select.GetComponent<SelectPlate>();
    }

    // Update is called once per frame
    void Update()
    {
        //
        if(_IsFood)
        {
            //合図がきたら消す
            if(_PlateScript.abandanFlag)Destroy(gameObject);
                
            if(!_PlateScript.moveTopFlag)_TopMove = false;
            if(!_PlateScript.moveFlag)_AllMove = false;

            //合図がきたら浮かぶ
            if(_PlateScript.moveFlag || _TopMove || _AllMove)
            {
                //まだ動いてなかったら最初にここで値を決めちゃう
                GetComponent<SpriteRenderer>().color = new Color(0.75f,0.75f,0.75f);

                //これから動くところなら
                if(!_Move)
                {
                    //自分の位置を調整
                    _YPos = transform.position.y - _Plate.transform.position.y;
                    _ZPos = _Plate.transform.position.z - transform.position.z;

                    //自身が一番上の食材かつ
                    if(_TopMove)
                    {
                        _YPos = _Plate.GetComponent<PlateScript>().dropHeight - _Select.transform.position.y - 5;
                        _Ytmp = _YPos;
                        _ZPos = 0;
                    }

                    //奥行きの座標を調整する
                    _ZPos = Rounding(_ZPos);
                }

                _Move = true;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);


                //プレイヤーが選択したプレートが自分がいるプレートだったら
                if(_SelectManager.plateNum == _PlateScript.plateNumber)
                {
                    if(_TopMove)
                    {
                        _ZPos = 0;
                        _YPos = _Ytmp;
                        
                    }
                    //Debug.Log(SelectManager.Plate[SelectManager.plateNum].name + ":" + SelectManager.Plate[SelectManager.plateNum].transform.position.z);
                    if(_TopMove)transform.position = new Vector3(_Select.transform.position.x, _YPos + _Select.transform.position.y + 5, -_ZPos + _SelectManager.Plate[_SelectManager.plateNum].GetComponent<PlateScript>().depth);
                    else transform.position = new Vector3(_Select.transform.position.x, _YPos + _Select.transform.position.y + 5, -_ZPos + _SelectManager.Plate[_SelectManager.plateNum].transform.position.z);
                }
                else 
                {
                    if(_TopMove)
                    {
                        _ZPos = 0.1f;
                        _YPos = 0;
                        //GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f);
                    }
                    //if(topMove)zpos = 0.1f;
                    //if(SelectManager.Plate[SelectManager.plateNum].GetComponent<StackScript>().top > -1 && SelectManager.Plate[SelectManager.plateNum].GetComponent<StackScript>().stackedfood[SelectManager.Plate[SelectManager.plateNum].GetComponent<StackScript>().top].transform.position.y > SelectManager.Plate[SelectManager.plateNum].GetComponent<StackScript>().dropHeight)
                    //transform.position = new Vector3(Select.transform.position.x, ypos + SelectManager.Plate[SelectManager.plateNum].GetComponent<StackScript>().stackedfood[SelectManager.Plate[SelectManager.plateNum].GetComponent<StackScript>().top].transform.position.y + 5, -zpos + SelectManager.Plate[SelectManager.plateNum].GetComponent<StackScript>().depth);
                    transform.position = new Vector3(_Select.transform.position.x, _YPos +/*Select.transform.position.y + 5 + */_SelectManager.Plate[_SelectManager.plateNum].GetComponent<PlateScript>().dropHeight, -_ZPos + _SelectManager.Plate[_SelectManager.plateNum].GetComponent<PlateScript>().depth);
                }
            }
            else
            {
                if(_Move)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                    SetPlate();
                }
                _Move = false;
                GetComponent<SpriteRenderer>().color = new Color(1,1,1);
                SetPlate();
            }

        }
        else GetComponent<SpriteRenderer>().color = new Color(1,1,1);

    }

    /// <summary> 自分が乗るプレートを設定する </summary>
    void SetPlate()
    {
        var xPos = gameObject.transform.position.x;
        if(Mathf.Abs(xPos - GameObject.Find("Plate").transform.position.x) < 0.5f)
        {
            _Plate = GameObject.Find("Plate");
            _PlateScript = _Plate.GetComponent<PlateScript>();
        }
        else if(Mathf.Abs(xPos - GameObject.Find("Plate1").transform.position.x) < 0.5f)
        {
            _Plate = GameObject.Find("Plate1");
            _PlateScript = _Plate.GetComponent<PlateScript>();
        }
        else if(Mathf.Abs(xPos - GameObject.Find("Plate2").transform.position.x) < 0.5f)
        {
            _Plate = GameObject.Find("Plate2");
            _PlateScript = _Plate.GetComponent<PlateScript>();
        }
        else if(Mathf.Abs(xPos - GameObject.Find("Plate3").transform.position.x) < 0.5f)
        {
            _Plate = GameObject.Find("Plate3");
            _PlateScript = _Plate.GetComponent<PlateScript>();
        }
        else _IsFood = false;
    }

    /// <summary> 四捨五入する </summary>
    /// <param name="num">四捨五入したい数値</param>
    /// <returns>四捨五入した数値</returns>
    float Rounding(float num)
    {
        int decimal1 = (int)num;
        int decimal2 = (int)(num * 10) - decimal1 * 10;
        int decimal3 = (int)(num * 100) - decimal1 * 100 - decimal2 * 10;

        if(decimal3 > 4)decimal3++;

        return (decimal1 * 100 + decimal2 * 10 + decimal3) / 100f;
    }
    
}
