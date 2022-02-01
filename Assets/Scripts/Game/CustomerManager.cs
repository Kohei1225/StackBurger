using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 客をまとめて管理するクラス </summary>
public class CustomerManager : MonoBehaviour
{
    private GameObject _Managers;
    private GameSystem _GameManager;
    private MoveOfFood _ThrowManager;
    [SerializeField]private CustomerTimeLimitBar[] _TimeLimitBars = new CustomerTimeLimitBar[3];
    [SerializeField] AnimationCurve _WaitTimeGlaph;

    /// <summary> お客さんの配列(全部) </summary>
    [SerializeField] private GameObject[] _AllCustomers;
    /// <summary> 今いるお客さんの配列 </summary>
    private GameObject[] _CurrnetCustomers;
    /// <summary> お客さんの情報の配列 </summary>
    private CustomerScript[] _CustomerInfo;
    /// <summary> お客さんの感情マークの配列 </summary>
    [SerializeField]private GameObject[] _CustomerEmotions;
    /// <summary> 客と感情マークの座標の差分 </summary>
    private Vector2[] _EmotionPosDiff =
    {
        new Vector2(-6,4),
        new Vector2(-3,4),
        new Vector2(-4,2),
        new Vector2(-4,2),
        new Vector2(-3,5),
    };

    private System.ValueTuple<GameObject, Vector2> [] _EmotionsInfo = new (GameObject, Vector2)[5];

    /// <summary> お客さんの上にあるスライダーの配列 </summary>
    public GameObject [] _CustomersSliders;

    /// <summary> お客さんを補充する時の判定 </summary>
    public bool _IsLeaveCustomer;
    /// <summary> 一番最初の処理をしたかどうか </summary>
    public bool HasFinishFirst{get; private set;} = true;
    /// <summary> 登場できるお客さんの数 </summary>
    private int _CustomerMax;

    private bool _HasCallFirstCustomer = false;

    public GameObject[] CurrentCustomers
    {
        get { return _CurrnetCustomers; }
    }
    public CustomerScript[] CustomerInfo
    {
        get { return _CustomerInfo; }
    }

    public System.ValueTuple<GameObject,Vector2>[] EmotionInfo
    {
        get { return _EmotionsInfo; }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeEmotionArray();
        _Managers = GameObject.Find("Managers");
        _GameManager = _Managers.GetComponent<GameSystem>();
        _ThrowManager = _Managers.GetComponent<MoveOfFood>();
        _IsLeaveCustomer = false;
        _CustomerMax = _AllCustomers.Length - 1;
        if(PlayerPrefs.GetInt("day") == 6)_CustomerMax = _AllCustomers.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームが始まるまでは何もしない
        if(_GameManager._IsStart)
        {
            //ゲームが始まって一番最初の処理
            if(HasFinishFirst)
            {
                _CurrnetCustomers = new GameObject[_GameManager._CustomerNum];
                _CustomerInfo = new CustomerScript[_GameManager._CustomerNum];
                for(int i = 0; i < _GameManager._CustomerNum; i++)
                {
                    _TimeLimitBars[i] = _CustomersSliders[i].GetComponent<CustomerTimeLimitBar>();
                    CallCustomer(i);
                }
                HasFinishFirst = false;  
            }

            //お客さんが消えたら
            if(_IsLeaveCustomer)
            {
                for(int i = 0;i < _GameManager._CustomerNum; i++)
                {
                    //客が帰るサインを出してたら
                    if(CustomerInfo[i].sign)
                    {
                        //客を消す
                        //CustomerInfo[i].flag = true;
                        Destroy(CustomerInfo[i].gameObject);
                        //新しい客を作る
                        CallCustomer(i);
                        //メニューを投げる食材を更新してもらう
                    }
                }
                _IsLeaveCustomer = false;
            }
        }
    }

    /// <summary> 定員に足りなくなった時に客を呼ぶ </summary>
    /// <param name="Num">いなくなった場所の番号</param>
    void CallCustomer(int Num)
    {
        var customer = _AllCustomers[Random.Range(0, _CustomerMax)];
        var pos = new Vector3(_CustomersSliders[Num].transform.position.x, -15, 15);

        _CurrnetCustomers[Num] = Instantiate( customer, pos, Quaternion.identity);
        CustomerInfo[Num] = _CurrnetCustomers[Num].GetComponent<CustomerScript>();
        CustomerInfo[Num].TimeLimitBar = _TimeLimitBars[Num];
        Debug.Log(CustomerInfo[Num].TimeLimitBar);
        if(!_HasCallFirstCustomer)
        {
            var product_type = RecipeList.ProductType.BURGER;
            switch(_GameManager.CurrentDay)
            {
                case 3:
                    product_type = RecipeList.ProductType.BURGER;
                    break;
                case 4:
                    product_type = RecipeList.ProductType.SANDWITCH;
                    break;
                case 5:
                    product_type = RecipeList.ProductType.MUFFIN;
                    break;
                case 6:
                    product_type = RecipeList.ProductType.BIG;
                    break; 
                case 7:
                    product_type = RecipeList.ProductType.DISH;
                    break;
                default:
                    CustomerInfo[Num].OrderProduct();
                    break;
            }
            CustomerInfo[Num].OrderProduct(product_type);
            _HasCallFirstCustomer = true;
            return;
        }
        CustomerInfo[Num].OrderProduct();
    }

    void InitializeEmotionArray()
    {
        for(int i = 0; i < _CustomerEmotions.Length; i++)
        {
            var pairs = (_CustomerEmotions[i], _EmotionPosDiff[i]);
            _EmotionsInfo[i] = pairs;
        }
    }

    public float GetWaitTime(int height)
    {
        return _WaitTimeGlaph.Evaluate(height);
    }
}
