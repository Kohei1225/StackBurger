using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//具材の動きに関するクラス
public class MoveOfFood : MonoBehaviour
{
    GameObject Managers;
    GameSystem GameManager;
    CustomerManager CustomerManager;
    SelectPlate SelectManager;
    AudioSource audioSource;
    MonitorScript [] Displays = new MonitorScript[3];//モニターのスクリプトの配列
    StackScript [] Stack;   //プレートにアタッチされてるスクリプト
    
    public AudioClip GarbageSound;
    public GameObject [] plate;//プレートの配列(Unityで直接参照する)
    public GameObject [] Monitor;//モニターの配列
    public int [] existFood;//落とす具材の配列
    [System.NonSerialized]public bool movedish = false;//食材(全部)のプレート間の移動(浮いてるかどうか)
    [System.NonSerialized]public bool moveTop = false;//食材(一枚)のプレート間の移動(浮いてるかどうか)
    [System.NonSerialized] public int rootPlate;//移動元のプレート番号
    [System.NonSerialized] public bool reset = false;
    
    private int _fallFoodCounter = 0;   //落とした食材の数(配列の番号を指す)
    private int _foodMax = 25;      //落とす予定の食材の数
    private int _foodNum;           //落とす具材の番号
    private int _nextFoodNum;       //次に落とす具材の番号
    private int _stockFoodNum = -1; //ストックしてる具材の番号
    private bool _stockFlag = false;//ストックしてたらtrue
    private bool _first = false;    //最初の処理かどうかの判定

    // Start is called before the _first frame update
    void Start()
    {
        //コンポーネントなどを取得
        Managers = GameObject.Find("Managers");
        GameManager = Managers.GetComponent<GameSystem>();
        CustomerManager = Managers.GetComponent<CustomerManager>();
        SelectManager = GameObject.Find("Select").GetComponent<SelectPlate>();
        audioSource = GetComponent<AudioSource>();

        Stack = new StackScript [plate.Length];
        for(int i = 0; i < plate.Length; i++)
            Stack[i] = plate[i].GetComponent<StackScript>();

        existFood = new int[GameManager._CustomerNum * 25];

        for(int i = 0;i < 3;i++)Displays[i] = Monitor[i].GetComponent<MonitorScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager._IsStart)
        {
            //一番最初だけ
            if(!_first)
            {
                ResetFoodValue(1);
                _first = true;
                _stockFoodNum = 0;
                ChangeMonitor();            
            }
            //リセットする
            if(reset)
            {
                ResetFoodValue(0);
                reset = false;
            }
            //食材が浮いてる間
            if(movedish || moveTop)
            {
                if(Input.GetKeyDown(KeyCode.C) && movedish)
                {
                    //どちらのプレートの量も一定値を超えてなければ落とす
                    if( rootPlate == SelectManager.plateNum || ( SelectManager.plateNum != rootPlate && (!Stack[SelectManager.plateNum].realFull && (Stack[SelectManager.plateNum].top + Stack[rootPlate].top + 2 < 29) ))){
                        movedish = false;

                        //同じプレートじゃなければ登録とかの処理をする
                        if(rootPlate != SelectManager.plateNum)
                        {
                            Stack[SelectManager.plateNum].add = true;                        
                            //Stack[rootPlate].reset = true;
                        }
                            //else Stack[SelectManager.plateNum].depth = SelectManager.Plate[rootPlate].transform.position.z;
                            
                        Stack[rootPlate].moveFlag = false;
                    }
                        
                }
                else if(Input.GetKeyDown(KeyCode.X) && moveTop)
                {
                    //相手のプレートの量が一定値を超えてなければ落とす
                    if( rootPlate == SelectManager.plateNum || ( SelectManager.plateNum != rootPlate && !Stack[SelectManager.plateNum].realFull))
                    {
                        //Debug.Log("左shift");
                                                
                        //同じプレートじゃなければ登録とかの処理をする
                        if(rootPlate != SelectManager.plateNum)
                        {
                            Stack[SelectManager.plateNum].add = true;                        
                            //Stack[rootPlate].reset = true;
                        }
                        else moveTop = false;    
                        //else Stack[SelectManager.plateNum].depth = SelectManager.Plate[rootPlate].transform.position.z;
                        //
                        Stack[rootPlate].moveTopFlag = false;
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
                        if(Stack[SelectManager.plateNum].top > -1)
                        {
                            movedish = true;
                            rootPlate = SelectManager.plateNum;                               
                            Stack[SelectManager.plateNum].moveFlag = true;
                        }

                }
                //左Shiftで一番上だけ移動させる
                else if(Input.GetKeyDown(KeyCode.X))
                {
                        //一個でも積まれてれば移動できる
                        if(Stack[SelectManager.plateNum].top > -1)
                        {
                            moveTop = true;
                            rootPlate = SelectManager.plateNum;                        
                            Stack[rootPlate].moveTopFlag = true;
                        } 
                }
                //Fが押されたら注目してる皿に具材を落とす
                else if(Input.GetKeyDown(KeyCode.Z))
                {
                        if(!Stack[SelectManager.plateNum].full && !Stack[SelectManager.plateNum].realFull)
                        {
                            ThrowFood(this._foodNum);
                            _foodNum = _nextFoodNum;
                            _nextFoodNum = existFood[_fallFoodCounter];
                            //Debug.Log("No." + _fallFoodCounter + ": " + existFood[_fallFoodCounter]);
                            _fallFoodCounter++;
                            //Debug.Log("Max:" + existFood.Length);
                            //Debug.Log("Counter:" + _fallFoodCounter);
                            if(_fallFoodCounter == existFood.Length)
                            {
                                _fallFoodCounter = 0;
                                //Debug.Log("まずいですよ！！");
                            }
                            ChangeMonitor();                
                        }
                }
                //スペースが押されたらストック関連
                else if(Input.GetKeyDown(KeyCode.Space))
                {
                    //何かストックしてたら
                    if(_stockFlag)
                    {
                        if(!Stack[SelectManager.plateNum].full)
                        {
                            ThrowFood(_stockFoodNum);
                            _stockFoodNum = 0;
                            _stockFlag = false;
                            ChangeMonitor();
                        }
                    }
                    //何もストックしてなかったら
                    else
                    {
                        _stockFoodNum = _foodNum;
                        _foodNum = _nextFoodNum;
                        _nextFoodNum = existFood[_fallFoodCounter];
                        _fallFoodCounter++;
                        if(_fallFoodCounter == _foodMax) 
                            _fallFoodCounter = 0;
                        _stockFlag = true;
                        ChangeMonitor();
                    }
                }
                else if(Input.GetKeyDown(KeyCode.G) && PlayerPrefs.GetInt("Easy") == 1)
                {
                    //今見てる皿に何か乗ってたら音を鳴らす
                    if(Stack[SelectManager.plateNum].top >= 0)
                    {
                        audioSource.Stop();
                        audioSource.PlayOneShot(GarbageSound);
                    }
                    Stack[SelectManager.plateNum].abandanFlag = true;   
                }
            }            
        }

    
    }

