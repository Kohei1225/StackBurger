using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> ゲーム中にレシピを表示するクラス </summary>
public class RecipeUIScript : MonoBehaviour
{
    /// <summary> お手本の画像 </summary>
    [SerializeField] private Sprite[] _Recipe;
    /// <summary> 何もない画像 </summary>
    [SerializeField] private Sprite _Empty;
    /// <summary> 商品の名前 </summary>
    string[] _RecipeNames = {
        //1~5(ハンバーガー)
        "格安バーガー","バーガー","ハンバーガー","ナイスバーガー","リッチバーガー",
        //6~10(チーズバーガー)
        "格安\nチーズバーガー","安い\nチーズバーガー","チーズバーガー","ナイス\nチーズバーガー","リッチ\nチーズバーガー",
        "ベーコン\nチーズバーガー","ダブル\nチーズバーガー","トリプル\nチーズバーガー",
        //14~15(カツバーガー)
        "安い\nカツバーガー","カツバーガー",
        "フィッシュ\nバーガー",
        //17~18(てりやき)
        "安い\nてりやき\nバーガー","てりやき\nバーガー",
        //19~ (てりたま)
        "たまてり\nバーガー","チーズ\nたまてり\nバーガー","とんかつ\nたまてり\nバーガー","ベーコン\nたまてり\nバーガー",
        //23~(スタック)
        "スタック\nバーガー","スタック\nチーズバーガー",
        //25~(月見)
        "お月見バーガー","チーズ\nお月見バーガー",
        //27~(Big)
        "ビッグバーガー","メガバーガー","ペタバーガー","ヨタバーガー","全部乗せ\nバーガー",
        //30~(その他)
        "アボカド\nバーガー","ベーコンレタス\nバーガー","野菜バーガー","フレッシュ\nバーガー","ミートバーガー",
        
        //37~(サンドイッチ)
        "カツサンド","たまごサンド","野菜サンド","ミートサンド","ベーコンサンド",
        
        //42~(マフィン)
        "ソーセージ\nマフィン","メガマフィン","ベーコンエッグ\nマフィン","ソーセージ\nエッグマフィン","てりたま\nマフィン",
        
        //47~(料理)
        "目玉焼き","ベーコンエッグ","ハンバーグ","チーズ\nハンバーグ","目玉ハンバーグ","目玉\nチーズ\nハンバーグ","カツ",
        "パンケーキ\n(1段)","パンケーキ\n(2段)","パンケーキ\n(3段)","パンケーキ\n(4段)","パンケーキ\n(5段)","サラダ","おまかせ\n(なんでもいい)"

    };
    [SerializeField] private SelectCustomer _SelectManager;
    [SerializeField] private CustomerManager _CustomerManager;
    [SerializeField] private GameSystem _GameManager;
    [SerializeField] private Text _OrderNum;
    [SerializeField] private Text _ProductName;
    [SerializeField] private Image _ProductImage;
    [SerializeField] private Text _HeightText;
    [SerializeField] private Text _IngredientText;
    [SerializeField] private Text _ValueText;
    private int _HeightNum;

    // Start is called before the first frame update
    void Start()
    {

        //_SelectManager = GameObject.Find("CustomerPlate").GetComponent<SelectCustomer>();
        //_JudgeManager = GameObject.Find("Managers").GetComponent<CustomerManager>();
        //_GameManager = GameObject.Find("Managers").GetComponent<GameSystem>();
        //_OrderNum = GameObject.Find("OrderNum").GetComponent<Text>();
        //_ProductImage = GameObject.Find("RecipeImage").GetComponent<Image>();
        //_ProductName = GameObject.Find("BurgerName").GetComponent<Text>();
        //_HeightText = GameObject.Find("BurgerHeight").GetComponent<Text>();
        //_IngredientText = GameObject.Find("Ingredient").GetComponent<Text>();

        //開店前のテキスト表示
        _OrderNum.text = "開店\n準備中...";
        _ProductName.text = "\nNo Guest";
        _ProductImage.sprite = RecipeList._Empty;
        _HeightText.text = "-";
        _HeightText.color = new Color(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_GameManager._IsStart)
        {
            if (!_CustomerManager.HasFinishFirst) return;
            var currentCustomer = _CustomerManager.CustomerInfo[_SelectManager._CustomerNum];
            if (!currentCustomer) return;

            if (currentCustomer._HasVisit && !currentCustomer._HasRecieve)
            {
                var orderNum = _CustomerManager.CustomerInfo[_SelectManager._CustomerNum]._OrderNum;
                var orderProduct = RecipeList.Menu[orderNum];

                //注文番号で商品を登録する
                _OrderNum.text = "No." + (orderNum + 1).ToString();
                _ProductName.text = orderProduct.name;
                _ProductImage.sprite = orderProduct.image;
                _HeightNum = CalcHeight(orderProduct.recipe);
                _HeightText.text = _HeightNum.ToString();
                _ValueText.text = "$" + orderProduct.value.ToString("N2");
                if (RecipeList.Menu[orderNum].type == RecipeList.ProductType.RANDOM) _HeightText.text = "?";

                //食材の高さによって色を変える
                if (_HeightNum >= 15) _HeightText.color = new Color(200 / 255f, 20 / 255f, 17 / 255f);
                else if (_HeightNum >= 7) _HeightText.color = new Color(255 / 255f, 239 / 255f, 0);
                else _HeightText.color = new Color(0, 0, 0);
                _IngredientText.text = "\nIngredients";
            }
            else
            {
                ChangeUIToNoCustomer();
            }
        }


    }

    int CalcHeight(int[] array)
    {
        int i;
        for (i = 0; i < array.Length; i++)
        {
            if (array[i] == 0) return i;
        }
        return i;
    }

    void UpdateProductUI()
    {
        //ここでUIの内容を更新する処理を書きたい
        //できればコルーチンも掛け合わせたいところ
    }

    void ChangeUIToNoCustomer()
    {
        _OrderNum.text = "Comming \nSoon...";
        _ProductName.text = "\nNo Guest";
        _ProductImage.sprite = RecipeList._Empty;
        _HeightText.text = "-";
        _ValueText.text = "$----";
        _HeightText.color = new Color(0, 0, 0);
    }

    IEnumerator UpdateUI()
    {
        yield return null;
    }
}
