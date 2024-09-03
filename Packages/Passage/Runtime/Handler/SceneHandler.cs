using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using DevLocker.Utils;

public static class SceneHandler
{
    public static List<AsyncOperation> LoadScenes(List<SceneReference> loadingScenes)
    {
        List<AsyncOperation> output = new List<AsyncOperation>();

        foreach (var scene in loadingScenes)
        {
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                output.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            }
        }

        return output;
    }

    public static List<AsyncOperation> LoadScenes(SceneReference loadingScene)
    {
        List<AsyncOperation> output = new List<AsyncOperation>();

        if (!SceneManager.GetSceneByName(loadingScene).isLoaded)
        {
            output.Add(SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive));
        }

        return output;
    }

    public static List<AsyncOperation> UnloadAllScenes(List<SceneReference> exclusionScenes)
    {
        List<AsyncOperation> output = new List<AsyncOperation>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            string buildIndex = SceneManager.GetSceneAt(i).name;
            bool exclusion = false;

            for (int j = 0; j < exclusionScenes.Count; j++)
            {
                switch (true)
                {
                    case bool a when buildIndex == exclusionScenes[j]:
                        exclusion = true;
                        break;
                }
            }

            switch (exclusion)
            {
                case false:
                    output.Add(SceneManager.UnloadSceneAsync(buildIndex));
                    break;
            }
        }

        return output;
    }

    public static List<AsyncOperation> SwapScenes(List<SceneReference> loadingScenes, List<SceneReference> exclusionScenes)
    {
        return new List<AsyncOperation>(UnloadAllScenes(exclusionScenes).Concat(LoadScenes(loadingScenes)));
    }

    public static List<AsyncOperation> SwapScenes(SceneReference loadingScenes, List<SceneReference> exclusionScenes)
    {
        return new List<AsyncOperation>(UnloadAllScenes(exclusionScenes).Concat(LoadScenes(loadingScenes)));
    }

    public static IEnumerator SetActive(SceneReference activeScene)
    {
        Scene active = SceneManager.GetSceneByName(activeScene);
        SceneManager.SetActiveScene(active);

        yield return null;
    }

    public static List<AsyncOperation> ReloadScene(SceneReference sceneToReload)
    {
        return new List<AsyncOperation>
            {
                SceneManager.UnloadSceneAsync(sceneToReload),
                SceneManager.LoadSceneAsync(sceneToReload, LoadSceneMode.Additive)
            };
    }
}