    /// <summary> 注文に応じて落とす具材の配列を設定する </summary>
    /// <param name="CustomerInfo">今いる客の情報</param>
    /// <param name="CustomerNum">今いる客の人数</param>
    /// <returns>配列の長さ</returns>
    int SelectFood(CustomerScript [] CustomerInfo, int CustomerNum)
    {
        int size = 0;
        int count = 0;//

        var orderNum = 0;
        var orderProduct = RecipeList.Menu[0];

        for (int i = 0; i < GameManager._CustomerNum; i++)
        {
            orderNum = CustomerInfo[i]._OrderNum;
            orderProduct = RecipeList.Menu[orderNum];
            size += RecipeList.CalcHeight(orderProduct.recipe);
        }

        //食材の数の配列を作成
        existFood = new int[size];

        for(int i = 0; i < CustomerNum; i++)
        {
            for(int j = 0;j <  orderProduct.recipe.Length; j++)
            {
                orderNum = CustomerInfo[i]._OrderNum;
                orderProduct = RecipeList.Menu[orderNum];

                if (orderProduct.recipe[j] != 0)
                {
                    //値が0じゃなければ配列に代入してカウント
                    existFood[count] = orderProduct.recipe[j];
                    count++;
                }
                else
                {
                    break;
                }
            }
        }
        //食材の配列をシャッフル
        existFood = Shuffle(existFood);
        return count;
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
    void ResetFoodValue(int N)
    {
        _foodMax = SelectFood(CustomerManager.CustomerInfo, GameManager._CustomerNum);
        //Debug.Log(_foodMax);
        //最初だけ1にする
        if(N == 1)
        {
            _foodNum = existFood[0];
            _nextFoodNum = existFood[1];
            _fallFoodCounter = 2;
        }
        else 
        {
            //_foodNum = _nextFoodNum;
            //_nextFoodNum = existFood[0];
            _fallFoodCounter = 0;
        }
        ChangeMonitor();
    }

    //モニターの表示を更新する
    void ChangeMonitor()
    {
        //今落とす食材の番号を渡す
        Displays[0].Foodnum = _foodNum;
        //次に控えてる食材の番号を渡す
        Displays[1].Foodnum = _nextFoodNum;
        //ストックにある食材を渡す
        Displays[2].Foodnum = _stockFoodNum;
        for(int i = 0;i < 3; i++)Displays[i].CanDisplay = true;
    }

    //食べ物をプレートに投げるメソッド
    void ThrowFood(int Number)
    {
        Stack[SelectManager.plateNum].foodNum = Number;
        Stack[SelectManager.plateNum].stackFlag = true;
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