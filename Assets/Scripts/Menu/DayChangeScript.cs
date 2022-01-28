using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ステージを変更するボタンを管理するクラス </summary>
public class DayChangeScript : MonoBehaviour
{
    CalenderManager _Calender;
    AudioSource _AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _Calender = GameObject.Find("Calendar").GetComponent<CalenderManager>();
        _AudioSource = GameObject.Find("Calendar").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        if(gameObject.name == "Tomorrow")
        {
            _Calender.CurrentDay++;
            _AudioSource.Stop();
            _AudioSource.PlayOneShot(_Calender._ChangeDaySound);
        }
        if(gameObject.name == "Yesterday")
        {
            _Calender.CurrentDay--;
            _AudioSource.Stop();
            _AudioSource.PlayOneShot(_Calender._ChangeDaySound);
        }
    }
}
