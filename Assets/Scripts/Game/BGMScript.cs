using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> BGMを制御するスクリプト </summary>
public class BGMScript : MonoBehaviour
{
    AudioSource _AudioSource;
    public bool _Start = true;
    private bool _Flag = false;

    // Start is called before the first frame update
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        _AudioSource.volume = PlayerPrefs.GetFloat("BGMvol") * PlayerPrefs.GetInt("BGMex");        
        if(GameObject.Find("BGM2"))
        {
            GameObject.Find("BGM2").GetComponent<AudioSource>().volume = _AudioSource.volume;
            _Start = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _AudioSource.volume = PlayerPrefs.GetFloat("BGMvol") * PlayerPrefs.GetInt("BGMex"); 
        if(_Start)
        {
            if(!_Flag)
            {
                _AudioSource.Play();
                _Flag = true;
            }
        }
        else _AudioSource.Stop();
    }
}
