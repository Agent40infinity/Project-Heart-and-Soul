using DevLocker.Utils;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;

public class StartEditor
{
    [InitializeOnEnterPlayMode]
    public static void OnPlayModeStart()
    {
        StartEditor.WaitForPersistent(EditorSceneManager.GetActiveScene().path);
    }

    public static async void WaitForPersistent(string editorScenePath)
    {
        SceneAsset firstScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
        EditorSceneManager.playModeStartScene = firstScene;

        while (PassageManager.Instance == null)
        {
            await System.Threading.Tasks.Task.Delay(10);
        }

        if (editorScenePath.Equals(EditorBuildSettings.scenes[0].path))
        {
            return;
        }

        foreach (var scene in PassageManager.Instance.exclusionScenes)
        {
            if (scene.m_SceneAsset != firstScene)
            {
                EditorSceneManager.LoadSceneAsync(scene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
        }

        SceneHandle[] handles = GetAllInstancesOfType("Assets", typeof(SceneHandle));
        SceneHandle selected = handles
            .First(h => h.scene.Initial != null ? 
            h.scene.Initial.ScenePath == editorScenePath : 
            h.scene.Alt.Any(s => s.ScenePath == editorScenePath));

        PassageManager.Instance.CallSceneTransition(selected);
    }

    private static SceneHandle[] GetAllInstancesOfType(string activePath, System.Type activeType)
    {
        string[] guids = AssetDatabase.FindAssets("t:" + activeType.Name, new[] { activePath });
        SceneHandle[] a = new SceneHandle[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = (SceneHandle)AssetDatabase.LoadAssetAtPath(path, activeType);
        }
        return a;
    }
}