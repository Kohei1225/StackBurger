using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//オプションで音量を確認するスクリプト
public class AudioTestScript : MonoBehaviour
{
    AudioSource AudioSource;
    SoundScript soundManager;

    float Volume;

    /// <summary> 音量確認する際に流す音源 </summary>
    public AudioClip[] Sounds;//ここは流石にUnity内でアタッチすればいいや
    int ClipNum = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        soundManager = GameObject.Find("Managers").GetComponent<SoundScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "BGMTestButton")
        {
            Volume = PlayerPrefs.GetFloat("BGMvol");
        }

        else if (gameObject.name == "ButtonTestButton")
        {
            Volume = PlayerPrefs.GetFloat("Buttonvol");
        }
        else if (gameObject.name == "SETestButton")
        {
            Volume = PlayerPrefs.GetFloat("SEvol");
        }
        AudioSource.volume = Volume;
    }

    public void OnClick()
    {
        AudioSource.Stop();
        AudioSource.PlayOneShot(Sounds[ClipNum]);
        ClipNum++;
        if(ClipNum == Sounds.Length)ClipNum = 0;
    }
}