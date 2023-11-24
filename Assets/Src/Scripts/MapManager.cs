using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


[DefaultExecutionOrder(-2)]
public class MapManager : MonoBehaviour
{
    public Scene CurrentScene => _currentScene;
    private Scene _currentScene;

    public void SpawnLevel(int currentLevel)
    {
        if (_currentScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(_currentScene);
        }

        int levelToLoad = currentLevel;
        if (levelToLoad > SceneManager.sceneCountInBuildSettings - 1)
        {
            levelToLoad = Random.Range(1, SceneManager.sceneCountInBuildSettings);
        }

        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);
        _currentScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
    }

    private void OnDestroy()
    {
        if (_currentScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(_currentScene);
        }
    }
}