using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//客にアタッチするスクリプト
public class CustomerScript : MonoBehaviour
{
    GameObject _Managers;        //大事なスクリプトが入ってるオブジェクト
    CustomerManager _CustomerManager;
    GameObject _Plate;           //提供用プレート
    StackScript _PlateInfo;      //プレートの情報
    GameSystem _GameManager;

    public int _OrderNum{get;private set;} = 0;    //オーダー番号
    public bool sign{get; private set;} = false;   //帰るサイン。動いた後にこっちでオンにする
    public bool take{get; private set;} = false;   //動くきっかけになるサイン。
    public int happyLevel{get; private set;} = 0;      //満足度(1以上で一応受け取る)

    [System.NonSerialized] public bool flag = false;    //消える判定。オンにされたら消える
    [System.NonSerialized] public bool buy = false;     //提供されて確認する判定。
    [System.NonSerialized] public bool _HasVisit = false;   //来店したかどうかの判定。
    [System.NonSerialized] public bool angry = false;   //怒って帰る時の判定(時間切れ、3回間違える)
    [System.NonSerialized] public bool happy = false;   //商品を受け取って帰る時の判定(パーフェクト)
    [System.NonSerialized] public bool strange = false; //一応商品を受け取って帰る時の判定(数カ所間違いあり)
    [System.NonSerialized] public bool bad = false;     //間違いが多すぎて受け取らない判定
    [System.NonSerialized] public bool exclamation = false;//食材の順番だけ違う判定
    [System.NonSerialized] public float chip = 5;             //払うチップ額
    [System.NonSerialized] public bool timeOver = false;   //時間切れ判定
    [System.NonSerialized] public int chipLevel = 1;      //払うチップの程度(経過時間によって変わる)
    public bool _HasRecieve = false;
    float speed = 30f;

    private int _falseNum = 0;               //間違ってる数
    private int _missTime = 0;               //ミスした回数
    private float _EmotionXPos;          //感情マークのX座標 
    private int _EmotionCounter = 0;     //感情マークの表示時間経過
    private bool _EmotionActive = false; //感情マークが有効かの判定   
    private int _height = 0;             //自分が注文したメニューの段数    
    private bool _chipFlag = false;      //チップを払う時の判定
    private int _StopCounter = 0;                //商品を渡してからお客さんが動き出すまでの時間経過
    private bool _HasOrder = false;

    [SerializeField]private float _tall;              //身長 1:低い　2:普通　3:高い
    [SerializeField]private GameObject _EmotionMark;  //感情を表すマークのオブジェクト

    const int EMOTIONRIMIT = 100;       //感情マークの表示時間

    TimerScript _CustomerMainTimer = new TimerScript();
    TimerScript _BeforeLeaveStoreTimer = new TimerScript(1);

    public bool CanReceiveFood
    {
        get { return _HasVisit && !_HasRecieve; }
    }

    public TimerScript MainTimer
    {
        get { return _CustomerMainTimer; }
    }

    void Awake()
    {
        //Debug.Log("pos(" + transform.position + ")");
        _Managers = GameObject.Find("Managers");
        _CustomerManager = _Managers.GetComponent<CustomerManager>();
        _GameManager = _Managers.GetComponent<GameSystem>();
        _Plate = GameObject.Find("Plate");
        _PlateInfo = _Plate.GetComponent<StackScript>();
        //注文する商品を決める
        //Debug.Log("決めました。");

    }

    // Start is called before the first frame update
    void Start()
    {
        _height = RecipeList.CalcHeight(RecipeList.Menu[_OrderNum].recipe);
        chip = RecipeList.Menu[_OrderNum].value;
        if( _height >= 15)
        {
            chipLevel = 15;
            chip = 60;
        }
        else if(_height >= 7)
        {
            chipLevel = 5;
            chip = 30;
        }
        else 
        {
            chipLevel = 2;
            chip = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_GameManager._IsStart) return;

        if(_HasRecieve)
        {
            _BeforeLeaveStoreTimer.UpdateTimer();
            if(_BeforeLeaveStoreTimer.IsTimeUp)
            {
                LeaveStore();
            }
            return;
        }

        if(!_HasVisit)
        {
            ComeStore();
            return;
        }

        _CustomerMainTimer.UpdateTimer();
        if(_CustomerMainTimer.IsTimeUp)
        {
            _HasRecieve = true;
            CreateEmotion(4);
            if (_GameManager._IsStart) _GameManager.AngryNum++;
            return;
        }

    }

