using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI body1;
    [SerializeField] private TextMeshProUGUI body2;

    public void Awake()
    {
        ClearText();
        HideTooltip();
    }

    public void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent.GetComponent<RectTransform>(),
            Input.mousePosition,
            null,
            out position);
        transform.localPosition = position;
    }

    private void SetText(string header, string body1, string body2)
    {
        this.header.text = header;
        this.body1.text = body1;
        this.body2.text = body2;
    }

    public void ClearText()
    {
        header.text = "";
        body1.text = "";
        body2.text = "";
    }

    public void ShowTooltip(string header, string body1, string body2)
    {
        SetText(header, body1, body2);
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}