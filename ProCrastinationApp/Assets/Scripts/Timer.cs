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

    public float sessionTime;
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

    public  Timer_C timerTest = new Timer_C();




    public string getUrl = "localhost:3000/timer";
    public string postUrl = "localhost:3000/timer/create";
    public string url;



    public float probe;
    public bool firstCall = false;
    internal object user;

    public bool startTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        
        //sessionTime.value = timerTest.getTime_session();
        //currentTime = timerTest.getTime_session();
         //Debug.Log("THIS IS MY CURRENT TIME STAAAART: " + currentTime);
        currentBreakTime = (int)breakTimeSlider.value * 60;
        //currentTime = timerTest.getTime_session();
         StartCoroutine(Get(url));
        
    }

    // Update is called once per frame
     void Update()
    {
        if(firstCall)
        {
            Start();
            firstCall = false;
            startTimer = true;
            
           
        }
        //Debug.Log("Session Time Update: " + timerTest.getTime_session());
        if (timerActive == true && startTimer == true)
        {
            
            Debug.Log("CurrentTime Value: " + currentTime);
            
            currentTime = (currentTime - Time.deltaTime);

            //timerTest.setTimerValue(currentTime);
            //Debug.Log("THIS IS MY CURRENT TIME: " + currentTime);
            if (currentTime <= 0)
            {
                
                timerActive = false;
                timerDone = true;
                //startTimer = false;
                //Start();
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
            firstCall = false;
            
            currentBreakTime = currentBreakTime - Time.deltaTime;
            if (currentBreakTime <= 0)
            {
                timerDone = false;
                
                Debug.Log("Break Time finished");
                //firstCall = true;
                
                
                stateAfterBreak(currentBreakTimeObj, currentBreakTimeTextObj, pomodoroPanel, timerPanel);
            }
            TimeSpan breakTime = TimeSpan.FromSeconds(currentBreakTime);
            currentBreakTimeText.text = breakTime.Minutes.ToString() + ":" + breakTime.Seconds.ToString();
        }

        

    }

    public void StartTimer()
    {
        
        firstCall = true;

        //StartCoroutine(Get(url));

        /* StartCoroutine(Get(url));
        sessionTime.value = timerTest.getTime_session();
        Debug.Log(timerTest.getTime_session());

        if (sessionTime.value >= 1)
        {
            timerActive = true;
            currentTime = (int)sessionTime.value * 1;
        }
        else
        {
            timerActive = true;
            currentTime = 1 * 60;
        } */


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
        firstCall = true;
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
                    
                    float timesession = float.Parse(timeValue.time_session.ToString());

                    timerTest.setTimerValue(timesession);
                    sessionTime = timerTest.getTime_session();
                    currentTime = timerTest.getTime_session();
                    

                   // float TestTime = timesession - Time.deltaTime;
                    //Debug.Log(TestTime);

                    //Debug.Log(timerTest.getTime_session());
                    //timerTest.setTimerValue(timeValue.time_session);

                    //Debug.Log("Timer_session = " + timeValue.time_session);
                    //Debug.Log("time_Break = " + timeValue.time_break);
                   // Debug.Log(probe);
                    yield return timesession;
                    Debug.Log("Session Time: " + currentTime);

                    timerActive = true;

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
