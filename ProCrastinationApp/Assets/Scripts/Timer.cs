using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Timer : MonoBehaviour
{
    bool timerActive = false;

    bool sendNotification = false;

    bool timerDone = false;
    public AudioSource Tick;
    float currentTime;
    float currentBreakTime;
    public int startMinutes;

    public Slider sessionTime;
    public Slider breakTimeSlider;

    public Text currentTimeText;
    public Text currentBreakTimeText;

    public GameObject pomodoroPanel;
    public GameObject timerPanel;
    public GameObject pauseButton;
    public GameObject stopButton;
    public GameObject currentTimeTextObj;
    public GameObject productiveTimeText;
    public GameObject currentBreakTimeTextObj;
    public GameObject currentBreakTimeObj;
    public GameObject navigationBar;
    TimerDoneNotification notificationManager;

    Timer_C timerTest;




    public string getUrl = "localhost:3000/timer";
    public string postUrl = "localhost:3000/timer/create";
    public string url;

    float probe;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Get(url));
        //currentTime = timerTest.getTime_session();
         //Debug.Log("THIS IS MY CURRENT TIME STAAAART: " + currentTime);
        currentBreakTime = (int)breakTimeSlider.value * 60;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive == true)
        {
            //StartCoroutine(Get(url));
            
            
            currentTime = (currentTime - Time.deltaTime);
            //Debug.Log("THIS IS MY CURRENT TIME: " + currentTime);
            if (currentTime <= 0)
            {
                
                timerActive = false;
                timerDone = true;
                Start();
                hideObjects(productiveTimeText, currentTimeTextObj ,pauseButton, stopButton );
                showObjectsDuringBreak(currentBreakTimeTextObj, currentBreakTimeObj,navigationBar);
                Debug.Log("Timer finished!");

                if(sendNotification == false){
                    
                sendingMessage();
                }
                //sendNotification = false;
                
                

            }
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();

        if (timerDone == true)
        {
            currentBreakTime = currentBreakTime - Time.deltaTime;
            if (currentBreakTime <= 0)
            {
                timerDone = false;
                Debug.Log("Break Time finished");
                stateAfterBreak(currentBreakTimeObj, currentBreakTimeTextObj, pomodoroPanel, timerPanel);
            }
            TimeSpan breakTime = TimeSpan.FromSeconds(currentBreakTime);
        currentBreakTimeText.text = breakTime.Minutes.ToString() + ":" + breakTime.Seconds.ToString();
        }

        

    }

    public void StartTimer()
    {
        if (sessionTime.value >= 1)
        {
            timerActive = true;
            currentTime = (int)sessionTime.value * 1;
        }
        else
        {
            timerActive = true;
            currentTime = 1 * 60;
        }
    }

    public void sendingMessage(){
        if(sendNotification == false){
                    sendNotification = true;
                    notificationManager = GameObject.FindGameObjectWithTag("NotificationTag").GetComponent<TimerDoneNotification>();
                    notificationManager.notificationDone();
                }
    }

    public void ResumeTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
        Start();
    }

    public void PauseTimer()
    {
        timerActive = false;

    }

  
    public void hideObjects(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4){
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
    }

    public void showObjectsDuringBreak(GameObject obj1, GameObject obj2,GameObject obj3){
        obj1.SetActive(true);
        obj2.SetActive(true);
        obj3.SetActive(true);      
    }

    public void stateAfterBreak(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4){
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(true);
        
    }

    //IEnumerator enables async-like functinality in Unity.
    public IEnumerator Get(string url)
    {
        using(UnityWebRequest www = UnityWebRequest.Get(url)){
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    var timeValue = JsonUtility.FromJson<Timer_C>(result);

                    

                    timerTest.setTimerValue(timeValue.time_session);

                    Debug.Log("Timer_session = " + timeValue.time_session);
                    Debug.Log("time_Break = " + timeValue.time_break);
                   // Debug.Log(probe);
                    yield return (float)probe;
                    


                    //Debug.Log(result);
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }

    }
    





}
