using TMPro;
using UnityEngine;

public class ConstructTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI body1;
    [SerializeField] private TextMeshProUGUI body2;
    [SerializeField] private TextMeshProUGUI body3;

    void Awake()
    {
        ClearText();
        Hide();
    }

    public void SetText(string h, string b1, string b2, string b3)
    {
        header.text = h;
        body1.text = b1;
        body2.text = b2;
        body3.text = b3;
    }

    public void ClearText()
    {
        header.text = "";
        body1.text = "";
        body2.text = "";
        body3.text = "";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
