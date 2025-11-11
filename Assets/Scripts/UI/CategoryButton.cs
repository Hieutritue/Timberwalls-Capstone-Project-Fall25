using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour
{
    private BuildingCategory categoryData;
    private BuildMenuManager menuManager;

    public void Initialize(BuildingCategory category, BuildMenuManager manager)
    {
        categoryData = category;
        menuManager = manager;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = StringTools.SplitCamelCase(category.ToString());
        GetComponentInChildren<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        menuManager.OnClickCategory(categoryData);
    }
    
}