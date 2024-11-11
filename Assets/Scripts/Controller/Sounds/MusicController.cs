using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [Header("Audio Stats")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _mainMenuBackgroundMusic;
    [SerializeField] private List<AudioClip> _backgroundMusic;
    [SerializeField] private bool _isFadingOut;
    private Dictionary<string, AudioClip> _sceneMusicMap;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _sceneMusicMap = new Dictionary<string, AudioClip>();
        for (int i = 0; i < _backgroundMusic.Count; i++)
        {
            string sceneName = "Lv_" + (i + 1).ToString();
            if (!_sceneMusicMap.ContainsKey(sceneName))
            {
                _sceneMusicMap.Add(sceneName, _backgroundMusic[i]);
            }
        }
    }

    private void Start()
    {
        PlayMusic(_mainMenuBackgroundMusic);
    }

    private void Update()
    {
        if (!_isFadingOut)
        {
            _audioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsKeys.MusicVolume, 0.5f);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="musicClip"></param>
    private void PlayMusic(AudioClip musicClip)
    {
        if (_audioSource.clip != musicClip)
        {
            _audioSource.clip = musicClip;
            _audioSource.Play();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneName"></param>
    public void OnSceneTransition(string sceneName)
    {
        if (_sceneMusicMap.ContainsKey(sceneName))
        {
            StartCoroutine(TransitionMusic(_sceneMusicMap[sceneName]));
        }

        if (sceneName == "Start")
        {
            StartCoroutine(TransitionMusic(_mainMenuBackgroundMusic));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newMusic"></param>
    /// <returns></returns>
    private IEnumerator TransitionMusic(AudioClip newMusic)
    {
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(2);
        PlayMusic(newMusic);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOut()
    {
        float startVolume = _audioSource.volume;
        float targetVolume = 0f;
        float fadeDuration = 2f;
        float currentTime = 0f;
        _isFadingOut = true;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeDuration);
            yield return null;
        }

        _isFadingOut = false;
        _audioSource.volume = startVolume;
    }
}
