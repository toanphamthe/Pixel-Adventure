using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

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

    /// <summary>
    /// Save the checkpoint when the player triggers it
    /// </summary>
    /// <param name="checkpoint"></param>
    public void SaveCheckpoint(Checkpoint checkpoint)
    {
        _checkedpoints = checkpoint;
    }

    /// <summary>
    /// Load the last checkpoint when the player dies
    /// </summary>
    /// <param name="player"></param>
    public void LoadLastCheckpoint(Player player)
    {
        player.gameObject.transform.position = _checkedpoints.transform.position;
    }

    /// <summary>
    /// Handle the win game event
    /// </summary>
    /// <param name="level"></param>
    public void WinGame(int level)
    {
        if (!SceneManager.GetSceneByName("Win").isLoaded)
        {
            SceneManager.LoadSceneAsync("Win", LoadSceneMode.Additive);
        }

        PlayerPrefs.SetInt(PlayerPrefsKeys.LevelCompletedKey(level), 1);
        PlayerPrefs.SetInt(PlayerPrefsKeys.LevelUnlockedKey(level + 1), 1);

        Diamond diamond = GameObject.FindGameObjectWithTag("Player").GetComponent<Diamond>();
        if (diamond != null)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.Diamonds, diamond.CurrentPoint);
        }
        else
        {
            Debug.LogError("Diamond not found");
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Handle the lose game event
    /// </summary>
    public void LoseGame()
    {
        SceneManager.LoadSceneAsync("Lose", LoadSceneMode.Additive);
    }
}
