using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//音量バランスをリセットするクラス
public class ResetSound : MonoBehaviour
{
    ControlSound [] CSs;

    // Start is called before the first frame update
    void Start()
    {
        CSs = new ControlSound[]{
            GameObject.Find("BGMVolumeSlider").GetComponent<ControlSound>(),
            GameObject.Find("ButtonVolumeSlider").GetComponent<ControlSound>(),
            GameObject.Find("SEVolumeSlider").GetComponent<ControlSound>()
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if(gameObject.name == "ResetDataButton")
        {
            PlayerPrefs.SetInt("day",0);
            PlayerPrefs.SetInt("Complete",0);
        }
        for(int i = 0; i < CSs.Length; i++)CSs[i]._ResetFlag = true;
        
    }
}
