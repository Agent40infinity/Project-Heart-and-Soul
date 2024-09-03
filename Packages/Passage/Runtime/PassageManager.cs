using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DevLocker.Utils;

public class PassageManager : MonoBehaviour
{
    public static PassageManager instance;
    [SerializeField] private bool inLoading = false;
    [SerializeField] private UnityEvent enterEvent;
    [SerializeField] private UnityEvent exitEvent;
    [SerializeField] public List<SceneReference> exclusionScenes;

    [Header("Key References")]
    public IFade fade;

    public PassageManager Instance
    {
        get { return instance; }
        private set { instance = this; }
    }

    public void Awake()
    {
        Instance = this;
    }

    public void CallSceneTransition(SceneHandle data)
    {
        StartCoroutine(IntitiateLoad(data));
    }

    public IEnumerator IntitiateLoad(SceneHandle data)
    {
        yield return StartCoroutine(StartLoad());

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

        SceneHandler.SwapScenes(data.scene.Value(), exclusionScenes);

        yield return StartCoroutine(LoadProgression(scenesLoading, null));
    }

    public IEnumerator StartLoad()
    {
        inLoading = true;

        if (fade != null)
        {
            yield return fade.FadeIn();
        }

        enterEvent.Invoke();

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator LoadProgression(List<AsyncOperation> scenesLoading, SceneReference activeScene)
    {
        yield return StartCoroutine(SceneLoadProgress(scenesLoading));

        if (activeScene != null)
        {
            yield return StartCoroutine(SceneHandler.SetActive(activeScene));
        }

        yield return WaitForLoadEvents();

        exitEvent.Invoke();

        if (fade != null)
        {
            yield return fade.FadeOut();
        }

        yield return new WaitForEndOfFrame();
        inLoading = false;
    }

    public IEnumerator SceneLoadProgress(List<AsyncOperation> scenesLoading)
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }
    }

    public IEnumerator WaitForLoadEvents()
    {
        yield return null;
        /*while (loadingEvents.GetPersistentEventCount() > 0)
        {
            yield return null;
        }*/
    }
}