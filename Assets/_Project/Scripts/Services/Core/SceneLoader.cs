using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string FaderPath = "Fader";

    private static SceneLoader s_instance;

    [SerializeField] private Fader _fader;

    private bool _isLoading;

    public static SceneLoader Instance
    {
        get
        {
            if (s_instance == null)
            {
                SceneLoader prefab = Resources.Load<SceneLoader>(FaderPath);

                if (prefab == null)
                    throw new System.Exception($"SceneLoader prefab not found at path: {FaderPath}");

                s_instance = Instantiate(prefab);
                DontDestroyOnLoad(s_instance.gameObject);
                s_instance.gameObject.SetActive(false);
            }

            return s_instance;
        }
    }

    private void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);

            return;
        }

        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ReloadCurrentScene()
    {
        if (_isLoading)
            return;

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        string name = GetSceneNameByIndex(currentScene);
        LoadScene(name);
    }

    public void LoadScene(string name)
    {
        if (_isLoading)
            return;

        gameObject.SetActive(true);
        StartCoroutine(LoadSceneRoutine(name));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        _isLoading = true;
        bool waitFading = true;
        _fader.PlayFadeIn(() => waitFading = false);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (waitFading || async.progress < 0.9f)
            yield return null;

        async.allowSceneActivation = true;
        waitFading = true;
        _fader.PlayFadeOut(() => waitFading = false);

        while (waitFading)
            yield return null;

        _isLoading = false;
        gameObject.SetActive(false);
    }

    private string GetSceneNameByIndex(int buildIndex)
    {
        if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
            throw new System.ArgumentOutOfRangeException(nameof(buildIndex));

        string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);

        return System.IO.Path.GetFileNameWithoutExtension(scenePath);
    }
}