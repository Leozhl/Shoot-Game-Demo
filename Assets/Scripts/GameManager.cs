using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action OnGameStart;
    public event Action OnPlayerDeath;
    public event Action OnGameRestart;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Main");
        ao.completed += StartLevel;
    }

    public void StartLevel(AsyncOperation ao)
    {
        if (OnGameStart != null)
            OnGameStart();
    }

    public void PlayerDeath()
    {
        if (OnPlayerDeath != null)
            OnPlayerDeath();
    }

    public void RestartLevel()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Main");
        ao.completed += StartLevel;
        if (OnGameRestart != null)
            OnGameRestart();
    }
}