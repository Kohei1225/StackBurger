using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDataTest : MonoBehaviour
{
    string file = "Test.json";
    string file2 = "data.json";

    RankingManager _RankingManager = null;
    int _PreviousDate = 1;
    public int day = 1;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start JsonDataTest");
        //GameData.ResetData();
        //var data_instance = GameData.Instance.CurrentGameData;

        //GameData.Instance.CurrentGameData = GameDataManager.LoadData<GameData.Data>(file);
        //GameDataManager.SaveData(data_instance, file);

        //GameDataManager.LoadData<GameData>(file);
        _RankingManager = FindObjectOfType<RankingManager>().GetComponent<RankingManager>();

        /*
        var array = GameData.Instance.IntArray;
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i*2;
            Debug.Log("[" + i + "]:" + GameData.Instance.IntArray[i]);
        }
        Debug.Log("-----------------------------");
        for (int i = 0; i < array.Length; i++)
        {
            GameData.Instance.IntArray[i] = i+5;
            Debug.Log("[" + i + "]:" + array[i]);
        }
        Debug.Log("---------staic------------");
        for(int i = 0;i < s_Int_Array.Length;i++)
        {
            Debug.Log(s_Int_Array[i]);
        }
        */
    }

    void Update()
    {
        if (day != _PreviousDate)
        {
            _PreviousDate = day;
            _RankingManager.UpdateRankingView(day);
        }

    }
}
