using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseInit : MonoBehaviour
{
    public UnityEvent OnFirebaseInit = new UnityEvent();
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
    }

}
