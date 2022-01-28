using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//コルーチンの理解度を上げる練習用のスクリプト
public class CoroutineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start All");
        StartCoroutine("First");
        StartCoroutine("Third");
        Debug.Log("FinishAll_0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator First()
    {
        Debug.Log("Enter First()");
        for(int i = 0;i < 5;i++)
        {
            Debug.Log("First["+i+"]");
            yield return null;
        }
        
        StartCoroutine("Second");
        Debug.Log("Exit First()");
    }

    IEnumerator Second()
    {
        Debug.Log("Enter Second");
        for(int i=0;i<5;i++)
        {
            Debug.Log("Second[" + i + "]");
        }
        yield return new WaitForSeconds(1);
        Debug.Log("Exit Second");
    }

    IEnumerator Third()
    {
        Debug.Log("Enter Third");
        for(int i = 0;i < 3;i++)
        {
            Debug.Log("Third[" + i + "]");
            yield return null;
        }
        Debug.Log("Exit Third");
    }
}
