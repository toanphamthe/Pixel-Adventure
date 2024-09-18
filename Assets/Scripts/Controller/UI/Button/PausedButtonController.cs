using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedButtonController : MonoBehaviour
{
    public void OnResumeButtonPressed()
    {
        SceneManager.UnloadSceneAsync("Pause");
        Time.timeScale = 1;
    }

    public void OnPauseButtonPressed()
    {
        if (SceneManager.GetSceneByName("Pause").isLoaded == false)
        {
            SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
            Time.timeScale = 0;
        }
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void OnMenuButtonPressed()
    {
        SceneManager.LoadScene("Start");
    }
}
