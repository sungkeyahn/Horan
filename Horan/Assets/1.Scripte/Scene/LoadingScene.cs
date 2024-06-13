    using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScene : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    float LoadingProgress;

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime; 
            if (op.progress < 0.9f)
            {
                LoadingProgress = Mathf.Lerp(LoadingProgress, op.progress, timer);
                if (LoadingProgress >= op.progress)
                { 
                    timer = 0f;
                } 
            } 
            else 
            {
                LoadingProgress = Mathf.Lerp(LoadingProgress, 1f, timer);
                if (LoadingProgress == 1.0f)
                { 
                    op.allowSceneActivation = true; 
                    yield break; 
                } 
            }
        }
    }
}
