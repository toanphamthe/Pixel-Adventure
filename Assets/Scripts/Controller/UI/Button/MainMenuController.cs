using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _level;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TextMeshProUGUI _diamondStockText;

    private void Awake()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume, 0.5f);
        _sfxSlider.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFXVolume, 0.5f);
    }

    private void Start()
    {
        LevelSelectionHandle();
        _diamondStockText.text = PlayerPrefs.GetInt(PlayerPrefsKeys.Diamonds, 0).ToString();
    }

    private void Update()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKeys.MusicVolume, _musicSlider.value);
        PlayerPrefs.SetFloat(PlayerPrefsKeys.SFXVolume, _sfxSlider.value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Handle the level selection  
    /// </summary>
    private void LevelSelectionHandle()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKeys.LevelUnlockedKey(1), 0) == 0)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.LevelUnlockedKey(1), 1);
            PlayerPrefs.Save();
        }

        for (int i = 1; i <= _level.Count; i++)
        {
            if (PlayerPrefs.GetInt(PlayerPrefsKeys.LevelUnlockedKey(i)) == 1)
            {
                _level[i - 1].SetActive(true);
            }
            else
            {
                _level[i - 1].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void PlayButtonSFX()
    {
        SoundManager.Instance.PlayButtonSFX();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneName"></param>
    public void OnLevelButtonPressed(string sceneName)
    {
        LoadingController.Instance.LoadScene(sceneName);
        SoundManager.Instance.DecreaseBackgroundVolumeEffect(sceneName);
        PlayButtonSFX();
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
