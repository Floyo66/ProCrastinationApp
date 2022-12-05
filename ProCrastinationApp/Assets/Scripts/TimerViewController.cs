using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerViewController : MonoBehaviour
{
    public Text timeText;
    public Text breakText;
   


    public void DisplayEnemyData(string time, string breakTime)
    {
        timeText.text = time;
        breakText.text = breakTime;
       

    }
}
