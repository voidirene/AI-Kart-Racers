using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private Transform pauseModal;
    [SerializeField] private UnityEvent pauseGame;
    [SerializeField] private UnityEvent unpauseGame;
    private bool isPaused;

    private Transform winModal;
    [SerializeField] private UnityEvent winGame;

    [SerializeField] private UnityEvent loseGame;

    private Transform optionsObject;

    private void Awake()
    {
        // if in a level, get the pause modal
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            pauseModal = GameObject.Find("PauseModal").transform;
            winModal = GameObject.Find("WinModal").transform;
        }
        else
            Debug.Log("No pause/win modal found. You may not be in a level or the modal object(s) is/are missing.");
    }

    private void Start()
    {
        if (pauseModal != null)
            pauseModal.gameObject.SetActive(false);
        if (winModal != null)
            winModal.gameObject.SetActive(false);
        if (optionsObject != null)
            optionsObject.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    private void Update()
    {
        //pausing method
        if (Input.GetKeyDown(KeyCode.P) && SceneManager.GetActiveScene().name.Contains("Level"))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else if (isPaused)
            {
                UnpauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            QuitGame();

        if (Input.GetKeyDown(KeyCode.R))
            RestartLevel();
    }

    public void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0;
        pauseGame.Invoke();
    }
    public void UnpauseGame()
    {
        isPaused = false;

        Time.timeScale = 1;
        unpauseGame.Invoke();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameWin()
    {
        winGame.Invoke();
        Time.timeScale = 0;
    }

    public void GameLose()
    {
        loseGame.Invoke();
        Time.timeScale = 0;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}