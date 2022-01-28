using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScript : MonoBehaviour
{
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("Complete"))PlayerPrefs.SetInt("Complete",0);
    }

    // Update is called once per frame
    void Update()
    {
        count ++;
        if(count > 100)SceneManager.LoadScene("Title");
    }

}
