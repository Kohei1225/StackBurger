using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 上に食材を積むクラス. 各プレートにアタッチする </summary>
public class PlateScript : MonoBehaviour
{
    const int SIZE = 30;
    private int max = 10;//１つずつ積める最大値(これを超えたらプレート間でしか積めない)
    private int realmax = 25;//その皿に積める最大値(これを超えたら(>)積めない)
    public int[] dish;//{get; private set;}          //積んだ具材(0で何もなし)
    public int top{get; private set;}               //自分の所に積んである具材の数

    public GameObject[] stackedfood;//積んだ具材の配列
    public bool stackFlag;          //積む合図
    public bool abandanFlag = false;//捨てる合図
    public bool moveFlag = false;   //食材を移動する合図(別のスクリプトから変更する)
    public bool moveTopFlag = false;//一番上だけ食材を移動する合図
    public bool full = false;       //いっぱいのサイン。個別で積めなくなるので他の皿の物を乗せるしかない
    public bool realFull = false;   //マジでいっぱいのサイン。ここまで積むと何も乗せられない
    public bool isPlate = false;
    public bool add = false;        //他のプレートから食材を受け入れるメソッド
    public bool reset = false;      //リセットする合図
    float [] heightList{get;} = {        //各食材の縦の長さ
        2f, 2f, 2f, 1.5f, 0.5f, 0.5f, 1f, 3.43f, 0.5f, 1.5f, 1.5f, 1f, 1.5f, 0.5f, 2f, 0.3f, 1f, 0.3f, 0.3f, 0.3f, 2.5f
    };
    public float height = 0;        //オブジェクトのリアルな高さ
    public float dropHeight = 10;   //物を落とすときの位置

    SelectCustomer SelectCustomer;
    GameObject Managers;
    CustomerManager _CustomerScript;
    SelectPlate SelectManager;
    MoveOfFood _MoveOfFoodManager;
    GameSystem GameManager;
    Text weightText;

    public float depth;//食材の深さ(z座標のこと)
    public int foodNum;//食材の種類
    float xpos;//X座標
    public int plateNumber = 0;//プレートの番号

