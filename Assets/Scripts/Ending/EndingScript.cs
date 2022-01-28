using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//エンディング用スクリプト
public class EndingScript : MonoBehaviour
{
    public GameObject [] foods;
    private string [] _names = {
        "バンズ(下)","バンズ(中)","バンズ(上)","パティ","ピクルス","レタス","トマト","玉ねぎ","チーズ","カツ",
        "卵","アボカド","良い肉","ベーコン","パンケーキ","シロップ","バター","ソース","ケチャップ","マヨネーズ",
        "〜お借りしたサウンド〜\n","「無料効果音で遊ぼう！」\n小森平 さま\n","「魔王魂」\n森田交一 さま\n","「ニコニ・コモンズ」\nフリー素材あそび さま\nNEW ROMANTIC BOY さま\n\n","製作\nAmusementMakers\nモル\n\n"
    };

    private bool _start = true;   
    private bool _generate = true;  //新しく作るタイミング
    private Text _nameText;
    private GameObject _nameObject;
    private int _junban = 0;        //それぞれ紹介する順番
    private GameObject food;        //食材のオブジェクト
    private GameObject kansya;      //ユーザーへの感謝のメッセージ
    private int _counter = 0;       //
    private float _time = 0;        //

    // Start is called before the first frame update
    void Start()
    {
        _nameObject = GameObject.Find("Name");
        GameObject _nameTextObject = _nameObject.transform.Find("NameText").gameObject;
        _nameText = _nameTextObject.GetComponent<Text>();
        
        kansya = GameObject.Find("Kansha");
    }

    // Update is called once per frame
    void Update()
    {
        if(_start)
        {
            //タイミングが来て順番が範囲内だったら
            if(_generate && _junban < foods.Length)
            {
                food = Instantiate(foods[_junban],new Vector3(gameObject.transform.position.x,transform.position.y, 0), Quaternion.identity);
                Destroy(food.GetComponent<Rigidbody2D>());
                Destroy(food.GetComponent<FoodScript>());
                _nameText.text = _names[_junban];
                _generate = false;
            }
            //まだオブジェクトが生きてる時
            if(food)
            {
                _time += Time.deltaTime;
                //food.transform.Translate(0.25f,0,0);
                food.transform.position = new Vector3(gameObject.transform.position.x + _time * 26,gameObject.transform.position.y,0);
                _nameObject.transform.position = new Vector3(food.transform.position.x,-120/27f,1);

                if(food.transform.position.x > 50)
                {
                    Destroy(food);
                    _junban++;
                    _generate = true;
                    _time = 0;
                }
                if(_names.Length > _junban)_nameText.text = _names[_junban];
            }

            if(_junban == foods.Length)
            {
                if(kansya.transform.position.x < 0)kansya.transform.Translate(0.25f,0,0);
                else _counter++;
            }

            if(_counter > 300)SceneManager.LoadScene("Title");
        }
    }
}
