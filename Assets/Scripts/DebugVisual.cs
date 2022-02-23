using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Debug用のスクリプト </summary>
public class DebugVisual : MonoBehaviour
{
    private Color[] _Colors =
    {
        new Color(0,0,0),
        new Color(1,0,0),
        new Color(0,1,0),
        new Color(0,0,1),
        new Color(1,1,0),
        new Color(0,1,1),
        new Color(1,0,1),
        new Color(1,1,1),
    };

    private int _CurrentColor = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// コイツをアタッチしたオブジェクトの色を変更するよ.
    /// コイツを使いたいならオブジェクトにSpriteRendererをアタッチしよう
    /// </summary>
    public void Change2DObjColorForDebug01()
    {
        _CurrentColor++;
        if (_CurrentColor >= _Colors.Length/2) _CurrentColor = 0;
        GetComponent<SpriteRenderer>().color = _Colors[_CurrentColor];
    }

    public void Change2DObjColorForError()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f);
    }

    public void Change2DObjColorForDebug02()
    {
        _CurrentColor++;
        if (_CurrentColor < _Colors.Length/2 || _CurrentColor >= _Colors.Length) _CurrentColor = _Colors.Length / 2;
        GetComponent<SpriteRenderer>().color = _Colors[_CurrentColor];
    }
}
