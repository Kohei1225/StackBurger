using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 置き手紙の処理を行うクラス </summary>
public class MessageScript : MonoBehaviour
{
    GameSystem _GameManager;
    Text _MessageText;
    Text _ButtonText;
    GameObject _BackButton;
    /// <summary> ページを捲る音 </summary>
    [SerializeField]private AudioClip _ChangePageSound = null;
    /// <summary> 動的に動く今見てるページ </summary>
    private int _CurrentPage = 0;
    /// <summary> 置き手紙の内容 </summary>
    /// 多分23文字くらいが１行に収まる限界。１ページあたりの行数は７行が限界。
    private string [,] _LetterMessage = {
        //初日
            {
                "新人くんへ\nはじめまして！\nStackBurgerへようこそ！\nまぁ、今日オープンしたキッチンカーだから\n僕も新人みたいなもんなんだけどね。\n手紙が置いてあってビックリしたかもしれないけど\n僕はちゃんと店にいるので安心してくれ\n本当は直接挨拶するべきなんけど僕シャイで...",
                "早速今日から働いてもらう訳だけど\n見ての通り狭いだろ？\nだから出来るだけ汚したくないんだ。\nそこで、調理用の皿をいくつか用意した。\n僕は裏で具材を調理するから君には\n調理した具材を皿に積んで料理を作って欲しい。\nこれだと料理というより盛り付けの方が近いかな？",
                "倒れるかもしれないからあんまり乗せすぎるなよ\nそうだなー。30枚は乗せられないと思ってくれ。\nあと、僕は食材を調理してそっちに投げる。\nだからある程度積んだ皿に投げると\nその衝撃で倒れるから\n新しい食材は余裕がある皿に乗せてくれ。\nそれと、提供は青い皿だからな。忘れないでくれよ",
                "注文されたらレシピが出てくるから\n基本的にはレシピ通りに作ってくれ。\nレシピと違うと味が変わっちゃうからね。\nまぁ限度はあるけど\n多少味が違くても受け取ってくれるとは思う...多分。\n何事も完璧じゃなくたっていいんだよ。\n気楽に行けばいいさ。",
                "色々と長くなったけど...\nつまり君の仕事は具材を積んで料理を提供するだけ！\nどうだい？簡単だろ？\nまぁ習うより慣れろだ！！\nさぁ、準備ができたら\nシャッターを開けて仕事にかかろう！\nこっちは準備万端だ！　　　　　　　　　店長", 
            },
        //

        //2日目
            {
                "新人くんへ\nやあ、昨日の君の働きもあって\nクチコミが広がったらしい！！\n仕事は簡単だったろ？\n今日はお客さんも増える気がするよ\n新しいメニューを考えてもいいかもしれないな...",
                "そういえば、\n昨日書いたのはちょっと分かりづらかったかもな。\n長々と書いたがあれだ、\n要は信号と同じだよ。\n黄色が注意で赤が危険。...あれ？違う？\nまぁ細かいことはいいや。",
                "そうそう、\nお客さんの切り替えは\n'Shift'+'左右キー'で切り替えられるよ！！\n注目してるお客さんの前にトレイを置く感じだね。\nあと、間違えて違うお客さんに提供しないように！\nまぁ、この調子で昨日みたいによろしく頼むよ\n今日もがんばろう！　　　　　　　　　　店長",
                "",
                "",
                
            },
        //

        //3日目
            {
                "新人くんへ\nおはよう！昨日はお客さんが増えてよかったよ！\nそこで、君にお知らせがある。\nメニューを増やすことにした！\n見たことあるようなバーガーも出てくるかもしれんが\n気にしないでいつも通りやってくれ。",
                "そういえば、\n言い忘れてたんだが\nお客さんは提供された商品が\n明らかに違う場合は受け取らないから気をつけろよ\n例えば君がバーガーを頼んだのに\n肉と野菜でパンを挟んだ何かが出てきたら困るだろ？\nそれと一緒だ。まぁ今更言うまでもないか。　店長",
                "",
                "",
                ""
            },
        //

        //4日目
            {
                "新人くんへ\n昨日は新しいバーガーも売れてよかったよ。\nやっぱり追加して正解だった！\nさぁ、ここで君に朗報だ。\nバーガーはバンズが上と下で違うから\n神経を使うよな？\nだからそんな君が苦労しないメニューを考えたよ",
                "それは...ジャーン！\nサンドイッチだ！\nこれならバンズが上下一緒だから安心だ。\nさらに！！\nサンドイッチはバーガーと違って\n中身の順番がぐちゃぐちゃでも関係ない！\nこれは嬉しい！",
                "まぁバンズが薄いから\nバーガーの下のバンズと見た目が似てるけど\n君なら大丈夫だろう！\nバーガーのバンズと間違えるなよ？\n中身があっててもバンズが違ったらダメだからな！",
                "そういえばお客さんから\n調理場を覗いたら\nケチャップが他の具材に埋まってた\nって報告があったんだけど\n何か知ってる？僕には全く分からないんだけど。\nもし君も同じ現象を見かけたら\n埋まってる食材を持ち上げてみてくれ。　　　店長",
                ""
            },
        //

        //5日目
            {
                "新人くんへ\n昨日のサンドイッチはどうだった？\n作りやすかっただろー\nおかげさまで売り上げはまぁまぁいいから\n感謝してるよ。ありがとう。\nオープニングスタッフとは思えないくらいの\n働きぶりだよ。君は。",
                "そんな君に感謝の意を込めて、\nまたまた新メニューだ！\n今度はマフィンを追加することにしたよ\nマフィンもサンドイッチ同様バンズが違う。\nただ順番はちゃんと決まってるから\nバーガーとサンドイッチの中間とでも思ってくれ\n今度は間違えようがないから安心してくれ！",
                "一応店長である僕からアドバイス\nお客さんが来たら\nまずは全員の注文に目を通した方がいい\nそこで優先順位を大体決めることが出来るからな\nただ、最初に決めた優先順位に縛られることはない。\nすぐに提供できるものがあったら提供してくれ。\n他のことでも言えることだが、臨機応変に。　店長",
                "",
                ""
            },
        //

        //6日目
            {
                "新人くんへ\n君の積みさばきも磨きがかかってるな！\nだいぶお客さんも来るようになったよ\nそんな君を見込んでメニュー追加だ！\nきっと君も驚くと思うよ。",
                "商売において大切なのはまず知ってもらうことだ。\n知らないことには店にも来てもらえないからね。\nそこで、巨大バーガーを考案した！\nデカイものには話題性があるから\n大食い自慢がこぞって来るって訳だ。\n僕も試しに食べてみたけど\n胸焼けで死にそうだよ。年はとりたくないな",
                "大きい分作るのが大変かもしれないが、悪い事ばかりじゃないぞ。\nデカいとミスをしてもバレにくいし\nお客さんも長時間待ってくれる。\n完璧に作るに越したことはないけど\n手を抜ける部分は手を抜いてもバチは当たらないはずだ。",
                "それと、いい知らせがひとつ。\n最近、この町では「おまかせ」が流行ってるらしい。\nお客さんを捌いてるうちにその「おまかせ」とやら\nを注文するお客さんが来るかもしれないが、\nその時は余ってる食材を押し付けてやれ。\nそれで満足するんだから楽なもんだ。\n　　　　　　　　　　　　　　　　　　　　　店長",
                ""
            },
        //
            
        //7日目
            {
                "新人くんへ\n昨日酒を飲みながら考えたんだ。\nこの店には何が足りないかって。\n君はこの店に足りないのは何だと思う？\nそう、「サイドメニュー」だ！！\n目玉焼きにハンバーグ、パンケーキとかだな。",
                "多分だが他のものが入ってるとすぐにバレる。\nあと、バターが乗ってる料理は\n絶対一番上にバターを乗せてくれ。\n理由は見た目で手抜きがバレるからだ！！\n追加するメニューはこれで最後。頑張って作ってくれ\n",
                "ついに君が来てからいよいよ１週間だ。\nもう君は新人ではないのかもしれないな。\n君のおかげで\nStackBurgerはたちまち人気フード店だ！\nそれに今日は休日だし\nお客さんが今までで一番来るだろうな。",
                "そんな君とも今日でお別れか。\nだがしんみりしている場合じゃない！\n今日ほどお客さんが来る日はないからな！\n\nよし、準備ができたらシャッターを開けて\n最後の仕事にかかるぞ！\nこっちは準備万端だ！　　　　　　　　店長",
                "P.S.\n気が向いたらまた来てくれよ。\n今度はお客さんとしてね。\nその時はオマケしとくからさ。\nもちろん従業員として来てくれてもいいけどね。\nハハハハ！冗談だよ。",
                
            }
        //
    };
    /// <summary> 各手紙のページ数 </summary>
    int [] _PageMax = {
        4,2,1,3,2,3,2
    };

