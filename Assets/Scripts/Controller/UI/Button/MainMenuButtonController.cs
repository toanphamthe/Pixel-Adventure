using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    public void ClosePanel(GameObject panel)
    {
        panel.gameObject.SetActive(false);
    }

    public void OpenPanel(GameObject panel)
    {
        panel.gameObject.SetActive(true);
    }

    public void StartGame(string level)
    {
        SceneManager.LoadScene("Lv" + level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
