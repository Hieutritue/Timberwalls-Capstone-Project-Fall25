using System;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ItemWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject subcategoryLayer;
    [SerializeField] private GameObject subcategoryPrefab;
    [SerializeField] private TextMeshProUGUI categoryName;
    public static ItemWindowManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void GenerateItems(CategorySO categoryClicked)
    {
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        categoryName.text = StringTools.SplitCamelCase(categoryClicked.category.ToString());
        //generate category
        foreach (var subCategory in categoryClicked.subCategories)
        {
            var newSubCategory = Instantiate(subcategoryPrefab, subcategoryLayer.transform);
            GameObject subcategoryItemContainer = newSubCategory.GetComponent<Subcategory>().GetItemContainer();
            TextMeshProUGUI subcategoryName = newSubCategory.GetComponent<Subcategory>().GetSubcategoryName();
            subcategoryName.SetText(subCategory.subCategoryName);
            //generate item in the category
            foreach (var item in subCategory.items)
            {
                var newItem = Instantiate(item.Prefab, subcategoryItemContainer.transform);
                if(item.Icon != null) newItem.GetComponent<Image>().sprite = item.Icon;
                
            }
            
            //resize the subcategoryPrefab to match item container
            RectTransform subcategoryPrefabRect = newSubCategory.GetComponent<RectTransform>();
            RectTransform subcategoryItemContainerRect = subcategoryItemContainer.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(subcategoryItemContainerRect);
            float subcategoryPrefabHeight = subcategoryPrefabRect.rect.height;
            float newSubcategoryPrefabHeight = subcategoryPrefabHeight + subcategoryItemContainerRect.rect.height; 
            subcategoryPrefabRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSubcategoryPrefabHeight);
            
        }
        //resize menu
        RectTransform rectTransform = GetComponent<RectTransform>();
        RectTransform subcategoryLayerRectTransform = subcategoryLayer.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(subcategoryLayerRectTransform);
        float extraHeight = subcategoryLayer.GetComponent<RectTransform>().rect.height;
        float newHeight = rectTransform.rect.height + extraHeight;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
    
}