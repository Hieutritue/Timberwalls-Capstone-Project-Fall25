using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIButtonSelector : MonoBehaviour
{
    [Header("Assign your buttons here (in order)")]
    public List<Button> buttons;

    [Header("Visual settings")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;

    private int selectedIndex = 0;

    void Start()
    {
        // Set up listeners
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }

        // Make sure one is selected by default
        UpdateButtonVisuals();
    }

    void OnButtonClicked(int index)
    {
        if (selectedIndex == index) return; // already selected

        selectedIndex = index;
        UpdateButtonVisuals();
    }

    void UpdateButtonVisuals()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i == selectedIndex) ? selectedColor : normalColor;
            buttons[i].colors = colors;
        }
    }

    public int GetSelectedIndex() => selectedIndex;
}