using System;
using DefaultNamespace.ScheduleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private GameObject _nightIcon;
    [SerializeField] private GameObject _dayIcon;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _dayText;

    private void Start()
    {
        var timeManager = GameTimeManager.Instance;
        timeManager.OnDayChanged += UpdateDayText;
        timeManager.OnHourChanged += UpdateIcons;
    }

    private void Update()
    {
        _fillImage.fillAmount = (GameTimeManager.Instance.CurrentHour + GameTimeManager.Instance.HourProgress) / 24f;
    }

    private void UpdateIcons(int arg1, int arg2)
    {
        if (arg2 is >= 6 and < 21)
        {
            // Daytime
            _dayIcon.SetActive(true);
            _nightIcon.SetActive(false);
        }
        else
        {
            // Nighttime
            _dayIcon.SetActive(false);
            _nightIcon.SetActive(true);
        }
    }

    private void UpdateDayText(int obj)
    {
        _dayText.text = "Day " + obj;
    }
}