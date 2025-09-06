using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Scene]
    public string sceneReference;

    public void OpenScene()
    {
        SceneManager.LoadScene(sceneReference, LoadSceneMode.Additive);
    }
}



