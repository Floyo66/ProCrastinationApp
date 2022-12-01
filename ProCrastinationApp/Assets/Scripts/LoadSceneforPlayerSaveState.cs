using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneforPlayerSaveState : MonoBehaviour
{
    [SerializeField] private PlayerSaveManager _playerSaveManager;
    [SerializeField] private string _sceneForSaveExits;
    [SerializeField] private string _sceneForNoSave;

    private Coroutine _coroutine;

    public void Trigger() {
        if (_coroutine == null) {
            _coroutine = StartCoroutine(LoadSceneCoroutine());

        }
    }

    private IEnumerator LoadSceneCoroutine() {
        var saveExistsTask = _playerSaveManager.SaveExists();
        yield return new WaitUntil(() => saveExistsTask.IsCompleted);

        if (saveExistsTask.Result) {
            SceneManager.LoadScene(_sceneForSaveExits);
        } else {
            SceneManager.LoadScene(_sceneForNoSave);
        }
    }
}
