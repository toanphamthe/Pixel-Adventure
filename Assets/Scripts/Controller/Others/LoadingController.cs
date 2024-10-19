using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    public static LoadingController Instance { get; private set; }

    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private float _sceneProgress;

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

    public async void LoadScene(string sceneName)
    {
        _loadingSlider.value = 0;
        _sceneProgress = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        _loadingCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _sceneProgress = scene.progress;
        }
        while (scene.progress < 0.9f);
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        await Task.Delay(1000);
        _loadingCanvas.SetActive(false);
    }

    private void Update()
    {
        _loadingSlider.value = Mathf.MoveTowards(_loadingSlider.value, _sceneProgress, Time.deltaTime);
    }
}
