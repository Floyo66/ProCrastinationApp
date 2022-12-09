using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using TMPro;

public class DBInteraction : MonoBehaviour
{
    public DatabaseReference Reference;
    private string userId;
    public TMP_InputField Username;
    public TMP_InputField Email;

    void Start() {
        // Create new User ID
        userId = SystemInfo.deviceUniqueIdentifier;
    // Get the root reference location of the database.
    Reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void writeNewUser() {
    User newUser = new User(Username.text, Email.text);
    string json = JsonUtility.ToJson(newUser);

    Reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
 
}







