using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//音量バランスをリセットするクラス
public class ResetButton : MonoBehaviour
{
    ControlSound [] _ControlSounds;

    // Start is called before the first frame update
    void Start()
    {
        _ControlSounds = new ControlSound[]{
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
            GameDataManager.ResetData();
        }
        for(int i = 0; i < _ControlSounds.Length; i++)_ControlSounds[i]._ResetFlag = true;
        
    }
}
