using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ゲームを中断するクラス </summary>
public class ExitScript : MonoBehaviour
{
    /// <summary> 押してる間のフレーム数 </summary>
    private int _PushingFrameCounter;

    // Start is called before the first frame update
    void Start()
    {
        _PushingFrameCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //エスケープキーが長押しされたら強制終了
        if(Input.GetKey(KeyCode.Escape))
        {
            _PushingFrameCounter++;
            if(_PushingFrameCounter > 60)UnityEngine.Application.Quit();
        }
        else _PushingFrameCounter = 0;
    }
}
