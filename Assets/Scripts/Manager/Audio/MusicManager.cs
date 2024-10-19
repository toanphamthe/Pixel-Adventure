using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Stats")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _backgroundMusic;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _volume;

    private Dictionary<string, AudioClip> _sceneMusicMap;

    public float FadeDuration { get => _fadeDuration; }

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

        _audioSource = GetComponent<AudioSource>();

        _sceneMusicMap = new Dictionary<string, AudioClip>();
        for (int i = 1; i < _backgroundMusic.Count; i++)
        {
            string sceneName = "Lv_" + (i).ToString();
            if (!_sceneMusicMap.ContainsKey(sceneName))
            {
                _sceneMusicMap.Add(sceneName, _backgroundMusic[i]);
            }
        }
    }

    private void Start()
    {
        PlayMusic(_backgroundMusic[0]);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        _volume = Mathf.Clamp(_volume, 0, 100);
        _audioSource.volume = _volume / 100;
    }

    private void PlayMusic(AudioClip musicClip)
    {
        if (_audioSource.clip != musicClip)
        {
            _audioSource.clip = musicClip;
            _audioSource.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_sceneMusicMap.ContainsKey(scene.name))
        {
            StartCoroutine(TransitionMusic(_sceneMusicMap[scene.name]));
        }
    }

    private void FadeOutMusic()
    {
        StartCoroutine(FadeOut(_fadeDuration));
    }

    private IEnumerator TransitionMusic(AudioClip newMusic)
    {
        FadeOutMusic();
        yield return new WaitForSeconds(FadeDuration);
        PlayMusic(newMusic);
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        float startVolume = _audioSource.volume;

        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        _audioSource.volume = startVolume;
    }
}
