using System.Collections;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float typingSpeed = 0.04f;

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
}