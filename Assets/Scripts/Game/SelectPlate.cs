using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 皿を選択するクラス </summary>
public class SelectPlate : MonoBehaviour
{
    public GameObject [] Plate;
    public int plateNum = 1;
    GameObject Letter;
    //public GameObject Cusor;//カーソル

    // Start is called before the first frame update
    void Start()
    {
        Letter = GameObject.Find("LetterObject");
        plateNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //置き手紙を読み終えたら動かせる
        if(!Letter)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) return;

            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))plateNum++;
            else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))plateNum--;
            if(plateNum == -1)plateNum = 3;
            else if(plateNum == 4)plateNum = 0;

            gameObject.transform.position = new Vector3 (Plate[plateNum].transform.position.x, Plate[plateNum].transform.position.y - 0.4f, 16);
        }

    }
}
