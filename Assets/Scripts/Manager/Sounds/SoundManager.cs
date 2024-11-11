using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }


    [Header("Sound Controllers")]
    [SerializeField] private MusicController _musicController;
    [SerializeField] private SFXController _soundEffectsController;

    private void Awake()
    {
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
    /// Decrease the background volume effect
    /// </summary>
    /// <param name="sceneName"></param>
    public void DecreaseBackgroundVolumeEffect(string sceneName)
    {
        _musicController.OnSceneTransition(sceneName);
    }

    /// <summary>
    /// Play the Audio SFX
    /// </summary>
    /// <param name="audioClip">Audio</param>
    public void PlaySFX(AudioClip audioClip)
    {
        _soundEffectsController.PlaySFX(audioClip);
    }

    /// <summary>
    /// Play the button SFX
    /// </summary>
    public void PlayButtonSFX()
    {
        _soundEffectsController.PlayButtonSFX();
    }
}
