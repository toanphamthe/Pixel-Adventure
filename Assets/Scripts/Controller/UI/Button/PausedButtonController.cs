using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedButtonController : MonoBehaviour
{
    /// <summary>
    /// Continue the current level
    /// </summary>
    public void OnResumeButtonPressed()
    {
        SceneManager.UnloadSceneAsync("Pause");
        Time.timeScale = 1;
    }

    /// <summary>
    /// Pause the current level
    /// </summary>
    public void OnPauseButtonPressed()
    {
        if (SceneManager.GetSceneByName("Pause").isLoaded == false)
        {
            SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Restart the current level
    /// </summary>
    public void OnRestartButtonPressed()
    {
        LoadingController.Instance.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Go back to the main menu
    /// </summary>
    public void OnMenuButtonPressed()
    {
        Time.timeScale = 1;
        LoadingController.Instance.LoadScene("Start");
    }
}
