using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionMark : MonoBehaviour
{
    TimerScript _Timer = new TimerScript(3);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _Timer.UpdateTimer();
        if(_Timer.IsTimeUp)
        {
            Destroy(gameObject);
        }
    }
}
