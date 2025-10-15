using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Transform target; // The object to follow
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    public void SetProgress(float progress)
    {
        fillImage.fillAmount = Mathf.Clamp01(progress);
    }
}