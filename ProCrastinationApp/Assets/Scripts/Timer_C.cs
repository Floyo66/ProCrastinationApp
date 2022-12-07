using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Serializable, so unity will be able to convert to json.
[System.Serializable]
public class Timer_C
{
    public float time_session;
    public float time_break;



    public void setTimerValue(float time_session)
    {
        this.time_session = time_session;
       
    }

    public float getTime_session()
    {
        return time_session;
    }


}
