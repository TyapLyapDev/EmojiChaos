using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void ReloadCurrentScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void LoadScene(string name) =>
        SceneManager.LoadScene(name);
}