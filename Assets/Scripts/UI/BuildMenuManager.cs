using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildMenuManager : MonoBehaviour
{
    [SerializeField]private GameObject categoryPrefab;
    [SerializeField]private GameObject menuContent;
    [SerializeField] private GameObject itemWindow;
    private CategorySO _currentCategory;

    private void Start()
    {
        GenerateCategories();
    }

    private void GenerateCategories()
    {
        foreach (BuildingCategory category in Enum.GetValues(typeof(BuildingCategory)))
        {
            GameObject categoryObj = Instantiate(categoryPrefab, menuContent.transform);
            CategoryButton categoryButton = categoryObj.GetComponent<CategoryButton>();
            categoryButton.Initialize(category, this);
        }
    }

    public void OnClickCategory(CategorySO categoryClicked)
    {
        bool isDifferentCategory;
        if (_currentCategory == null) isDifferentCategory = true;
        else if (categoryClicked.category == _currentCategory.category)  isDifferentCategory = false;
        else   isDifferentCategory = false;
        
        if (isDifferentCategory)
        {
            _currentCategory = categoryClicked;
            ItemWindowManager.Instance.GenerateItems(categoryClicked);
        }
    }
    
}
