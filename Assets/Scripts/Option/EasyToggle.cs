using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//簡単モードにするかのチェックボックスを管理するクラス
public class EasyToggle : MonoBehaviour
{
    Toggle CheckBox;
    // Start is called before the first frame update
    void Start()
    {
        CheckBox = GetComponent<Toggle>();
        if(!PlayerPrefs.HasKey("Easy"))PlayerPrefs.SetInt("Easy",0);
        if(PlayerPrefs.GetInt("Easy") == 1)CheckBox.isOn = true;
        else CheckBox.isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        //オンなら1に、オフなら0に数値を設定する。
        if(CheckBox.isOn)PlayerPrefs.SetInt("Easy",1);
        else PlayerPrefs.SetInt("Easy",0);
    }
}
