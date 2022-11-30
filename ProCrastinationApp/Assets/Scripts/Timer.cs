using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Timer : MonoBehaviour
{
    
    

    bool timerActive = false;

    bool collectedReward = false;

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

    public GameObject RewardButton;

    public GameObject LevelBar;

    public GameObject ProgressBar;
    TimerDoneNotification notificationManager;

    


    // Start is called before the first frame update
    void Start()
    {
        currentBreakTime = (int)breakTimeSlider.value * 60;
        
        
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive == true)
        {
            currentTime = currentTime - Time.deltaTime;
            //currentTime = _timerMicroserviceClient.CountTimeDown(currentTime);
             //_timerMicroserviceClient.CountTimeDown(currentTime); 
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
            if(collectedReward == false)
            {
            collectedReward = true;
            RewardButton.SetActive(true);
            }
            LevelBar.SetActive(true);
            ProgressBar.SetActive(true);
            if (currentBreakTime <= 0)
            {
                LevelBar.SetActive(false);
                RewardButton.SetActive(false);
                ProgressBar.SetActive(false);
                timerDone = false;
                Debug.Log("Break Time finished");
                stateAfterBreak(currentBreakTimeObj, currentBreakTimeTextObj, pomodoroPanel, timerPanel);
                collectedReward = false;
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

   /*  private async void SetupBeamable()
   {
    //await context.OnReady;
    var context = BeamContext.Default;

    _timerMicroserviceClient = context.Microservices().TimerMicroservice();
     
    
   } */

    



}
