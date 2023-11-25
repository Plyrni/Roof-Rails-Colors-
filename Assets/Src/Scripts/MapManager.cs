using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


[DefaultExecutionOrder(-2)]
public class MapManager : MonoBehaviour
{
    public Scene CurrentScene => _currentScene;
    public Transform MapTransform => _currentScene.GetRootGameObjects()[0].transform; // That's a quick fix for an "android only" bug (cf "GetMap"). I know it isn't optimal


    private Scene _currentScene;
    private Map _currentMap;

#if UNITY_EDITOR
    [SerializeField] private int forceLoad = 0;
#endif

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

#if UNITY_EDITOR
        if (forceLoad > 0)
        {
            levelToLoad = forceLoad;
        }
#endif

        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);
        _currentScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
    }

    /// <summary>
    /// Bugged on android 
    /// </summary>
    /// <returns></returns>
    private Map GetMap()
    {
        try
        {
            if (_currentMap == null || _currentMap is null)
            {
                var rootGameObjects = _currentScene.GetRootGameObjects();
                if (rootGameObjects.Length > 0)
                {
                    _currentMap = rootGameObjects[0].GetComponentInChildren<Map>();
                    if (_currentMap == null)
                    {
                        Debug.LogError("Map component not found in the children of the root GameObject.");
                    }
                }
                else
                {
                    Debug.LogError("No root GameObjects found in the current scene.");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception in GetMap: " + e.Message);
        }

        return _currentMap;
    }

    private void OnDestroy()
    {
        if (_currentScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(_currentScene);
        }
    }
}