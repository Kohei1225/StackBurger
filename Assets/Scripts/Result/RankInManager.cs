using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankInManager : SingletonMonoBehaviour<RankInManager>
{
    [SerializeField] private Button[] _Buttons = null;
    private Animation _Animation = null;
    private InputField _InputField = null;
    private ResultManager _ResultManager = null;

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
        _ResultManager = FindObjectOfType<ResultManager>();
    }

    public void PlayRankInAnim()
    {
        SetButtonInteractable(false);
        _Animation.Play();
    }

    public void ClickedDeciedButton()
    {
        var name = _InputField.text;
        if(name == "")
        {
            name = "Unknown";
        }
        var score = _ResultManager.FinalScore;
        var date = _ResultManager.CurrentDay;
        Debug.Log("name:" + name + " score:" + score);
        var rank_m = FindObjectOfType<RankingManager>().GetComponent<RankingManager>();
        rank_m.UpdateRanking(name, score, date);

        SetButtonInteractable(true);
        Destroy(gameObject);
    }

    void SetButtonInteractable(bool value)
    {
        for(int i = 0;i < _Buttons.Length;i++)
        {
            _Buttons[i].interactable = value;
        }
    }
}