    // Start is called before the first frame update
    void Start()
    {
        SelectCustomer = GameObject.Find("CustomerPlate").GetComponent<SelectCustomer>();
        Managers = GameObject.Find("Managers");
        GameManager = Managers.GetComponent<GameSystem>();
        _CustomerScript = Managers.GetComponent<CustomerManager>();
        SelectManager = GameObject.Find("Select").GetComponent<SelectPlate>();
        _MoveOfFoodManager = Managers.GetComponent<MoveOfFood>();

        dish = new int[SIZE];
        stackedfood = new GameObject[SIZE];

        top = -1;
        foodNum = 0;

        //初期化
        abandanFlag = false;
        moveFlag = false;
        full = false;
        realFull = false;
        isPlate = false;
        add = false;
        
        for(int i = 0;i < dish.Length;i++)dish[i] = 0;
        stackFlag = false;
        depth = gameObject.transform.position.z;
        xpos = gameObject.transform.position.x;
        //Debug.Log(foods.Length);

        if(gameObject.name == "Plate")
        {
            isPlate = true;
            plateNumber = 0;
            //本皿だったら15個まで普通に積める
            max = 10;
            weightText = GameObject.Find("PlateCounter").GetComponent<Text>();
        }
        //それ以外の皿だったら7まで普通に積める
        else 
        {
            max = 7;
            if(gameObject.name == "Plate1")plateNumber = 1;
            else if(gameObject.name == "Plate2")plateNumber = 2;
            else if(gameObject.name == "Plate3")plateNumber = 3;
            weightText = GameObject.Find("Plate" + plateNumber + "Counter").GetComponent<Text>();
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        //皿の溜まり具合を確認する
        if(top >= max - 1)full = true;
        else full = false;
        if(top >= realmax - 1)realFull = true;
        else realFull = false;

        //何枚乗ってるかを表示する
        weightText.text = (top + 1).ToString();
        if(realFull)weightText.color = new Color(203f/255, 0, 0);
        else if(full)
        {
            weightText.color = new Color(1, 233f / 255, 0);//212,202,9 //191,182,9
            //Debug.Log("full");
        }
        else weightText.color = new Color(0,0,0);

        //自分の皿に積む時の落とす高さを確認する
        dropHeight = height + 2.5f + transform.position.y;
        //一個でも積まれてれば
        if(top > -1 && stackedfood[top])
        {
            dropHeight = stackedfood[top].transform.position.y + 2.5f;
        }
        else dropHeight = -4.5f;

        //他のプレートの食材を登録する
        if(add)
        {
            Debug.Log("RootPlate:" + SelectManager.Plate[_MoveOfFoodManager._RootPlate].name);
            //一枚しか移動しない時
            if(_MoveOfFoodManager._IsMovingTopFood)
            {
                top++;
                Debug.Log("top: " + top);
                //コピーする
                dish[top] = SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().dish[ SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().top ];
                stackedfood[top] = SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().stackedfood[ SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().top ];
                depth -= 0.1f;
                height += heightList[SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().dish[ SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().top] - 1];

                //高さの設定をする
                SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().depth += 0.1f;
                SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().height -= heightList[ SelectManager.Plate[ _MoveOfFoodManager._RootPlate ].GetComponent<PlateScript>().dish[ SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().top] ];

                //コピー元のトップを消す
                SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().dish[ SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().top ] = 0;
                SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().stackedfood[ SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().top ] = null;                

                SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().top--;
            }
            else AddFood(SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().dish, SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().stackedfood);
            _MoveOfFoodManager._IsMovingTopFood = false;   
            add = false;
        }
        if(reset)
        {
            ResetPlate();
            reset = false;
        }
        //皿に乗ってる食材を移動させる
        if(moveFlag)
        {
            
            for(int i = 0; i < top + 1; i++)
            {
                stackedfood[top].GetComponent<FoodScript>()._TopMove = true;
                //dish[i] = 0;
                //Destroy(stackedfood[i]);
            }

            
            //moveFlag = false;
        }

        if(moveTopFlag)
        {
            //Debug.Log("top: " + top);
            stackedfood[top].GetComponent<FoodScript>()._TopMove = true;
        }

        //指定された皿だったら食材を落とす
        if(stackFlag)
        {
            if(!full && !realFull)
            {
                StackFood(foodNum);
            }
            stackFlag = false;
        }

        //Gキーで乗ってる食材を捨てる(判定は他のスクリプト)
        if(abandanFlag){
            //if(top > -1)Debug.Log("無くしたよ");
            for(int i = 0; i < top + 1; i++)Destroy(stackedfood[i]);
            ResetPlate();
            abandanFlag = false;
        }

        //Enterで客に提供する
        if(GameManager._IsStart && Input.GetKeyDown(KeyCode.Return) && isPlate && !_MoveOfFoodManager._IsMovingAllFoods && _CustomerScript.CustomerInfo[SelectCustomer._CustomerNum].CanReceiveFood)
        {
            if (dish[0] != 0)
            {
                _CustomerScript.CustomerInfo[SelectCustomer._CustomerNum].buy = true;
                _CustomerScript.CustomerInfo[SelectCustomer._CustomerNum].CheckReceivedProduct(dish);
            }
            abandanFlag = true;
        }
    }

    //指定された番号の具材を積むメソッド
    void StackFood(int Num)
    {
        depth -= 0.1f;
        //if(top > -1 && stackedfood[top].transform.position.y > dropHeight)stackedfood[top + 1] = Instantiate(foods[Num - 1],new Vector3(gameObject.transform.position.x,stackedfood[top].transform.position.y + 5 ,depth), Quaternion.identity);
        stackedfood[top + 1] = Instantiate(GameManager.FoodPrefabs[Num - 1],new Vector3(gameObject.transform.position.x, dropHeight, depth), Quaternion.identity);
        dish[top + 1] = Num;
        height += heightList[Num - 1];
        top++;
    }

    //他のプレートから来た食材を登録するメソッド
                //数字の配列,オブジェクトの配列
    void AddFood(int [] newFoodNum,GameObject [] newFoods)
    {
        int tmp = top;
        //Debug.Log("Length:" + newFoodNum.Length);
        for(int i = 0; i < newFoodNum.Length; i++)
        {
            //具材なしになったら
            //Debug.Log("value" + i + ":" + newFoodNum[i]);
            if(newFoodNum[i] == 0)
            {
                top = tmp;
                //Debug.Log("Depth:" + depth);
                //Debug.Log(gameObject.name + "で消します");
                
                SelectManager.Plate[_MoveOfFoodManager._RootPlate].GetComponent<PlateScript>().reset = true;
                return;
            }
            dish[i + top + 1] = newFoodNum[i];
            stackedfood[i + top + 1] = newFoods[i];
            tmp++;
            height += heightList[newFoodNum[i] - 1];
            depth -= 0.1f;
            //Debug.Log("Depth" + i + ":" + depth);
        }
    }

    //プレートの食材をリセットするメソッド
    void ResetPlate()
    {
        for(int i = 0; i < top + 1; i++)
        {
            dish[i] = 0;
        }
        //Debug.Log(gameObject.name + "で消したぞ！！");
        stackedfood = new GameObject[SIZE];
        top = -1;
        depth = transform.position.z;
        height = 0;
    }
}
