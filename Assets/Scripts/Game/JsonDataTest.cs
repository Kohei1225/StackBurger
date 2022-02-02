using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonDataTest : MonoBehaviour
{
    string file = "Test.json";
    string file2 = "data.json";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start JsonDataTest");
        var data_instance = GameData.Instance.CurrentGameData;

        GameDataManager.SaveData(data_instance, file);
        GameDataManager.LoadData<GameData>(file);

    }
}
