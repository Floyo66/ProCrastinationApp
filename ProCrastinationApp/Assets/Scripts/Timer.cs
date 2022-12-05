using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Serializable, so unity will be able to convert to json.
[System.Serializable]
public class Timer
{
    public float time;
    public float timeBreak;


    private void setTimer(float time_param, float timebreak_param)
    {
        this.time = time_param;
        this.timeBreak = timebreak_param;
    }

    
}
