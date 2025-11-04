using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour
{
    private CategorySO categoryData;
    private BuildMenuManager menuManager;

    public void Initialize(BuildingCategory category, BuildMenuManager manager)
    {
        menuManager = manager;
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = StringTools.SplitCamelCase(category.ToString());
        CategorySO categorySo = CategoryList.instance.GetCategoryDataBasedOnBuildingCategory(category);
        categoryData = categorySo;
        GetComponentInChildren<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        menuManager.OnClickCategory(categoryData);
    }
    
}