    IEnumerator UpdateCoroutine()
    {
        while(!_HasRecieve)
        {
            //if (!_GameManager._IsStart) yield return null;

            _CustomerMainTimer.UpdateTimer();
            if(_CustomerMainTimer.IsTimeUp)
            {
                _HasRecieve = true;
                CreateEmotion(4);
                _GameManager.AngryNum++;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine("LeaveShop");
    }

    /// <summary> 商品を受け取って確認 </summary>
    /// <param name="stackedFoods">  </param>
    public void CheckReceivedProduct(int [] stackedFoods)
    {
        for(int i = 0;i < stackedFoods.Length;i++)
        {
            //Debug.Log("stacked["+i+"]" + stackedFoods[i]);
        }
        int evaluate = EvaluateProduct(stackedFoods);
        

        Vector3 pos = transform.position;
        switch(evaluate)
        {
            case 0:
                //完璧の時(音符マーク)
                happy = true;
                _StopCounter = 0;
                if (_GameManager._IsStart) _GameManager.HappyNum++;
                break;
            case 1:
                //中身の順番が違う時(！マーク)
                exclamation = true;
                _StopCounter = 0;
                if (_GameManager._IsStart) _GameManager.ExclamationNum++;
                chip = (int)(chip * 0.75);
                break;
            case 2:
                //若干違う時(？マーク)
                strange = true;
                _StopCounter = 0;
                if (_GameManager._IsStart) _GameManager.StrangeNum++;
                chip /= 2;
                break;
            default:
                //普通に違う時(もやもやマーク)
                _falseNum++;
                _EmotionCounter = 0;
                _GameManager.BadNum++;
                _EmotionActive = true;
                bad = true;
                _GameManager.PlayMissSound();
                if (_falseNum >= 3)
                {
                    evaluate = 4;
                    angry = true;
                    _StopCounter = 0;
                    _GameManager.AngryNum++;
                    _HasRecieve = true;
                }
                break;
        }
        CreateEmotion(evaluate);
        if(evaluate < 3)
        {
            Buy();
        }
    }
     
    void Buy()
    {
        _HasRecieve = true;
        this.chip = CalcChip();
        _GameManager.Chip += chip;
        GameObject chip_text = Instantiate(_GameManager.ChipText);
        chip_text.transform.parent = GameObject.Find("Canvas").transform;
        chip_text.GetComponent<Text>().text = "$" + (chip.ToString("N2"));

    }

    float CalcChip()
    {
        var product = RecipeList.Menu[_OrderNum];
        float product_value = product.value;
        float product_height = RecipeList.CalcHeight(product.recipe);
        Debug.Log("value:" + product_value + "\nheight:" + product_height + "\nrate:" + _CustomerMainTimer.RateOfTime);
        float chip = product_value + (product_value + product_height) * _CustomerMainTimer.RateOfTime;

        return chip ;
    }

    IEnumerator ComeShop()
    {
        Vector3 target_pos = transform.position;
        target_pos.y = _tall + 6;
        Vector3 vec = target_pos - transform.position;

        while (vec.magnitude > 0.25f)
        {
            transform.position = vec.normalized * speed * Time.deltaTime;
            vec = target_pos - transform.position;
            yield return null;
        }

        transform.position = target_pos;
        _HasVisit = true;
    }

    void ComeStore()
    {
        //Debug.Log("ComeStore()");
        Vector3 pos = transform.position;
        pos.y = _tall + 6;

        if (MoveTo(pos, speed))
        {
            _HasVisit = true;
        }
        
    }

    IEnumerator LeaveShop()
    {
        Vector3 target_pos = transform.position;
        target_pos.y = -15;
        Vector3 vec = target_pos - transform.position;

        while (vec.magnitude > 0.25f)
        {
            transform.position = vec.normalized * speed * Time.deltaTime;
            vec = target_pos - transform.position;
            yield return null;
        }

        sign = true;
        _CustomerManager._IsLeaveCustomer = true;
    }

    void LeaveStore()
    {
        Vector3 pos = transform.position;
        pos.y = -15;

        if (MoveTo(pos, speed))
        {

            sign = true;
            _CustomerManager._IsLeaveCustomer = true;
        }
    }

    void CreateEmotion(int emotionNum)
    {
        if (_EmotionMark)
        {
            Destroy(_EmotionMark);
        }

        var emotion_info = _CustomerManager.EmotionInfo[emotionNum];
        GameObject emotion_mark = emotion_info.Item1;
        Vector3 pos = transform.position;
        pos.x += emotion_info.Item2.x;
        pos.y += emotion_info.Item2.y;

        _EmotionMark = Instantiate(emotion_mark, pos, Quaternion.identity);
        _EmotionMark.transform.parent = transform;
    }

    bool MoveTo(Vector3 targetPos,float speed)
    {
        Vector3 vec = targetPos - transform.position;
        if(vec.magnitude > 0.25f)
        {
            transform.position += vec.normalized*speed*Time.deltaTime;
            return false;
        }

        transform.position = targetPos;
        return true;
    }

    /// <summary> オーダーと商品を比べて評価を返す </summary>
    /// <param name="stackedFoods">積んだ具材</param>
    /// <returns> ミスの具合を返す(0:完璧 1:順番だけ違う 2:具材が違う 3:ダメ)</returns>
    int EvaluateProduct(int [] stackedFoods)
    {
        int perfect = 0, almost_same = 1, little_different = 2, bad = 3;


        int[] true_recipe = RecipeList.Menu[_OrderNum].recipe;
        var recipe_height = RecipeList.CalcHeight(true_recipe);//正解の高さ
        var stacked_height = RecipeList.CalcHeight(stackedFoods);//積んだ食材の高さ

        //var heightDiff = Mathf.Abs(recipe_height - stacked_height);//高さの差
        var is_sandwitched = IsSandwitched(stackedFoods);
        var false_counter = CountMiss(stackedFoods);
        var is_perfect = IsPerfect(stackedFoods);

        //もし完璧ならもう終わり(合格)
        if(is_perfect)
        {
            return perfect;
        }

        //商品のジャンルによって判定基準が変わる
        switch (RecipeList.Menu[_OrderNum].type)
        {
            case RecipeList.ProductType.BURGER:
            {
                //バンズで挟んでなければアウト
                if(!is_sandwitched)
                {
                    return bad;
                }
                // 中身が合ってればセーフ
                if(false_counter == 0)
                {
                    return almost_same;
                }
                break;
            }

            case RecipeList.ProductType.SANDWITCH:
            {
                //パンで挟んでなければアウト
                if(!is_sandwitched)
                {
                    return bad;
                }
                //入ってる具材が合ってればセーフ
                if(false_counter == 0)
                {
                    return perfect;
                }
                break;
            }
            case RecipeList.ProductType.MUFFIN:
            {
                //マフィンで挟んでなければアウト
                if(!is_sandwitched)
                {
                    return bad;
                }
                //
                if(false_counter == 0)
                {
                    return almost_same;
                }
                break;
            }
            case RecipeList.ProductType.DISH:
            {
                //
                if(false_counter == 0)
                {
                    return almost_same;
                }
                return bad;
            }
            case RecipeList.ProductType.PANCAKE:
            {
                //ホットケーキなら一番上がバターじゃなければアウト
                var topOfStackedFoods = stackedFoods[stacked_height - 1];
                var butter = true_recipe[recipe_height - 1];

                if (topOfStackedFoods != butter)
                {
                    return bad;
                }

                //中身が一緒なら合格
                if(false_counter == 0)
                {
                    return almost_same;
                }
                return bad;
            }
            case RecipeList.ProductType.SALAD:
            {
                //中身が一緒だったら合格
                if(false_counter == 0)
                {
                    return perfect;
                }
                break;
            }
            case RecipeList.ProductType.RANDOM:
            {
                return perfect;
            }
        }

        int max_miss_border_line = recipe_height / 6;
         
        //
        if(false_counter <= max_miss_border_line + 1)
        {
            return little_different;
        }

        return bad;

    }

    public void OrderProduct()
    {
        if (_HasOrder) return;

        _OrderNum = Random.Range(0, _GameManager._Range);
        var recipe = RecipeList.Menu[_OrderNum].recipe;
        _CustomerMainTimer.ResetTimer(_CustomerManager.GetWaitTime(RecipeList.CalcHeight(recipe)));
        _HasOrder = true;
    }

    public void OrderProduct(RecipeList.ProductType type)
    {
        if (_HasOrder) return;

        int min = 0, max = _GameManager._Range;
        switch(type)
        {
            case RecipeList.ProductType.BURGER:
                min = 0;
                max = 29;
                break;
            case RecipeList.ProductType.SANDWITCH:
                min = 29;
                max = 34;
                break;
            case RecipeList.ProductType.MUFFIN:
                min = 34;
                max = 39;
                break;
            case RecipeList.ProductType.BIG:
                min = 39;
                max = 46;
                break;
            case RecipeList.ProductType.DISH:
                min = 46;
                max = 59;
                break;
        }
        _OrderNum = Random.Range(min, max);
        var recipe = RecipeList.Menu[_OrderNum].recipe;
        _CustomerMainTimer.ResetTimer(_CustomerManager.GetWaitTime(RecipeList.CalcHeight(recipe)));
        _HasOrder = true;
        return;
    }


    //中身を昇順にした新しい配列を作って返すメソッド(一次配列)
    int [] SortArray(int [] sourceArray, int length)
    {
        int [] array = new int[length];
        //代入する
        for (int i = 0; i < length; i++)
        {
            array[i] = sourceArray[i];
        }
        return Sort(array);
    }

    //実際にソートするメソッド
    int [] Sort(int [] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            for(int j = i + 1; j < array.Length; j++)
            {
                if(array[i] > array[j])
                {
                    int tmp = array[i];
                    array[i] = array[j];
                    array[j] = tmp;
                }
            }
        }
        //for(int i = 0; i < array.Length; i++)Debug.Log(array[i]);
        //Debug.Log("ーーーー");
        return array;
    }

    /// <summary> 特定の食材で挟んでいるか </summary>
    /// <param name="stackedFoods">作った料理</param>
    /// <returns>挟めているかどうか</returns>
    bool IsSandwitched(int[] stackedFoods)
    {
        int[] recipe = RecipeList.Menu[_OrderNum].recipe;
        var stackedHeight = RecipeList.CalcHeight(stackedFoods);
        var recipeHeight = RecipeList.CalcHeight(recipe);

        if (stackedFoods[0] != recipe[0] ||
            stackedFoods[stackedHeight - 1] != recipe[recipeHeight - 1])
        {
            return false;
        }

        return true;
    }

    /// <summary>  </summary>
    /// <param name="stackedFoods"></param>
    /// <returns>レシピとの各食材の差分</returns>
    int CountMiss(int [] stackedFoods)
    {
        var falseCount = 0;
        var recipe = RecipeList.Menu[_OrderNum].recipe;
        var recipeHeight = RecipeList.CalcHeight(recipe);
        var stackedHeight = RecipeList.CalcHeight(stackedFoods);

        var foodsOfRecipe = MakeNumberOfFoodArray(recipe);
        var foodsOfStacked = MakeNumberOfFoodArray(stackedFoods);

        int missCounter = 0;
        for(int i = 0;i < 20; i++)
        {
            missCounter += Mathf.Abs(foodsOfRecipe[i] - foodsOfStacked[i]);
        }

        Debug.Log("miss:" + missCounter);
        return missCounter;
    }

    /// <summary> それぞれの食材の合計を記録した配列を作成 </summary>
    /// <param name="sourceArray">元の配列</param>
    /// <returns>各値の合計を記録した配列</returns>
    int[] MakeNumberOfFoodArray(int []sourceArray)
    {
        //食材の種類の数の大きさの配列を作成
        int[] array = new int[20];

        InitializeArray(array, 0);

        //配列の中身をカウント
        for (int i = 0; i < sourceArray.Length;i++)
        {
            var number = sourceArray[i] - 1;
            if (number < 0) break;
            array[number]++;
        }
        return array;
    }

    void InitializeArray(int[] array,int num)
    {
        for(int i = 0; i < array.Length;i++)
        {
            array[i] = num;
        }
    }

    /// <summary> 完璧かどうか </summary>
    /// <param name="stackedFoods">作った料理</param>
    /// <returns>完璧か</returns>
    bool IsPerfect(int[] stackedFoods)
    {
        var recipe = RecipeList.Menu[_OrderNum].recipe;
        var stackedHeight = RecipeList.CalcHeight(stackedFoods);
        var recipeHeight = RecipeList.CalcHeight(recipe);

        var limit = (stackedHeight >= recipeHeight) ? stackedHeight : recipeHeight;

        for(int i = 0;i < limit;i++)
        {
            if(stackedFoods[i] != recipe[i])
            {
                return false;
            }
        }
        return true;
    }
}
