using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipTextScript : MonoBehaviour
{
    public bool isDestroy = false;

    private void Awake()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SEvol") * PlayerPrefs.GetInt("SEex");
    }

    private void Start()
    {
        transform.localScale = new Vector3(1,1,1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroy) Destroy(gameObject);
    }
}
