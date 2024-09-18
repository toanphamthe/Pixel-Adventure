using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private const string _levelKey = "Lv_";

    public static GameManager Instance { get; private set; }

    [SerializeField] private Checkpoint _checkedpoints;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCheckpoint(Checkpoint checkpoint)
    {
        _checkedpoints = checkpoint;
    }

    public void LoadLastCheckpoint(Player player)
    {
        player.gameObject.transform.position = _checkedpoints.transform.position;
    }

    public void WinGame(int level)
    {
        if (!SceneManager.GetSceneByName("Win").isLoaded)
        {
            SceneManager.LoadSceneAsync("Win", LoadSceneMode.Additive);
        }

        PlayerPrefs.SetInt("Lv_" + level.ToString() + "_Completed", 1);
        PlayerPrefs.SetInt("Lv_" + (level + 1).ToString() + "_Unlocked", 1);
        PlayerPrefs.Save();
    }

    public void LoseGame()
    {
        Debug.Log("Lose");
    }
}
