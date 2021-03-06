using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> お客さんを選択するスクリプト </summary>
public class SelectCustomer : SingletonMonoBehaviour<SelectCustomer>
{
    private CustomerManager _CustomerManager;
    private GameSystem _GameManager;
    public int _CustomerNum { get; private set; } = 1;
    private bool _HasSetIniPos = false;


    void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _CustomerManager = FindObjectOfType<CustomerManager>().GetComponent<CustomerManager>();
        _GameManager = FindObjectOfType<GameSystem>().GetComponent<GameSystem>();
        _CustomerNum = _GameManager._CustomerNum-1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_GameManager._IsStart || !_CustomerManager.HasFinishFirst) return;

        if(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))_CustomerNum--;
            else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))_CustomerNum++;
            if(_CustomerNum == -1)_CustomerNum = _CustomerManager.CurrentCustomers.Length - 1;
            else if(_CustomerNum == _CustomerManager.CurrentCustomers.Length)_CustomerNum = 0;
            
        }
        gameObject.transform.position = new Vector3 (_CustomerManager.CurrentCustomers[_CustomerNum].transform.position.x, 3.9f, 10);
    }

    public void SetInitializePlatePos(int pos)
    {
        if (_HasSetIniPos) return;

        _CustomerNum = pos;
        _HasSetIniPos = true;
        Debug.Log("Pos:" + pos);
    }
}
