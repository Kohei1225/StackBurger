using UnityEngine;

/// <summary> 時間計測用クラス </summary>
public class TimerScript
{
    #region constructor
    public TimerScript(){}

    public TimerScript(float time)
    {
        _TimeLimit = time;
    }
    #endregion

    #region property
    public bool IsTimeUp
    {
        get { return _CurrentTime >= _TimeLimit; }
    }

    public float RateOfRemainingTime
    {
        get { return 1 - _CurrentTime / _TimeLimit; }
    }

    public float RateOfTime
    {
        get { return _CurrentTime / _TimeLimit; }
    }

    public  float CurrentTime
    {
        get { return _CurrentTime; }
    }

    public float TimeLimit
    {
        get { return _TimeLimit; }
    }
    #endregion

    #region field
    float _TimeLimit = 0;
    float _CurrentTime = 0;
    #endregion

    #region public function
    public void ResetTimer(float timeLimit)
    {
        _TimeLimit = timeLimit;
        _CurrentTime = 0;
    }

    public void UpdateTimer()
    {
        _CurrentTime += Time.deltaTime;
    }

    public void UpdateTimer(float speed)
    {
        _CurrentTime += Time.deltaTime * speed;
    }
    #endregion
}