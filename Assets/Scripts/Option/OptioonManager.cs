using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptioonManager : MonoBehaviour
{
    [SerializeField] private ControlSound[] _ControlSounds = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetSoundsVolumes()
    {
        for(int i = 0; i < _ControlSounds.Length;i++)
        {
            _ControlSounds[i]._ResetFlag = true;
        }
    }

    public void InitializeData()
    {
        PlayerPrefs.SetInt("day", 0);
        PlayerPrefs.SetInt("Complete", 0);
        GameDataManager.ResetData();
        ResetSoundsVolumes();
    }
}
