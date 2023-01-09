using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using System;
using UnityEngine.UI;
using Firebase.Extensions;

public class LevelSystem : MonoBehaviour
{
    
    


    public TextMeshProUGUI levelText, XpText, neededExpText, goldText;
    public int level;
    public int currentXP, targetXP;
    private string userId;
    public DatabaseReference Reference;

    

    


    public static LevelSystem instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void addXP(int xp)
    {
        Reference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.Message);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                levelText.text = snapshot.Child("level").Value.ToString();
                level = int.Parse(levelText.text);

                targetXP = 100 * (int)Math.Pow(1.5,(level-1));

                neededExpText.text = targetXP.ToString();
                XpText.text = snapshot.Child("exp").Value.ToString();
                currentXP = int.Parse(XpText.text);
            }
        
       
       currentXP += xp;
        Reference.Child("users")
                        .Child(userId)
                        .Child("exp")
                        .SetValueAsync(currentXP.ToString());
       XpText.text = currentXP.ToString();

       while(currentXP >= targetXP)
       {
            currentXP = currentXP - targetXP;
            levelUp();
            targetXP += targetXP / 20;

            XpText.text = currentXP.ToString();
            neededExpText.text = targetXP.ToString();
             Reference.Child("users")
                        .Child(userId)
                        .Child("exp")
                        .SetValueAsync(currentXP.ToString());

            
       }
       });
    }


    void Start()
    {

        // Create new User ID
        userId = SystemInfo.deviceUniqueIdentifier;
        // Get the root reference location of the database.
        Reference = FirebaseDatabase.DefaultInstance.RootReference;
       
       loadData();
       addXP(150);
       //Debug.Log("my level is: " + level);
       //saveData();
        //loadData();
        //levelUp();
    
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void levelUp()
    {
        Reference.Child("users").Child(userId).Child("level").GetValueAsync().ContinueWithOnMainThread(task => {
        if (task.IsFaulted)
            {
                Debug.Log(task.Exception.Message);
            }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    levelText.text = snapshot.Value.ToString();
                    level = int.Parse(levelText.text);
                    level = (level+1);
                    Reference.Child("users")
                        .Child(userId)
                        .Child("level")
                        .SetValueAsync(level.ToString());
                    levelText.text = level.ToString();
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
                levelText.text = snapshot.Child("level").Value.ToString();
                XpText.text = snapshot.Child("exp").Value.ToString();
                goldText.text = snapshot.Child("gold").Value.ToString();
                neededExpText.text = targetXP.ToString();
                level = int.Parse(levelText.text);
                targetXP = 100 * (int)Math.Pow(1.5,(level-1));
                neededExpText.text = targetXP.ToString();

            }
        });
    }

    

}