    private int _MaxPage;
    private bool _HasRead = false;
    private AudioSource _AudioSource = null;

    #region property
    public int MaxPage
    {
        get { return _MaxPage; }
    }

    /// <summary> 現在見てるページ </summary>
    public int CurrentPage
    {
        set
        {
            //許容範囲を超えてなければ代入
            if(0 <= value && value <= MaxPage)
            {
                _CurrentPage = value;
            }
        }
        get { return _CurrentPage; }
    }

    /// <summary> 読み終えたか </summary>
    public bool HasRead
    {
        set
        {
            if(CurrentPage != _MaxPage)
            {
                return;
            }
            _HasRead = value;
        }
        get { return _HasRead; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントの取得
        _GameManager = GameObject.Find("Managers").GetComponent<GameSystem>();
        _MessageText = GameObject.Find("Message").GetComponent<Text>();
        _ButtonText = GameObject.Find("NextText").GetComponent<Text>();
        _BackButton = GameObject.Find("BackPage");
        //テキストの初期化
        _MessageText.text = _LetterMessage[_GameManager.CurrentDay - 1,0];
        _MaxPage = _PageMax[_GameManager.CurrentDay - 1];
        _AudioSource = GetComponent<AudioSource>();
        _AudioSource.volume = PlayerPrefs.GetFloat("SEvol") * PlayerPrefs.GetInt("SEex");
    }

    // Update is called once per frame
    void Update()
    {
        //まだ読み終わってなかったら
        if (!HasRead)
        {
            //最後のページまで読んだらボタンのテキストを「閉じる」に変える
            if (CurrentPage == MaxPage)
            {
                _ButtonText.text = "閉じる";
            }
            //それ以外の時はボタンのテキストを「Next」にする。
            else
            {
                _ButtonText.text = "Next";
                
            }

            //対応するキーが入力されたらページをめくる
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space))
            {
                TurnNextPage();
            }
            
            //最初のページだったら前に戻るボタンを消す
            if (CurrentPage == 0)
            {
                _BackButton.SetActive(false);
            }
            //前にページがあれば戻るボタンを表示
            else
            {
                _BackButton.SetActive(true);
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Backspace))
                {
                    TurnPreviousPage();
                }
            }
        }
        //読み終わったらもう必要ないので消す
        else
        {
            _GameManager._HasRead = true;
            Destroy(gameObject);
        }
    }

    /// <summary> 次のページへ </summary>
    public void TurnNextPage()
    {
        PlayPageSound();

        //最後のページだったら読み終えた判定だけする
        if(CurrentPage == MaxPage)
        {
            HasRead = true;
            return;
        }

        CurrentPage++;
        var day = _GameManager.CurrentDay - 1;

        //メッセージを更新する
        _MessageText.text = _LetterMessage[day, CurrentPage];
    }

    /// <summary> 前のページへ </summary>
    public void TurnPreviousPage()
    {
        CurrentPage--;
        var day = _GameManager.CurrentDay - 1;

        //メッセージを更新する
        _MessageText.text = _LetterMessage[day, CurrentPage];
        PlayPageSound();
    }

    /// <summary> ページを捲る音を再生 </summary>
    private void PlayPageSound()
    {
        if(_ChangePageSound == null)
        {
            return;
        }

        _AudioSource.Stop();
        _AudioSource.PlayOneShot(_ChangePageSound);
    }
}
