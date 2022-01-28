using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> これから落とす食材を表示するモニターにアタッチするスクリプト </summary>
public class MonitorScript : MonoBehaviour
{
    private MoveOfFood ThrowManager;
    private GameSystem _GameManager;
    /// <summary> 食べ物が既に表示されてるかどうか </summary>
    private bool _IsDisplayFood = false;        
    /// <summary> 表示する食材 </summary>
    private GameObject _ActiveFood;
    /// <summary> 表示させる座標 </summary>
    private Vector3 _DisplayPosition;           
    /// <summary> 表示を更新するかどうか </summary>
    public bool CanDisplay = false;
    /// <summary> 表示する食材の番号 </summary>
    public int Foodnum = 0;     

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = GameObject.Find("Managers").GetComponent<GameSystem>();

        //オブジェクトによっては位置調整
        if(gameObject.name == "NOWMonitor")_DisplayPosition = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z - 0.1f);
        else _DisplayPosition = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z - 0.1f);
        _IsDisplayFood = false;
    }

    // Update is called once per frame
    void Update()
    {
        //更新する合図が来たら
        if(CanDisplay)
        {
            //表示してる食べ物があったら消す
            if(_IsDisplayFood)
            {
                Destroy(_ActiveFood);
                _IsDisplayFood = false;
            }
            
            //表示するべき食材があれば表示する
            if(Foodnum != 0)
            {
                //Debug.Log("foodNum:" + Foodnum);
            
                _ActiveFood = Instantiate(_GameManager.FoodPrefabs[Foodnum - 1],_DisplayPosition,Quaternion.identity);
                Destroy(_ActiveFood.GetComponent<Rigidbody2D>());
                Destroy(_ActiveFood.GetComponent<BoxCollider2D>());
                Destroy(_ActiveFood.GetComponent<FoodScript>());
                _ActiveFood.transform.localScale = new Vector3(1.6f,1.6f,1);
                _IsDisplayFood = true;
            }
            CanDisplay = false;
        }
    }
}
