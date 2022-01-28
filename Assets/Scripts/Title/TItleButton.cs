using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> タイトル画面で使うボタンのスクリプト </summary>
public class TItleButton : MonoBehaviour
{
    TitleManager _TitleManager;

    // Start is called before the first frame update
    void Start()
    {
        _TitleManager = GameObject.Find("Managers").GetComponent<TitleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        if(gameObject.name == "ExplainButton")
        {
            _TitleManager._State = TitleManager.TitleState.Explain;
        }

        if(gameObject.name == "OptionButton")
        {
            _TitleManager._State = TitleManager.TitleState.Option;
        }
    }
}
