using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FirebaseInit : MonoBehaviour
{




 public static bool firebaseReady;

    void Start()
    {
        CheckIfReady();
    }

    void Update()
    {
        if(firebaseReady == true)
        {
            SceneManager.LoadScene("FirebaseLogin");
        }
    }

    public static void CheckIfReady()
    {

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {

                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                firebaseReady = true;
                Debug.Log("Firebase is ready for use.");
            }
            else
            {
                firebaseReady = false;
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }



























/*     public UnityEvent OnFirebaseInit = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Exception != null) {
                Debug.LogError($"Failed to initialize Firebase with {task.Exception}");
                return;
            }
            
        });
        OnFirebaseInit.Invoke();
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
    } */

}
