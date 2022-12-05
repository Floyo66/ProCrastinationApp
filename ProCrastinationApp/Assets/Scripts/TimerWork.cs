using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


public class TimerWork : MonoBehaviour
{

    public string getUrl = "localhost:3000/timer";
    public string postUrl = "localhost:3000/timer/create";
    public string url;
    bool timerActive = false;
    bool firstTimeStart = true;

    bool collectedReward = false;

    bool sendNotification = false;

    bool timerDone = false;
    public AudioSource Tick;
    float currentTime;
    float currentBreakTime;
    public int startMinutes;

    public float sliderTimeValue;

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
        timerActive = true;
        
        currentBreakTime = (int)breakTimeSlider.value * 60;
        currentTime = sessionTime.value * 1;


        var timeSession = new Timer()
        {
            time = currentTime,
            timeBreak = currentBreakTime
        };
      
           
        StartCoroutine(Post(postUrl, timeSession));
        

    }

    // Update is called once per frame
    IEnumerator Update()
    {

        if (timerActive == true)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                //yield return it will make sure the progress complete until it's done
                yield return www.SendWebRequest();

                //Check for error
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

                        var timer = JsonUtility.FromJson<Timer>(result);


                        //enemyViewController.DisplayEnemyData(enemy.name, enemy.health.ToString(), enemy.attack.ToString());

                        currentTime = timer.time;
                        
                        currentTime = currentTime - Time.deltaTime;

                        if (currentTime <= 0)
                        {

                            timerActive = false;
                            timerDone = true;
                            Start();
                            hideObjects(productiveTimeText, currentTimeTextObj, pauseButton, stopButton);
                            showObjectsDuringBreak(currentBreakTimeTextObj, currentBreakTimeObj, navigationBar);
                            Debug.Log("Timer finished!");

                            if (sendNotification == false)
                            {

                                sendingMessage();
                            }
                            //sendNotification = false;



                        }

                    }
                    else
                    {
                        //handle the problem
                        Debug.Log("Error! data couldn't get.");
                    }
                }
            }



        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();

        if (timerDone == true)
        {
            currentBreakTime = currentBreakTime - Time.deltaTime;
            if (collectedReward == false)
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
/*
    public void StartTimer()
    {
        if (sessionTime.value >= 1)
        {
            timerActive = true;
            currentTime = sessionTime.value * 1;
        }
        else
        {
            timerActive = true;
            currentTime = 1 * 60;
        }
    }
*/

    public void sendingMessage()
    {
        if (sendNotification == false)
        {
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


    public void hideObjects(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4)
    {
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
    }

    public void showObjectsDuringBreak(GameObject obj1, GameObject obj2, GameObject obj3)
    {
        obj1.SetActive(true);
        obj2.SetActive(true);
        obj3.SetActive(true);
    }

    public void stateAfterBreak(GameObject obj1, GameObject obj2, GameObject obj3, GameObject obj4)
    {
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(true);

    }



    //IEnumerator enables async-like functinality in Unity.
    public IEnumerator Get(string url)
    {
        //Make Web request
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            //yield return it will make sure the progress complete until it's done
            yield return www.SendWebRequest();

            //Check for error
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

                    var timer = JsonUtility.FromJson<Timer>(result);

                    //enemyViewController.DisplayEnemyData(enemy.name, enemy.health.ToString(), enemy.attack.ToString());


                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }


    public IEnumerator Post(string url, Timer timer)
    {
        var jsonData = JsonUtility.ToJson(timer);
        Debug.Log(jsonData);

        using (UnityWebRequest www = UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
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
                    result = "{\"result\":" + result + "}";
                    var resultEnemyList = JsonHelper.FromJson<Timer>(result);

                    foreach (var item in resultEnemyList)
                    {
                        Debug.Log(item.time);
                    }
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
