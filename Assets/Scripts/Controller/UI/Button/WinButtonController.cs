using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinButtonController : MonoBehaviour
{
    [SerializeField] GameObject _nextButtonObject;

    private void Start()
    {
        ReachTheMaxLevel();
    }

    /// <summary>
    /// Hide the next button if the current level is the last level
    /// </summary>
    private void ReachTheMaxLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            _nextButtonObject.SetActive(false);
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
        LoadingController.Instance.LoadScene("Start");
    }

    /// <summary>
    /// Load the next level
    /// </summary>
    public void OnNextButtonPressed()
    {
        LoadingController.Instance.LoadScene("Lv_" + (SceneManager.GetActiveScene().buildIndex - 1).ToString());
        Time.timeScale = 1;
    }
}
