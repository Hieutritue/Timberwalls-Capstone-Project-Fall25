using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragClickButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            button.onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0)) // left mouse held
            button.onClick.Invoke();
    }
}