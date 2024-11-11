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

    [SerializeField] private Animator _loadingAnimator;
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

    /// <summary>
    /// Load async a scene with a loading animation.
    /// </summary>
    /// <param name="sceneName"></param>
    public async void LoadScene(string sceneName)
    {
        _sceneProgress = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        _loadingAnimator.SetTrigger("start");
        scene.allowSceneActivation = false;
        await Task.Delay(1000);

        do
        {
            await Task.Delay(100);
            _sceneProgress = scene.progress;
        }
        while (scene.progress < 0.9f);

        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        _loadingAnimator.SetTrigger("end");
    }
}
