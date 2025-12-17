using System.Threading.Tasks;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async Task Fade(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            await Task.Yield();
        }

        canvasGroup.alpha = targetAlpha;
    }

    // === Timeline-safe entry points ===

    public void FadeOutSignal()
    {
        _ = Fade(1f); // turn to black 
    }

    public void FadeInSignal()
    {
        _ = Fade(0f); // turn to transparent
    }
}