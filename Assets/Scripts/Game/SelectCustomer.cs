using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> お客さんを選択するスクリプト </summary>
public class SelectCustomer : MonoBehaviour
{
    CustomerManager _CustomerManager;
    GameSystem GameManager;
    public int CustomerNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        _CustomerManager = GameObject.Find("Managers").GetComponent<CustomerManager>();
        GameManager = GameObject.Find("Managers").GetComponent<GameSystem>();
        CustomerNum = 1;
        //if(GameManager.level == 1)CustomerNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift)) return;

        if(GameManager._IsStart && !_CustomerManager.HasFinishFirst)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))CustomerNum--;
            else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))CustomerNum++;
            if(CustomerNum == -1)CustomerNum = _CustomerManager.CurrentCustomers.Length - 1;
            else if(CustomerNum == _CustomerManager.CurrentCustomers.Length)CustomerNum = 0;
            gameObject.transform.position = new Vector3 (_CustomerManager.CurrentCustomers[CustomerNum].transform.position.x, 3.9f, 10);
        }

    }
}
