using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, 1));
    }

    IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        SceneManager.LoadScene(sceneName);
    }
}
