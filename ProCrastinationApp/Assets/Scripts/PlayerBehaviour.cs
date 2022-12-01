using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Class to update the player's behaviours
public class PlayerBehaviour : MonoBehaviour
{
    // Import PlayerData by creating an update function that sets this data equal to the current Data
    [SerializeField]
    private PlayerData _playerData;

    public PlayerData PlayerData => _playerData;

    public string Name => _playerData.Name;

    public int Score => _playerData.Score;

    public UnityEvent OnPlayerUpdated = new UnityEvent();

    public void UpdatePlayer(PlayerData playerData) {
        if (!playerData.Equals(_playerData)) {
            _playerData = playerData;
            OnPlayerUpdated.Invoke();
        }
    }

}
