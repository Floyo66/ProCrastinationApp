using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using System;

public class LevelSystem : MonoBehaviour
{
    public int level = 1;
    public float maxLevel;
    public float currentXp;
    public int nextLevelXp = 100;
    [Header("Multipliers")]
    [Range(1f, 300f)]
    public float additionMultiplier;
    [Range(2f, 4f)]
    public float powerMultiplier = 20f;
    [Range(7f, 14f)]
    public float divisionMultiplier = 7f;


    public TextMeshProUGUI levelText;
    public TextMeshProUGUI XpText;

    private string userId;
    public DatabaseReference Reference;


    void Start()
    {

        // Create new User ID
        userId = SystemInfo.deviceUniqueIdentifier;
        // Get the root reference location of the database.
        Reference = FirebaseDatabase.DefaultInstance.RootReference;
        levelText.text = "Level " + level;
        // DB level = ;
        XpText.text = Mathf.Round(currentXp) + "/" + Mathf.Round(nextLevelXp);
        nextLevelXp = CalculateNextLevelXp();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetUserInfo();
        UpdateLevel();
        UpdateExp();
        if (level != maxLevel)
        {
            if (currentXp >= nextLevelXp)
            {
                LevelUp();
            }        
        }
        else
        {
            currentXp = nextLevelXp;
            XpText.text = "MAX";
           
        }
    }

    

    public void GainExperienceFlatRate(float xpGained)
    {
            currentXp += xpGained;
    }

    public void GainExperienceScalable(float xpGained, int passedLevel)
    {
        if (passedLevel < level)
        {
            float multiplier = 1 + (level - passedLevel) * 0.1f;
            currentXp += Mathf.Round(xpGained*multiplier);

        }
        else
        {
            currentXp += xpGained;

        }
    }   

      public void LevelUp() 
    {
        level += 1;
        currentXp = Mathf.Round(currentXp-nextLevelXp);

        nextLevelXp = CalculateNextLevelXp();
        level = Mathf.Clamp(level,0, 50);

        //XpText.text = Mathf.Round(currentXp) + "/" + nextLevelXp;
        //levelText.text = "Level " + level;

    }

    private int CalculateNextLevelXp() 
    {
        int solveForRequiredXp = 0;
        for (int levelCycle = 1; levelCycle <= level; levelCycle++)
        {
            solveForRequiredXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiredXp / 4;
    }

    public void writeNewUser() {
    User newUser = new User(levelText.text, XpText.text);
    string json = JsonUtility.ToJson(newUser);

    Reference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }  


    public IEnumerator GetLevel(Action<int> onCallback)
    {
        var levelData = Reference.Child("users").Child(userId).Child("level").GetValueAsync();

        yield return new WaitUntil(predicate: () => levelData.IsCompleted);

        if(levelData != null)
        {
            DataSnapshot snapshot = levelData.Result;
            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }

    public IEnumerator GetExp(Action<float> onCallback)
    {
        var expData = Reference.Child("users").Child(userId).Child("exp").GetValueAsync();

        yield return new WaitUntil(predicate: () => expData.IsCompleted);

        if(expData != null)
        {
            DataSnapshot snapshot = expData.Result;
            onCallback.Invoke(float.Parse(snapshot.Value.ToString()));
        }
    }

    public void GetUserInfo()
    {
        StartCoroutine(GetLevel((int level) =>{
            levelText.text = "Level: " + level;
        }));

        StartCoroutine(GetExp((float xpGained) =>{
           XpText.text = Mathf.Round(currentXp) + "/" + nextLevelXp;
        }));

    }


    public void UpdateLevel()
    {
        Reference.Child("users").Child(userId).Child("level").SetValueAsync(levelText.text);
    }

    public void UpdateExp()
    {
       Reference.Child("users").Child(userId).Child("exp").SetValueAsync(XpText.text);  
    }
    
}
