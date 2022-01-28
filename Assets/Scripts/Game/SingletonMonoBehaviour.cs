using UnityEngine;
using System.Collections;

/// <summary>
/// 生成するインスタンスの数を一つに制限する(Singletonパターン)
/// このクラスの子クラスになるとインスタンスの数を一つに制限できるようになる。
/// 
/// といってもUnityで扱うなら
/// １つのシーンに１つまでしかこのクラスがアタッチされたオブジェクトが配置できなくなる
/// </summary>
/// <typeparam name="T">クラス名</typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    //field
    private static T instance = null;

    //property
    public static T Instance
    {
        get
        {
            //呼び出された時点でinstanceへの割り振りを行う
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError(typeof(T) + " type Object is not found!!!!");
                }
            }

            return instance;
        }
    }
}