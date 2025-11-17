using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour
{
    private BuildingCategory categoryData;
    private BuildMenuManager menuManager;
    [field: SerializeField] public Button Button { get; private set; }
    public BuildingCategory CategoryData => categoryData;

    public void Initialize(BuildingCategory category, BuildMenuManager manager)
    {
        categoryData = category;
        menuManager = manager;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = StringTools.SplitCamelCase(category.ToString());
        Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        menuManager.OnClickCategory(this);
    }

    public void SetSelected(bool isSelected)
    {
        Button.interactable = !isSelected;
    }
}