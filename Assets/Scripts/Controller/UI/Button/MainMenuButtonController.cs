using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _level;

    private void Start()
    {
        LevelSelectionHandle();
    }

    /// <summary>
    /// Handle the level selection
    /// </summary>
    private void LevelSelectionHandle()
    {
        if (PlayerPrefs.GetInt("Lv_1_Unlocked", 0) == 0)
        {
            PlayerPrefs.SetInt("Lv_1_Unlocked", 1);
            PlayerPrefs.Save();
        }

        for (int i = 1; i <= _level.Count; i++)
        {
            if (PlayerPrefs.GetInt("Lv_" + (i) + "_Unlocked") == 1)
            {
                _level[i - 1].SetActive(true);
            }
            else
            {
                _level[i - 1].SetActive(false);
            }
        }
    }

    public void OnLevelButtonPressed(string sceneName)
    {
        LoadingController.Instance.LoadScene(sceneName);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
