using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//具材の動きに関するクラス
public class MoveOfFood : MonoBehaviour
{
    GameObject _Managers;
    GameSystem _GameManager;
    CustomerManager _CustomerManager;
    SelectPlate _SelectManager;
    AudioSource _AudioSource;
    [SerializeField]MonitorScript [] _Displays = new MonitorScript[3];//モニターのスクリプトの配列
    [SerializeField]PlateScript [] _Plates = new PlateScript[3];   //プレートにアタッチされてるスクリプト
    
    public AudioClip _GarbageSound;
    public int [] _CandidateFoodsToDrop;//落とす具材の配列
    [System.NonSerialized]public bool _IsMovingAllFoods = false;//食材(全部)のプレート間の移動(浮いてるかどうか)
    [System.NonSerialized]public bool _IsMovingTopFood = false;//食材(一枚)のプレート間の移動(浮いてるかどうか)
    [System.NonSerialized] public int _RootPlate;//移動元のプレート番号
    [System.NonSerialized] public bool reset = false;
    
    private int _fallFoodCounter = 0;   //落とした食材の数(配列の番号を指す)
    private int _CurrentFoodNumbers;           //落とす具材の番号
    private int _FurtherNextFoodNumbers;       //次に落とす具材の番号
    /// <summary> ストックしてる具材の番号 </summary>
    private int _StockFoodNumbers = -1; 
    private bool _StockFlag = false;//ストックしてたらtrue
    private bool _HasFinishFirst = false;    //最初の処理かどうかの判定

    bool IsMoving
    {
        get { return _IsMovingAllFoods || _IsMovingTopFood; }
    }

