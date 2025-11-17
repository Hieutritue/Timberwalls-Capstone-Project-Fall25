using System;
using System.Collections.Generic;
using DefaultNamespace.UI.Build;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject categoryPrefab;
    [SerializeField] private SubCategoryPanel subCategoryPanelPrefab;
    [SerializeField] private GameObject menuContent;
    [SerializeField] private GameObject itemWindow;
    [SerializeField] private GameObject subCategoryParent;
    [SerializeField] private VerticalLayoutGroup subCategoryLayoutGroup;

    private Dictionary<BuildingCategory, List<SubCategoryPanel>> _categoryToSubCategories
        = new Dictionary<BuildingCategory, List<SubCategoryPanel>>();

    private CategoryButton _currentCategoryButtonSelected;

    private void Start()
    {
        GenerateCategories();
        // gameObject.SetActive(false);
        // click first category by default
        _currentCategoryButtonSelected.Button.onClick.Invoke();
        _currentCategoryButtonSelected.Button.Select();
    }

    private void GenerateCategories()
    {
        foreach (BuildingCategory category in Enum.GetValues(typeof(BuildingCategory)))
        {
            GameObject categoryObj = Instantiate(categoryPrefab, menuContent.transform);
            LoadSubCategories(category);
            CategoryButton categoryButton = categoryObj.GetComponent<CategoryButton>();
            categoryButton.Initialize(category, this);
            if (!_currentCategoryButtonSelected)
                _currentCategoryButtonSelected = categoryButton;
        }
    }

    public void OnClickCategory(CategoryButton categoryClicked)
    {
        // Deactivate previous subcategories
        SetActiveSubCategories(_currentCategoryButtonSelected.CategoryData);

        if (_currentCategoryButtonSelected != categoryClicked)
            _currentCategoryButtonSelected.SetSelected(false);
        _currentCategoryButtonSelected = categoryClicked;
        _currentCategoryButtonSelected.SetSelected(true);

        // Activate new subcategories
        SetActiveSubCategories(_currentCategoryButtonSelected.CategoryData);

        // Force layout rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate(subCategoryParent.transform as RectTransform);
    }

    private void SetActiveSubCategories(BuildingCategory category)
    {
        foreach (var kvp in _categoryToSubCategories)
        {
            bool isActive = kvp.Key == category;
            foreach (var subCategoryPanel in kvp.Value)
            {
                subCategoryPanel.gameObject.SetActive(isActive);
            }
        }
    }

    private void LoadSubCategories(BuildingCategory buildingCategory)
    {
        // Load new subcategories
        foreach (var subCategory in buildingCategory.GetSubcategories())
        {
            SubCategoryPanel subCategoryPanel = Instantiate(subCategoryPanelPrefab, subCategoryParent.transform);
            subCategoryPanel.Initialize(subCategory);
            if (!_categoryToSubCategories.ContainsKey(buildingCategory))
            {
                _categoryToSubCategories[buildingCategory] = new List<SubCategoryPanel>();
            }

            _categoryToSubCategories[buildingCategory].Add(subCategoryPanel);
            // Further initialization can be done here
        }
    }
}