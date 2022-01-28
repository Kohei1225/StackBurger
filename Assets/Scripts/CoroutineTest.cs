using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start All");
        StartCoroutine("First");
        
        Debug.Log("FinishAll_0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator First()
    {
        Debug.Log("Enter First()");
        yield return null;
        StartCoroutine("Second");
        Debug.Log("Exit First()");
    }

    IEnumerator Second()
    {
        Debug.Log("Enter Second");
        yield return new WaitForSeconds(1);
        Debug.Log("Exit Second");
    }

    IEnumerator Third()
    {
        Debug.Log("Enter Third");
        yield return null;
        Debug.Log("Exit Third");
    }
}