    // Start is called before the _first frame update
    void Start()
    {
        //コンポーネントなどを取得
        _Managers = GameObject.Find("Managers");
        _GameManager = _Managers.GetComponent<GameSystem>();
        _CustomerManager = _Managers.GetComponent<CustomerManager>();
        _SelectManager = GameObject.Find("Select").GetComponent<SelectPlate>();
        _AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_GameManager._IsStart)
        {
            //一番最初だけ
            if(!_HasFinishFirst)
            {
                ResetFoodValue();
                _HasFinishFirst = true;
                _StockFoodNumbers = 0;           
            }
            //リセットする
            if(reset)
            {
                ResetFoodValue();
                reset = false;
            }
            //食材が浮いてる間
            if(IsMoving)
            {
                if(Input.GetKeyDown(KeyCode.C) && _IsMovingAllFoods)
                {
                    //どちらのプレートの量も一定値を超えてなければ落とす
                    if( _RootPlate == _SelectManager.plateNum || ( _SelectManager.plateNum != _RootPlate && (!_Plates[_SelectManager.plateNum].realFull && (_Plates[_SelectManager.plateNum].top + _Plates[_RootPlate].top + 2 < 29) ))){
                        _IsMovingAllFoods = false;

                        //同じプレートじゃなければ登録とかの処理をする
                        if(_RootPlate != _SelectManager.plateNum)
                        {
                            _Plates[_SelectManager.plateNum].add = true;                        
                        }
                        _Plates[_RootPlate].moveFlag = false;
                    }
                        
                }
                else if(Input.GetKeyDown(KeyCode.X) && _IsMovingTopFood)
                {
                    //相手のプレートの量が一定値を超えてなければ落とす
                    if( _RootPlate == _SelectManager.plateNum || ( _SelectManager.plateNum != _RootPlate && !_Plates[_SelectManager.plateNum].realFull))
                    {
                        //Debug.Log("左shift");
                                                
                        //同じプレートじゃなければ登録とかの処理をする
                        if(_RootPlate != _SelectManager.plateNum)
                        {
                            _Plates[_SelectManager.plateNum].add = true;                        
                            //Stack[rootPlate].reset = true;
                        }
                        else _IsMovingTopFood = false;    
                        _Plates[_RootPlate].moveTopFlag = false;
                    }
                }
            }
            //食材が浮いてない時
            else
            {
                //右Shiftで全部移動させる
                if(Input.GetKeyDown(KeyCode.C))
                {
                        //一個でも積まれてれば移動できる
                        if(_Plates[_SelectManager.plateNum].top > -1)
                        {
                            _IsMovingAllFoods = true;
                            _RootPlate = _SelectManager.plateNum;                               
                            _Plates[_SelectManager.plateNum].moveFlag = true;
                        }

                }
                //左Shiftで一番上だけ移動させる
                else if(Input.GetKeyDown(KeyCode.X))
                {
                        //一個でも積まれてれば移動できる
                        if(_Plates[_SelectManager.plateNum].top > -1)
                        {
                            _IsMovingTopFood = true;
                            _RootPlate = _SelectManager.plateNum;                        
                            _Plates[_RootPlate].moveTopFlag = true;
                        } 
                }
                //Fが押されたら注目してる皿に具材を落とす
                else if(Input.GetKeyDown(KeyCode.Z))
                {
                        if(!_Plates[_SelectManager.plateNum].full && !_Plates[_SelectManager.plateNum].realFull)
                        {
                            ThrowFood(this._CurrentFoodNumbers);
                            _CurrentFoodNumbers = _FurtherNextFoodNumbers;
                            _FurtherNextFoodNumbers = _CandidateFoodsToDrop[_fallFoodCounter];
                            //Debug.Log("No." + _fallFoodCounter + ": " + existFood[_fallFoodCounter]);
                            _fallFoodCounter++;
                            //Debug.Log("Max:" + existFood.Length);
                            //Debug.Log("Counter:" + _fallFoodCounter);
                            if(_fallFoodCounter == _CandidateFoodsToDrop.Length)
                            {
                                _fallFoodCounter = 0;
                                //Debug.Log("まずいですよ！！");
                            }
                            UpdateMonitor();                
                        }
                }
                //スペースが押されたらストック関連
                else if(Input.GetKeyDown(KeyCode.Space))
                {
                    //何かストックしてたら
                    if(_StockFlag)
                    {
                        if(!_Plates[_SelectManager.plateNum].full)
                        {
                            ThrowFood(_StockFoodNumbers);
                            _StockFoodNumbers = 0;
                            _StockFlag = false;
                            UpdateMonitor();
                        }
                    }
                    //何もストックしてなかったら
                    else
                    {
                        _StockFoodNumbers = _CurrentFoodNumbers;
                        _CurrentFoodNumbers = _FurtherNextFoodNumbers;
                        _FurtherNextFoodNumbers = _CandidateFoodsToDrop[_fallFoodCounter];
                        _fallFoodCounter++;
                        if(_fallFoodCounter == _CandidateFoodsToDrop.Length) 
                            _fallFoodCounter = 0;
                        _StockFlag = true;
                        UpdateMonitor();
                    }
                }
                else if(Input.GetKeyDown(KeyCode.G) && PlayerPrefs.GetInt("Easy") == 1)
                {
                    //今見てる皿に何か乗ってたら音を鳴らす
                    if(_Plates[_SelectManager.plateNum].top >= 0)
                    {
                        _AudioSource.Stop();
                        _AudioSource.PlayOneShot(_GarbageSound);
                    }
                    _Plates[_SelectManager.plateNum].abandanFlag = true;   
                }
            }            
        }

    
    }

    /// <summary> 注文に応じて落とす具材の配列を設定する </summary>
    /// <param name="CustomerInfo">今いる客の情報</param>
    /// <param name="CustomerNum">今いる客の人数</param>
    /// <returns>配列の長さ</returns>
    void ResetDropFoodArray(CustomerScript [] CustomerInfo, int CustomerNum)
    {
        int size = 0;
        int index = 0;//

        var order_num = 0;
        var orderProduct = RecipeList.Menu[0];

        for (int i = 0; i < _GameManager._CustomerNum; i++)
        {
            order_num = CustomerInfo[i]._OrderNum;
            orderProduct = RecipeList.Menu[order_num];
            size += RecipeList.CalcHeight(orderProduct.recipe);
        }

        //食材の数の配列を作成
        _CandidateFoodsToDrop = new int[size];

        //それぞれの客の注文品の食材番号を取得していく
        for(int i = 0; i < CustomerNum; i++)
        {
            for(int j = 0;j <  orderProduct.recipe.Length; j++)
            {
                order_num = CustomerInfo[i]._OrderNum;
                orderProduct = RecipeList.Menu[order_num];

                if (orderProduct.recipe[j] != 0)
                {
                    //値が0じゃなければ配列に代入してカウント
                    _CandidateFoodsToDrop[index] = orderProduct.recipe[j];
                    index++;
                }
                else
                {
                    break;
                }
            }
        }

        //食材の配列をシャッフル
        _CandidateFoodsToDrop = Shuffle(_CandidateFoodsToDrop);
        return;
    }

    //これから作る配列の長さを決めるメソッド
    /*int SetArraySize(int orderNum)
    {
        int size = 0;
        for(int i = 0; i < 30; i++)
        {
            if(CustomerManager._Menu[orderNum,i] == 0)return size;
            size++;
        }
        return size;
    }*/

    //リセットする
    public void ResetFoodValue()
    {
        ResetDropFoodArray(_CustomerManager.CustomerInfo, _GameManager._CustomerNum);
        //Debug.Log(_foodMax);
        //最初だけ1にする
        
        if(!_HasFinishFirst)
        {
            _CurrentFoodNumbers = _CandidateFoodsToDrop[0];
            _FurtherNextFoodNumbers = _CandidateFoodsToDrop[1];
            _fallFoodCounter = 2;
        }
        else 
        {
            //_foodNum = _nextFoodNum;
            //_nextFoodNum = existFood[0];
            _fallFoodCounter = 0;
        }
        UpdateMonitor();
    }

    //モニターの表示を更新する
    void UpdateMonitor()
    {
        //今落とす食材の番号を渡す
        _Displays[0].Foodnum = _CurrentFoodNumbers;
        //次に控えてる食材の番号を渡す
        _Displays[1].Foodnum = _FurtherNextFoodNumbers;
        //ストックにある食材を渡す
        _Displays[2].Foodnum = _StockFoodNumbers;
        for(int i = 0;i < 3; i++)_Displays[i].CanDisplay = true;
    }

    //食べ物をプレートに投げるメソッド
    void ThrowFood(int Number)
    {
        _Plates[_SelectManager.plateNum].foodNum = Number;
        _Plates[_SelectManager.plateNum].stackFlag = true;
    }

    // 引数として受け取った配列の要素番号を並び替えるメソッド
    int [] Shuffle(int [] array)
    {
        for(int i = 0; i < array.Length - 1; i++)
        {
            //（説明１）現在の要素を預けておく
            int temp = array[i]; 
            //（説明２）入れ替える先をランダムに選ぶ
            int randomIndex = Random.Range(0, array.Length); 
            //（説明３）現在の要素に上書き
            array[i] = array[randomIndex]; 
            //（説明４）入れ替え元に預けておいた要素を与える
            array[randomIndex] = temp; 
        }
        return array;
    }
}