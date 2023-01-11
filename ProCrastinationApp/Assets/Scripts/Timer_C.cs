using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Serializable, so unity will be able to convert to json.
[System.Serializable]
public class Timer_C
{
    public float number;

    public float nothing;
   



     public void setTimerValue(float number)
    {
        this.number = number;
       
    }

     public float getTime_session()
    {
        return number;
    } 
 

}
