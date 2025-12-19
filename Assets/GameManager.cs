using System.Collections;
using DefaultNamespace;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private MMF_Player _fadeFeedback;

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneCoroutine(sceneName, 1));
    }

    IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }

    public void Win()
    {
        if(_fadeFeedback) _fadeFeedback.PlayFeedbacks();
        LoadScene("End Game Cutscene");
    }

    public void Loose()
    {
        if(_fadeFeedback) _fadeFeedback.PlayFeedbacks();
        LoadScene("Lose Scene");
    }
}