using System.Collections;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.04f;
    [SerializeField] private float warningBlinkSpeed = 0.3f;
    private Coroutine warningCoroutine;
    private Coroutine typingCoroutine;

    public void PlayText(string fullText)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string fullText)
    {
        textUI.text = "";
        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void Clear()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textUI.text = "";
    }
    public void PlayWarningSignal()
    {
        StopAllTextCoroutines();
        warningCoroutine = StartCoroutine(WarningLoop());
    }
    
    public void StopWarningSignal()
    {
        StopAllTextCoroutines();
        textUI.text = "";
    }
    
    
    private IEnumerator WarningLoop()
    {
        const string baseText = "Warning";

        int exclamationCount = 1;

        while (true)
        {
            textUI.text = baseText + new string('!', exclamationCount);
            exclamationCount = exclamationCount % 3 + 1;

            yield return new WaitForSeconds(warningBlinkSpeed);
        }
    }
    
    private void StopAllTextCoroutines()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (warningCoroutine != null)
            StopCoroutine(warningCoroutine);
    }
}