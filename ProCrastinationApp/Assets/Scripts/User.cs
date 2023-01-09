using System;
using UnityEngine;


//Serializable, so unity will be able to convert to json.
[System.Serializable]
public class User {
    public string username;

    public int gold;

    public int level;
    public int exp;
    public string useriD;

    /* public User(string username, string email) {
    this.username = username;
    this.email = email;    
    } */

    public User(int level, int exp, string username, int gold, string useriD) {
    this.level = level;
    this.exp = exp;
    this.username = username;
    this.gold = gold;
    this.useriD = useriD;
    }

    public int getLevel() {
    return level;
    }

    public void setLevel(int _level) {
    this.level = _level;
  }

    
} 





