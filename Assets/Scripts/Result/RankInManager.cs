using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankInManager : SingletonMonoBehaviour<RankInManager>
{
    [SerializeField] private Button[] _Buttons = null;
    private Animation _Animation = null;
    [SerializeField]private InputField _InputField = null;

    void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _Animation = GetComponent<Animation>();
        _InputField = transform.Find("NewRankInObject/InputField").GetComponent<InputField>();
        PlayRankInAnim();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRankInAnim()
    {
        for(int i = 0;i < _Buttons.Length;i++)
        {
            _Buttons[i].interactable = false;
        }
        _Animation.Play();
    }
    public void ClickedDeciedButton()
    {
        var name = _InputField.text;
        var score = FindObjectOfType<ResultManager>().GetComponent<ResultManager>().FinalScore;
        Debug.Log("name:" + name + " score:" + score);
        var rank_m = FindObjectOfType<RankingManager>().GetComponent<RankingManager>();
        //rank_m.UpdateRanking()
    }
}
