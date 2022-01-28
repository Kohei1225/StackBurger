using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        PlayerPrefs.Save();
        GameObject.Find("Managers").GetComponent<TitleManager>()._State = TitleManager.TitleState.Title;
    }
}
