using System;
using System.Collections.Generic;
using UnityEngine;

public class CategoryList : MonoBehaviour
{
    public static CategoryList instance;
    
    [SerializeField] private List<CategorySO> categories;

    private void Awake()
    {
        instance = this;
    }

    public List<CategorySO> GetList()
    {
        return instance.categories;
    }
    
    public CategorySO GetCategoryDataBasedOnBuildingCategory(BuildingCategory buildingCategory)
    {
        foreach (var category in categories)
        {
            if (category.category == buildingCategory)
            {
                return category;
            }
        }
        return null;
    }
}
