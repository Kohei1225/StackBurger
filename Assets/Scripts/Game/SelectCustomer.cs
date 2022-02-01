using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> お客さんを選択するスクリプト </summary>
public class SelectCustomer : MonoBehaviour
{
    private CustomerManager _CustomerManager;
    private GameSystem _GameManager;
    public int _CustomerNum { get; private set; } = 1;

    // Start is called before the first frame update
    void Start()
    {
        _CustomerManager = FindObjectOfType<CustomerManager>().GetComponent<CustomerManager>();
        _GameManager = FindObjectOfType<GameSystem>().GetComponent<GameSystem>();
        _CustomerNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift)) return;

        if(_GameManager._IsStart && _CustomerManager.HasFinishFirst)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))_CustomerNum--;
            else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))_CustomerNum++;
            if(_CustomerNum == -1)_CustomerNum = _CustomerManager.CurrentCustomers.Length - 1;
            else if(_CustomerNum == _CustomerManager.CurrentCustomers.Length)_CustomerNum = 0;
            gameObject.transform.position = new Vector3 (_CustomerManager.CurrentCustomers[_CustomerNum].transform.position.x, 3.9f, 10);
        }

    }
}
