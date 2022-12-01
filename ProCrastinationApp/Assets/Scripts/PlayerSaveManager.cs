using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveManager : MonoBehaviour
{
    // Key that will define the player
    private const string PLAYER_KEY = "PLAYER_KEY";

    private FirebaseDatabase _database;

    private void Start() {
        _database = FirebaseDatabase.DefaultInstance;
    }

    // Function to turn the playerdata into JSON to send to the database later
    public void SavePlayer(PlayerData player) {
        PlayerPrefs.SetString(PLAYER_KEY, JsonUtility.ToJson(player));
        _database.GetReference(PLAYER_KEY).SetRawJsonValueAsync(JsonUtility.ToJson(player));
    }

    // Check if there is an existing player and loads their progress
    public async Task<PlayerData?> LoadPlayer() {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        if (!dataSnapshot.Exists) {
            return null;
        };
        return JsonUtility.FromJson<PlayerData>(dataSnapshot.GetRawJsonValue());
    }

    public async Task<bool> SaveExists()
    {
        var dataSnapshot = await _database.GetReference(PLAYER_KEY).GetValueAsync();
        return dataSnapshot.Exists;
    }

    public void EraseSave(){
        PlayerPrefs.DeleteKey(PLAYER_KEY);
        _database.GetReference(PLAYER_KEY).RemoveValueAsync();
    }
}
