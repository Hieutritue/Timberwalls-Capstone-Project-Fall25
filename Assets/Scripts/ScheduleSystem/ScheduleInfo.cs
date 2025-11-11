using UnityEngine;
using UnityEngine.UI;

public class ScheduleInfo : MonoBehaviour
{
    public ScheduleType Type;
    public Button Button;
    public Sprite Icon;
    public ScheduleMenu ScheduleMenu;

    void Start()
    {
        Button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        ScheduleMenu.SelectSchedule(this);
    }
}