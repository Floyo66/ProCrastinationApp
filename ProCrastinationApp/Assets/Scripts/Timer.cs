using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class Timer : MonoBehaviour
{
    bool timerActive = false;

    bool sendNotification = false;

    
    public AudioSource Tick;
    public float displayTime;
    float currentBreakTime;
    public int startMinutes;

    public Slider sessionTime;
    public Slider breakTimeSlider;

    public Text currentTimeText;
    public Text currentBreakTimeText;

    public GameObject pomodoroPanel;
    public GameObject timerPanel;
   
    public GameObject stopButton;
    public GameObject currentTimeTextObj;
    public GameObject productiveTimeText;
    public GameObject currentBreakTimeTextObj;
    public GameObject currentBreakTimeObj;

    public GameObject bottomNav;

    public GameObject topNav;
    TimerDoneNotification notificationManager;

    public  Timer_C timerTest = new Timer_C();

    public string url;


    //variables after backup
    private string userId;
    public float numberTime;
    public DatabaseReference Reference;
    
    public int dbNumber;

    public float firstTimeNumber;
    public float secondTimeNumber;

    public bool timerDone = false;

    public bool startIt = false;

    public bool checkDone = false;

    public bool callOnce = false;
    public bool runMc = true;


    void Start()
    {
        // Create new User ID
        userId = SystemInfo.deviceUniqueIdentifier;
        // Get the root reference location of the database.
        Reference = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("display Time: " + displayTime);
        Debug.Log("slider time: " + sessionTime.value);

        
      // setDbNumber();
      // StartCoroutine(Get(url));

      


    }


    void Update()
    {  
        if(callOnce)
        {
            setDbNumber();
            StartCoroutine(Get(url));
            callOnce = false;
        }

        if(timerActive == true)
        {
            displayTime = displayTime - Time.deltaTime;

            if(displayTime <= 0 && CheckIfTimeDone())
            {
                timerActive = false;
                timerDone = true;
                hideObjects(productiveTimeText, currentTimeTextObj, stopButton );
                showObjectsDuringBreak(currentBreakTimeTextObj, currentBreakTimeObj,navigationBar);
                Debug.Log("Timer finished!");
            }
        }

        TimeSpan time = TimeSpan.FromSeconds(displayTime);
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








       ///////////MICROSERVICE///////////////// 
       if(runMc && CheckIfTimeDone() == false){
        StartCoroutine(Get2(url));
       }
        
        if(!checkDone)
        {
        if(CheckIfTimeDone() == true){
            checkDone = true;
            Debug.Log("TIME DONE");
        }else{
            Debug.Log("Time not done");
           
        }
        }
        ///////////////////////////////////////

    }

    

    public Boolean CheckIfTimeDone()
    {
        float result = (secondTimeNumber - firstTimeNumber)/60;
        Debug.Log("SecondNumber MINUS First = " + result);
        if((float)Math.Floor(result) == dbNumber && (float)Math.Floor(result) != 0){
            return true;
        }else{
            //Debug.Log("my Db number is " + dbNumber);
            return false;
        }

    }

    
public void setDbNumber()
    {
        Reference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.Message);
            }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
            
                    Reference.Child("users")
                        .Child(userId)
                        .Child("taskTime")
                        .SetValueAsync((int)sessionTime.value);

                   
                   //Debug.Log(dbNumber = int.Parse(snapshot.Child("taskTime").Value.ToString()));
                    //dbNumber = Convert.ToInt32(snapshot.Child("taskTime").Value.ToString());
                    loadData();
                    
                 
                }
       });
    }

    public void loadData()
    {
        Reference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.Message);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                dbNumber = int.Parse(snapshot.Child("taskTime").Value.ToString());
                

            }
        });
    }





    public void StartTimer()
    {
        if (sessionTime.value >= 1)
        {
            timerActive = true;
            callOnce = true;
            runMc = true;
            currentBreakTime = (int)breakTimeSlider.value * 60;
            displayTime = (int)sessionTime.value * 60;
            
            
        }
        else
        {
            timerActive = true;
            currentBreakTime = (int)breakTimeSlider.value * 60;
            displayTime = 1 * 60;
            Debug.Log(displayTime);
        }
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
                    
                     firstTimeNumber = float.Parse(timeValue.number.ToString());

                    //timerTest.setTimerValue(timesession);
                    //numberTime = timerTest.getTime_session();
                    

                    

                    

                    Debug.Log("FirstTimeNumber: " + firstTimeNumber);
                    //Debug.Log("time_Break = " + timeValue.nothing);
                   // Debug.Log(probe);
                    yield return firstTimeNumber;


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

    public IEnumerator Get2(string url)
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
                    
                     secondTimeNumber = float.Parse(timeValue.number.ToString());

                    //timerTest.setTimerValue(timesession);
                    //numberTime = timerTest.getTime_session();
                    

                    

                    

                   // Debug.Log("secondTimeNumber = " + secondTimeNumber);
                    //Debug.Log("time_Break = " + timeValue.nothing);
                   // Debug.Log(probe);
                    yield return secondTimeNumber;


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

    public void hideObjects(GameObject obj1, GameObject obj2, GameObject obj3){
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        
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
   
















































































    // Start is called before the first frame update
    /* void Start()
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
                showObjectsDuringBreak(currentBreakTimeTextObj, currentBreakTimeObj, bottomNav, topNav);
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

    public void showObjectsDuringBreak(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4){
        obj1.SetActive(true);
        obj2.SetActive(true);
        obj3.SetActive(true);    
        obj4.SetActive(true);  
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
 */
    
    




    
}
